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
            string code = Console.ReadLine();
            Interpreter a;
            if (getString(code))
            {
                a = new Interpreter(code);
                Console.WriteLine(a.Run());
            }
            else Console.WriteLine("Error: string contain invalid characters");
        }
    }
}
