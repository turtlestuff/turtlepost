using System;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace TurtlePost
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("TurtlePost version 999999999.999999999999999 BEETA");
            Console.WriteLine("By Vrabbers and Reflectronic, 2019");
            Console.WriteLine("Type exit to exit");
            var directInterpreter = new Interpreter();

            while (true)
            {
                Console.Write(">");
                var toInterpret = Console.ReadLine();

                directInterpreter.Interpret(toInterpret);
            }
        }
    }
}