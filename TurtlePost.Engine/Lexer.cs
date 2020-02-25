using System;
using System.Globalization;
using System.Text;
using TurtlePost.Operations;

namespace TurtlePost
{
    public partial class Interpreter
    {
        /// <summary>
        /// Continues reading text until the next specified delimiter or EOF. If no delimiter is specified, any whitespace character will mark the end.
        /// </summary>
        /// <param name="eof">Indicates whether reading was terminated by EOF.</param>
        /// <param name="c">The delimiter to stop reading at. If no value is specified, any whitespace character will be a delimiter.</param>
        /// <returns>The span of text that is returned. The delimiter that was reached is not part of the span.</returns>
        ReadOnlySpan<char> ReadToNextDelimiter(out bool eof, char? c = null)
        {
            eof = false;
            var start = Enumerator.Position;
            do
            {
                if (!Enumerator.MoveNext())
                {
                    eof = true;
                    break;
                }
            } while (c == null ? !char.IsWhiteSpace(Enumerator.Current) : c != Enumerator.Current);

            return Code.AsSpan()[start..Enumerator.Position];
        }

        void LexNumber(ref Diagnostic d)
        {
            var buffer = ReadToNextDelimiter(out _);
            
            // We have to check for 2PI here, since the main lexer loop will call this method after reading the 2.
            if (buffer.Equals("2PI", StringComparison.Ordinal))
            {
                UserStack.Push(Math.PI * 2);
                return;
            }

            if (double.TryParse(buffer, NumberStyles.Any, CultureInfo.InvariantCulture, out var num))
                UserStack.Push(num);
            else
                d = Diagnostic.Translate("TP0007", DiagnosticType.Error, buffer);
        }

        void LexList(ref Diagnostic d)
        {
            var buffer = ReadToNextDelimiter(out var eof, '}');
            if (eof)
            {
                d = Diagnostic.Translate("TP0011", DiagnosticType.Error, buffer);
            }

            var list = new List();

            // If the length is 1, the input must be '{' (the closing brace is not included); thus, we shouldn't try and parse anything. 
            // Likewise, if there is only whitespace after the first brace, we know the list is empty.
            if (buffer.Length > 1 && !buffer[1..].IsWhiteSpace())
                new Interpreter(Operations, list).Interpret(buffer[1..].ToString(), ref d,
                    new InterpretationOptions { DisallowOperations = true, HideDiagnostics = true, HideStack = true });

            UserStack.Push(list);
        }

        Operation? LexOperation(ref Diagnostic d, bool allowOperations)
        {
            var buffer = ReadToNextDelimiter(out _);
            
            // We need to check for constants in here since these can't really be special cased.

            if (buffer.Equals("true", StringComparison.Ordinal))
                UserStack.Push(true);
            else if (buffer.Equals("false", StringComparison.Ordinal))
                UserStack.Push(false);
            else if (buffer.Equals("null", StringComparison.Ordinal))
                UserStack.Push(null);
            else if (buffer.Equals("PI", StringComparison.Ordinal))
                UserStack.Push(Math.PI);
            else if (buffer.Equals("E", StringComparison.Ordinal))
                UserStack.Push(Math.E);
            else
            {
                // If operations aren't allowed, error. We can't do this earlier since constants are checked in this method as well.
                if (!allowOperations)
                {
                    d = Diagnostic.Translate("TP0012", DiagnosticType.Error, buffer);
                    return null;
                }

                if (!Operations.TryGetValue(buffer.ToString(), out var op))
                {
                    d = Diagnostic.Translate("TP0004", DiagnosticType.Error, buffer);
                }

                return op;
            }

            return null;

        }

