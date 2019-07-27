using System;

namespace TurtlePost
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("TurtlePost version 999999999.999999999999999 BEETA");
            Console.WriteLine("By Vrabbers, 2019");
            Console.WriteLine("Type exit to exit");
            Interpreter directInterpreter = new Interpreter();
            while (true)
            {
                Console.Write(">");
                string toInterpret = Console.ReadLine();

                if(toInterpret == "exit")
                {
                    return;
                }

                directInterpreter.Interpret(toInterpret);
            }
        }
    }
}
