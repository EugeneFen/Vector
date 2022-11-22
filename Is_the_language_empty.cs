using System;
using System.Collections.Generic;

namespace TIP_01
{
    public class Program
    {
        static MainCore mainCore = new MainCore();

        static void Main(string[] args)
        {
           //ввод данных
           /////////////
            mainCore.checkIsHaveTermOnRules();

            mainCore.printCanBelanguage(); // Выводит, пуст ли язык
        }
    }

    public class MainCore
    {
        // Обявляю NEPS
        public List<string> NoTerminals = new List<string>(); //нетерминалы
        public List<string> Terminals = new List<string>(); //терминалы
        List<List<string>> Rules = new List<List<string>>(); //правила
        public string Axioma; //начальный символ

        public List<string> GoodSimvols = new List<string>(); //хорошие нетерминалы

        bool termWord = false;          // Если false - то язык пуст
        bool isProgrammEnd = false;     // Если true - то все рекурсивные вызовы завершаются
        bool isValPrint = false;        // Если true - то ответ больше не печатается

        /*
            Например правило A -> BC, Bc, c;
            Будет выглядеть: rules[n] = [["A", "Bc", "Bc", "c"]], где n - это номер этого правила

            Лямбда - будет & 
           */

        // Находит все терминалы в правилах (только односимвольные), и отправляет их в Finder
        public void checkIsHaveTermOnRules() 
        {
            for (int i = 0; i < Rules.Count; i++)
            {
                for (int j = 0; j < Rules[i].Count; j++) // Смотрим по всем правилам
                {
                    for (int k = 0; k < Terminals.Count; k++) // По всем терминалам
                    {
                        if (Rules[i][j] == Terminals[k]) // Сравниваем каждый элемент
                        {
                            // Нашли, что в каком-то правиле есть терминал

                            string ruleCondition = Rules[i][0];

                            GoodSimvols.Add(ruleCondition); //хорошие терминалы

                            Console.WriteLine("При начальном обходе нашли терминал в правилах, это: " + Rules[i][j]);
                            Finder(ruleCondition); // Отправляем его в Finder
                        }
                    }
                }
            }
        }

        // Получает на вход лексему, и ищет существует ли правило, из которого эту лексему можно получить
        // Если такое правило есть, то рекурсивно отправляет в самого себя это правило, в качестве входного значения
        // Если это найденное правило является аксиомой - то язык не пуст
        void Finder(string findCell) 
        {
            if (isProgrammEnd == false)
            {
                Console.WriteLine("Ищем элемент " + findCell + " во всех правилах");
                for (int i = 0; i < Rules.Count; i++)
                {
                    for (int j = 1; j < Rules[i].Count; j++)
                    {
                        if (Rules[i][j] == findCell)
                        {
                            Console.Write("Нашли. ");
                            Console.WriteLine("Начало правила: " + Rules[i][0] + ". Теперь ищем правило, из которого этот элемент получался бы");
                            GoodSimvols.Add(Rules[i][0]);
                            Finder(Rules[i][0]);
                        }
                    }
                }
            }
        }

        // Ищет во множестве хороших смиволов Аксимоу
        void isLangEmpty()
        {
            for (int i = 0; i < GoodSimvols.Count; i++)
            {
                if (GoodSimvols[i] == Axioma)
                {
                    Console.WriteLine("В множестве хороших смиволов нашли аксиому. Значит язык не пуст");
                    termWord = true;
                    break;
                }
            }
        }

        // Печатает, пуст ли язык
        public void printCanBelanguage()
        {
            if (isValPrint == false)
            {
                Console.WriteLine(" ");
                isLangEmpty();
                Console.WriteLine(" ");
                isValPrint = true;
                if (termWord == true) Console.WriteLine("Язык не пуст");
                else Console.WriteLine("Язык пуст");
            }
        }
    }
}