        void LexString(ref Diagnostic d)
        {
            // This function shifts a sub-slice of the buffer to the left a specified amount of times. The sub-slice starts at the given start index and
            // continues to the end (we don't need anything more).
            // The goal is to overwrite some leftover data after we collapse the escape sequence into 1 (or 2) chars. It does this in a few steps:
            static void ShiftBufferSegment(ref Span<char> buffer, int startIndex, int amount)
            {
                // Take a slice of the buffer from the given starting index to the end and subtract our shift amount from the start index,
                // giving us the index we intend to start writing to. Then, use CopyTo to actually write the sub-slice into the original buffer.
                buffer[startIndex..].CopyTo(buffer[(startIndex - amount)..]);

                // Slice off leftover data from the end; since we shifted over 'amount' times, there will be 'amount' indices of "garbage data" left
                // over on the end of the buffer that we need to remove.
                buffer = buffer[..^amount];
            }

            Enumerator.MoveNext(); // Skip " character
            var sourceString = ReadToNextDelimiter(out _, '"');

            // Before we push the input string to the stack, we need to parse escape sequences. Since the buffer we get from the source is read-only
            // (strings are immutable), we will need to make our own temporary mutable buffer. It needs to be as long as the source string, but when we collapse
            // the escape sequences, the string becomes shorter, so we will have to trim off the leftover space as we go.

            // Allocate our buffer. If the string is too big for the stack, allocate a new array.
            var buffer = sourceString.Length > 512 ? new char[sourceString.Length] : stackalloc char[sourceString.Length];

            // Now we can copy from the source input to our mutable buffer
            sourceString.CopyTo(buffer);

            // Loop through our buffer to find any escape sequences
            for (var i = 0; i < buffer.Length; i++)
            {
                if (buffer[i] != '\\')
                    continue;

                // An escape sequence has started; look ahead to see what is actually being escaped
                switch (buffer[i + 1])
                {
                    case '"': buffer[i] = '"'; break;
                    case '0': buffer[i] = '\0'; break;
                    case 'a': buffer[i] = '\a'; break;
                    case 'b': buffer[i] = '\b'; break;
                    case 'f': buffer[i] = '\f'; break;
                    case 'n': buffer[i] = '\n'; break;
                    case 'r': buffer[i] = '\r'; break;
                    case 't': buffer[i] = '\t'; break;
                    case 'v': buffer[i] = '\v'; break;
                    case '\'': buffer[i] = '\''; break;
                    case '\\': buffer[i] = '\\'; break;
                    case 'u':
                        // 'u' denotes an escape of the form \uXXXX, where XXXX is a UTF-16 code unit in hex. We need to capture those 4 characters that make up
                        // the hex digit, parse it, and reinterpret it as  a character. 
                        if (!ushort.TryParse(buffer.Slice(i + 2, 4), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var utf16CodeUnit))
                        {
                            d = Diagnostic.Translate("TP0008", DiagnosticType.Error, sourceString, i + 2);
                            return;

                        }

                        buffer[i] = (char) utf16CodeUnit;

                        // Shift the buffer left 5 to cover up the 'u' and the hex digit. Since we overwrote the backslash with the code unit, we shouldn't
                        // discard it.
                        ShiftBufferSegment(ref buffer, i + 6, 5);
                        continue;
                    case 'U':
                        // 'U' is similar in form to 'u', but parses a UTF-32 code unit instead (with an 8 digit long hex string to match). Because a UTF-32
                        // code unit may or may not correspond to a UTF-16 surrogate pair, we will have to shift either 8 or 9 characters over, depending on how
                        // many UTF-16 code units we end up writing.
                        if (!uint.TryParse(buffer.Slice(i + 2, 8), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var utf32CodeUnit))
                        {
                            d = Diagnostic.Translate("TP0008", DiagnosticType.Error, sourceString, i + 2);
                            return;
                        }

                        // We will use Rune to convert a UTF-32 code unit to UTF-16 code units and write it to the buffer, starting from the current index
                        // (which is the backslash). We need to store the amount of characters written so that we trim off the correct number of garbage
                        // characters; if we write more than 1 character, we need to account for that (or else it gets discarded and the wrong character
                        // sequence is written).
                        var charsWritten = new Rune(utf32CodeUnit).EncodeToUtf16(buffer[i..]);

                        // Since charsWritten is always 1 or 2, we can subtract it from 10 to see how many garbage characters we have to trim.
                        ShiftBufferSegment(ref buffer, i + 10, 10 - charsWritten);
                        continue;
                    default:
                        d = Diagnostic.Translate("TP0009", DiagnosticType.Error, sourceString);
                        return;
                }

                // We have already performed the character escape; however, the character we inserted only replaces the first character of the escape sequence:
                // "H e l l \ n o" -> "H e l l \n n o"
                // Since we're only changing one character, and escape sequences are multiple characters long, we need to clear away the rest of the characters
                // in the escape sequence. We will do that by shifting the remaining contents of the buffer *over* the leftover characters from the escape
                // sequence. Thus, the other characters in the escape sequence get overwritten and the buffer is trimmed to correctly fit the string.
                // See ShiftBufferSegment for info on how it works.

                // The shifted sub-buffer should start 2 indices ahead of the current one to skip the (now replaced) backslash and the character representing
                // the escape. We are only going to shift one space to the left, since we only want to overwrite the character representing the escape, not the
                // backslash (which got replaced with the escaped character).
                ShiftBufferSegment(ref buffer, i + 2, 1);
            }

            // Finally push our escaped string to the stack
            UserStack.Push(buffer.ToString());
        }

        void SkipComment()
        {
            Enumerator.MoveNext(); // Skip / character
            ReadToNextDelimiter(out _, '/');
        }

        void LexGlobal()
        {
            Enumerator.MoveNext(); // Skip & character
            var buffer = ReadToNextDelimiter(out _);
            UserStack.Push(Globals[buffer.ToString()]);
        }

        void LexLabel(ref Diagnostic d)
        {
            var buffer = ReadToNextDelimiter(out _);
            if (buffer[^1] == ':')
            {
                // Label declaration; do not do anything
                return;
            }

            if (!Labels.TryGetValue(buffer[1..].ToString(), out var label))
            {
                d = Diagnostic.Translate("TP0005", DiagnosticType.Error, buffer);
                return;
            }

            UserStack.Push(label);
        }
    }
}