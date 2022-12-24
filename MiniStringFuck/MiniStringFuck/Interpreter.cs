using System;
using System.Collections.Generic;
using System.Text;

namespace MiniStringFuck
{
    //лишь одна ячейка памяти
    //счетчик может только увеличиваться
    //если счетчик достигнет 256, то он обнуляется
    class Interpreter
    {
        private int pointer; //Ячейка памяти
        private char[] code; //входная строка
        private string output;

        public Interpreter(string code)
        {
            this.code = code.ToCharArray(); //преобразовываем строку в массив символов
            output = "";
        }

        public string Run()
        {
            for (int i = 0; i < code.Length; i++) //просматриваем всю строку
            {
                switch (code[i])
                {
                    case '+': //увеличивает счетчик
                        pointer++;
                        break;
                    case '.': //выводит символ
                        output = output + Convert.ToChar(pointer);
                        break;                        
                }
                if (pointer == 256) pointer = 0;//если счетчик достигнет 256, то он обнуляется
            }
            return output;
        }
    }
}
