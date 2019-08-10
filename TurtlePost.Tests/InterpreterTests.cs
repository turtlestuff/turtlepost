using Xunit;

namespace TurtlePost.Tests
{
    public class InterpreterTests
    {
        [Fact]
        public void TestBaseFunctionality()
        {
            var test = new Interpreter();
            test.Interpret("\"astring\" @drop call 3 2 mod dup 1 sub swap drop @end jump @drop: drop ret", true);
            var x = (double) test.UserStack.Pop()!;
            Assert.True(x == 0 && test.UserStack.Count == 0);
        }

        [Fact]
        public void TestBadOperation()
        {
            var test = new Interpreter();
            test.Interpret("2 2 2 aaaaaaaaaaaaaaaaaaaaaaaaaa",false);
            Assert.True(test.UserStack.Count == 0);
        }
    }
}