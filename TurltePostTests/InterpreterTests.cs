using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using TurtlePost;
namespace TurltePostTests
{
    [TestClass]
    public class InterpreterTests
    {
        [TestMethod]
        public void TestBaseFunctionality()
        {
            Interpreter test = new Interpreter();
            test.Interpret("\"astring\" @drop call 3 2 mod dup 1 sub swap drop @end jump @drop: drop ret", true);
            Stack<object?> stack = new Stack<object?>();
            double x = (double)test.UserStack.Pop()!;
            Assert.IsTrue(x == 0 && test.UserStack.Count == 0);
        }

        [TestMethod]
        public void TestBadOperation()
        {
            Interpreter test = new Interpreter();
            test.Interpret("2 2 2 aaaaaaaaaaaaaaaaaaaaaaaaaa",false);
            Assert.IsTrue(test.UserStack.Count == 0);
        }
    }
}
