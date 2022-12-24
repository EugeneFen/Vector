using System;

namespace MiniStringFuck
{
    class Program
    {
        static bool getString(string code)
        {
            bool check = true;
            for (int i = 0; i < code.Length; i++) //просматриваем всю строку
            {
                switch (code[i])
                {
                    case '+':
                        check = true;
                        break;
                    case '.':
                        check = true;
                        break;
                    default:
                        check = false;
                        break;
                }
                if (!check) return check;
            }
            return check;
        }
        static void Main(string[] args)
        {
            //string code = Console.ReadLine();
            //Interpreter b;
            // if (getString(code))
            //{
            // b = new Interpreter(code);
            // Console.WriteLine(b.Run());
            //}
            // else Console.WriteLine("Error: string contain invalid characters");

            AvtoTest a = new AvtoTest();
            Console.WriteLine("Test 1: " + a.Test_1());
            Console.WriteLine();
            Console.WriteLine("Test 2: " + a.Test_2());
        }
    }
}
