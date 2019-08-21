using Xunit;

namespace TurtlePost.Tests
{
    public class InterpreterTests
    {
        [Fact]
        public void TestBaseFunctionality()
        {
            var test = new Interpreter();
            Diagnostic d = default;
            test.Interpret(@"""astring"" @drop call 3 2 mod dup 1 sub swap drop @end jump @drop: drop ret", ref d);
        }

        [Fact]
        public void TestBadOperation()
        {
            var test = new Interpreter();
            Diagnostic d = default;
            test.Interpret("2 2 2 aaaaaaaaaaaaaaaaaaaaaaaaaa",ref d);
            Assert.Equal(DiagnosticType.Error, d.Type);
        }
    }
}