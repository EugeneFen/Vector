using System;
using System.Collections.Generic;
using System.Text;
using System.IO;


namespace Chomsky_normal_form
{
    class Grammar
    {
        private List<string> noterminals = new List<string>(); //нетерминалы
        private List<string> terminals = new List<string>(); //терминалы
        private List<List<string>> rules = new List<List<string>>(); //правила
        private String initial;

        private List<string> noterminals_V = new List<string>(); //нетерминалы вывода 
        private List<string> terminals_V = new List<string>(); //терминалы вывода
        private List<List<string>> rules_V = new List<List<string>>(); //правила вывода

		public Grammar()
		{
			ReadFile();
		}
		private void ReadFile()
		{
			string curfile = @"Grammar.txt";

			if (File.Exists(curfile)) //проверяем открыт ли файл
			{
				StreamReader file = new StreamReader("Grammar.txt"); // говорим, что переменная file теперь ссылается на наш файл

				string value; //терминал или нетерминал
				value = ""; //нужно, чтобы коректно работал код			
				int j = 0; //для поиска *
				string finish = "^"; // стопсивол
				string litter = "*"; //разделитель
				bool theend = true; //чтобы цикл while останавливался
				List<string> expressions; //каждое новое правило сначала заноситься в промежуточный лист

				initial = file.ReadLine(); //первая строка начальный символ
				string buff = file.ReadLine(); //вторая строка файла нетерминалы

				while (theend) //пока не нашли ^ и не дошли до конца
				{

					while (buff[j] != litter[0] && buff[j] != finish[0]) j++; //отмеряем размер строки до *
					if (buff[j] == litter[0])
					{
						value = buff.Substring(0, j); //копируем строку до * в отдельную переменную
						noterminals.Add(value); //добавляет строку в лист
						buff = buff.Remove(0, j + 1); //удаляем строку вместе с *
						j = 0; //начинаем с начала строки
						theend = true;

					}
					else if (buff[j] == finish[0]) //нашли стопсимвол и выходим из цикла
					{
						theend = false;
					}
					else
					{
						Console.WriteLine("Something went wrong while reading the first line");
						theend = false; // в любой непонятной ситуайии выходим из цикла
					}


				}
				int size_buff = buff.Length; //осталась последняя строка
				value = buff.Substring(0, size_buff - 1); //копируем последнюю строку
				noterminals.Add(value); //добавляем строку в лист

				buff = file.ReadLine(); //читаем третью строку файла. терминалы
				theend = true; //запускаем цикл

				while (theend) //пока не нашли ^ и не дошли до конца
				{

					while (buff[j] != litter[0] && buff[j] != finish[0]) j++; //отмеряем размер строки до *
					if (buff[j] == litter[0])
					{
						value = buff.Substring(0, j); //копируем строку до * в отдельную переменную
						terminals.Add(value); //добавляет строку в лист
						buff = buff.Remove(0, j + 1); //удаляем строку вместе с *
						j = 0; //начинаем с начала строки
						theend = true;

					}
					else if (buff[j] == finish[0]) //нашли стопсимвол и выходим из цикла
					{
						theend = false;
					}
					else
					{
						Console.WriteLine("Something went wrong while reading the second line");
						theend = false; // в любой непонятной ситуайии выходим из цикла
					}
				}
				size_buff = buff.Length; //осталась последняя строка
				value = buff.Substring(0, size_buff - 1); //копируем последнюю строку
				terminals.Add(value); //добавляем строку в лист

				while (!file.EndOfStream) //пока не конец файла
				{
					buff = file.ReadLine(); //читаем строку. правиа
					theend = true; //запускаем цикл
					expressions = new List<string>(); //создаем новую строку таблицы листов


					while (theend) //пока не нашли ^
					{
						while (buff[j] != litter[0] && buff[j] != finish[0]) j++; //отмеряем размер строки до *
						if (buff[j] == litter[0]) //нашли разделяющий символ
						{
							value = buff.Substring(0, j); //копируем строку до * в отдельную переменную
							expressions.Add(value); //добавляет строку в лист
							buff = buff.Remove(0, j + 1); //удаляем строку вместе с *							
							theend = true;

						}
						else if (buff[j] == finish[0]) //нашли стопсимвол и выходим из цикла
						{
							theend = false;
						}
						else
						{
							Console.WriteLine("something went wrong while reading the rules");
							theend = false; // в любой непонятной ситуайии выходим из цикла
						}
						j = 0; //начинаем с начала строки
					}
					size_buff = buff.Length; //осталась последняя строка
					value = buff.Substring(0, size_buff - 1); //копируем последнюю строку
					expressions.Add(value); //добавляем строку в лист
					rules.Add(expressions);
				}

				file.Close(); //закрываем файл
			}
		}
		private bool Check() //проверяет есть ли првило S->&
        {
			bool lambda_rule = false; //есть лямда правило из S
			bool right_axiom = false; //есть S в правой части
			for (int i=0; i< rules.Count; i++)
				for(int j=0; j<rules[i].Count; j++) //проходимся по всем правилам
                {
					if ((rules[i][0][0] == 'S') && (rules[i][j][0] == '&')) lambda_rule = true; //ищем лямда правило
					if (j > 0) for (int k = 0; k < rules[i][j].Length; k++) if (rules[i][j][k] == 'S') right_axiom = true; //ищем S в правой части
				}
			if ((lambda_rule && !right_axiom) || (!lambda_rule)) return true; //если есть лямда правило и нет s в правой части то гуд или лямда правила вообще нет
			else return false;
        }
		private bool Check_term_and_noterm(char x, List<string> term_noterm) //проверяет принадлежит ли символ списку
        {
			bool cheking = false;
			int i = 0;
			while(!cheking && i< term_noterm.Count)
            {
				if (x == term_noterm[i][0]) cheking = true;
				i++;
            }
			return cheking;
        }
		private bool Check_term_and_noterm_V(string x) //проверяет принадлежит ли нетерм списку. нетерм вида a*
		{
			bool cheking = false;
			int i = 0;
			while (!cheking && i < noterminals_V.Count)
			{
				if (noterminals_V[i].Length == 2 && x[0] == noterminals_V[i][0] && x[1] == noterminals_V[i][1]) cheking = true;
				i++;
			}
			return cheking;
		}
		private void Add_two_liter(string first, string canon)
        {
			List<string> expressions;
			string conclusion; //правило с новым нетермом. нужно для добавление в новые правила
			string noterm; //новый нетерм. нужно для добавления нового нетерма
			//Console.WriteLine(canon[0] + " --- " + canon[1]);

			if (Check_term_and_noterm(canon[0], noterminals) && Check_term_and_noterm(canon[1], noterminals)) // правило у которого с права два нетерма
			{
				//Console.WriteLine("no" + "   " + "no");
				expressions = new List<string>();
				expressions.Add(first);
				expressions.Add(canon);
				rules_V.Add(expressions); //провто добавляем его в правила без изменений
			}
			else if (Check_term_and_noterm(canon[0], terminals) && Check_term_and_noterm(canon[1], noterminals)) //если вправа первый символ терминал, а второй нетерм
			{
				//Console.WriteLine("te" + "   " + "no");
				noterm = canon[0] + "*"; //новый нетерм
				conclusion = noterm + canon[1]; //у нас левый символ терминал, значит создаем новый нетерм
				expressions = new List<string>();
				expressions.Add(first);
				expressions.Add(conclusion);
				rules_V.Add(expressions); //добавляем новое правило	
				if (!Check_term_and_noterm_V(noterm)) //проверяем есть ли такой нетерм в списке
                {
					noterminals_V.Add(noterm); //добавляем новый нетерм
					expressions = new List<string>(); //создаем новое правило для нового нетерма
					expressions.Add(noterm);
					expressions.Add(String.Concat(canon[0]));
					rules_V.Add(expressions); //добавляем новое правило
                }
					
			}
			else if (Check_term_and_noterm(canon[0], noterminals) && Check_term_and_noterm(canon[1], terminals)) //если у нас справа первый символ нетерм, а второй терм
			{
				//Console.WriteLine("no" + "   " + "te");
				noterm = canon[1] + "*"; //создаем новый нетерм
				conclusion = canon[0] + noterm; //у нас левый символ терминал, значит создаем новый нетерм
				expressions = new List<string>();
				expressions.Add(first);
				expressions.Add(conclusion);
				rules_V.Add(expressions); //добавляем новое правило
				if (!Check_term_and_noterm_V(noterm)) //если такого нетерма еще нет, то дабавляем
                {
					noterminals_V.Add(noterm); //добавляем новый нетерм
					expressions = new List<string>(); //создаем новое правило для нового нетерма
					expressions.Add(noterm);
					expressions.Add(String.Concat(canon[1]));
					rules_V.Add(expressions); //добавляем новое правило
                }					
			}
            else
            {
				string noterm_2 = canon[0] + "*";
				noterm = canon[1] + "*"; //создаем новый нетерм
				conclusion = noterm_2 + noterm; //у нас левый символ терминал, значит создаем новый нетерм
				expressions = new List<string>();
				expressions.Add(first);
				expressions.Add(conclusion);
				rules_V.Add(expressions); //добавляем новое правило
				if (!Check_term_and_noterm_V(noterm)) //если такого нетерма еще нет, то дабавляем
				{
					noterminals_V.Add(noterm); //добавляем новый нетерм
					expressions = new List<string>(); //создаем новое правило для нового нетерма
					expressions.Add(noterm);
					expressions.Add(String.Concat(canon[1]));
					rules_V.Add(expressions); //добавляем новое правило
				}
				if (!Check_term_and_noterm_V(noterm_2)) //если такого нетерма еще нет, то дабавляем
				{
					noterminals_V.Add(noterm_2); //добавляем новый нетерм
					expressions = new List<string>(); //создаем новое правило для нового нетерма
					expressions.Add(noterm_2);
					expressions.Add(String.Concat(canon[0]));
					rules_V.Add(expressions); //добавляем новое правило
				}
			}
		}
		private void Chomsky() //сам алгоритм Хомского
		{			
			List<string> expressions;
			string conclusion; //правило с новым нетермом. нужно для добавление в новые правила
			string noterm; //новый нетерм. нужно для добавления нового нетерма

			for (int i=0; i<noterminals.Count; i++) noterminals_V.Add(noterminals[i]); //еопирую все нетерминалы в новый список нерминалов
			for (int i = 0; i < rules.Count; i++)
				for (int j = 0; j < rules[i].Count; j++) //проходимся по всем правилам
				{
					if (rules[i][j].Length == 1 && j>0) // правило у которого с права терминал
						if(Check_term_and_noterm(rules[i][j][0], terminals)) //если справа терминал то добавляем его в новые правила
                        {
							expressions = new List<string>();
							expressions.Add(rules[i][0]);
							expressions.Add(rules[i][j]);
							rules_V.Add(expressions);
                        }
					if (rules[i][j].Length == 2 && j > 0) Add_two_liter(rules[i][0], rules[i][j]);
					if (rules[i][0][0] == 'S' && rules[i][j][0] == '&') //если есть правило S->&, то его добавляем в правила
                    {
						expressions = new List<string>();
						expressions.Add(rules[i][0]);
						expressions.Add(rules[i][j]);
						rules_V.Add(expressions);
					}
					if (rules[i][j].Length > 2 && j > 0) //если вывод больше чем из 2 символов
                    {
						string rules_buff = rules[i][j]; //строка вывода
						string rule_start = rules[i][0]; //из чего выводиться
						while (rules_buff.Length != 2) //в случае если символов больше двух, то циклически уменишаем их численность
                        {
							string noterm_term = rules_buff.Substring(0, 1); //первый символ строки
							rules_buff = rules_buff.Remove(0, 1); //вывод без первого символа
							if (Check_term_and_noterm(noterm_term[0], terminals)) //если первым символом вывода был терминал
							{
								noterm = noterm_term + "*"; //создаем новый нетерминал
								conclusion = noterm + "<" + rules_buff + ">"; //создаем новый вывод
								expressions = new List<string>();
								expressions.Add(rule_start); //добавляем из чего выводиться вывод
								expressions.Add(conclusion); //добавляем вывод
								rules_V.Add(expressions); //добавляем новое правило									
								noterminals_V.Add("<" + rules_buff + ">"); //добавляем также новый нетерминал
								if (!Check_term_and_noterm_V(noterm)) //если такого нетерминала еще небыло добавленн
                                {
									noterminals_V.Add(noterm); //добавляем новый нетерм
									expressions = new List<string>(); //создаем новое правило для нового нетерма
									expressions.Add(noterm);
									expressions.Add(noterm_term);
									rules_V.Add(expressions); //добавляем новое правило
                                }								
								rule_start = "<" + rules_buff + ">"; //теперь строка из которой выводиться правило новая
							}
							else
							{
								conclusion = noterm_term + "<" + rules_buff + ">"; //создаем новый вывод
								expressions = new List<string>();
								expressions.Add(rule_start); //добавляем из чего выводиться вывод
								expressions.Add(conclusion); //добавляем вывод
								rules_V.Add(expressions); //добавляем новое правило
								noterminals_V.Add("<" + rules_buff + ">"); //добавляем также новый нетерминал
								rule_start = "<" + rules_buff + ">"; //теперь строка из которой выводиться правило новая
							}
                        }
						if (rules_buff.Length == 2) Add_two_liter(rule_start, rules_buff); //если осталось только 2 сивола
					}
				}
		}
		public void Start_metod()
        {
			if (Check()) Chomsky();
			else Console.Write("Error!"); //если в грамматике есть S->& и при этом справа в каком-то правиле есть S
		}
		public void Write_Grammar()
		{
			for (int i = 0; i < noterminals.Count; i++)
			{
				if (i > 0) Console.Write(", ");
				Console.Write(noterminals[i]);
			}
			Console.WriteLine();

			for (int i = 0; i < terminals.Count; i++)
			{
				if (i > 0) Console.Write(", ");
				Console.Write(terminals[i]);
			}
			Console.WriteLine();
			for (int i = 0; i < rules.Count; i++) // Чуть сложного кода для карсивого вывода)
			{
				Console.Write("    " + rules[i][0] + " -> ");
				for (int j = 1; j < rules[i].Count; j++) // Проверить на ошибки при выводе 
				{
					if (j > 1) Console.Write(", ");
					Console.Write(rules[i][j]);
				}
				Console.WriteLine(" ");
			}
			Console.WriteLine(initial);
			Console.WriteLine("---------------");

			for (int i = 0; i < noterminals_V.Count; i++)
			{
				if (i > 0) Console.Write(", ");
				Console.Write(noterminals_V[i]);
			}
			Console.WriteLine("  ;");
			for (int i = 0; i < terminals.Count; i++)
			{
				if (i > 0) Console.Write(", ");
				Console.Write(terminals[i]);
			}
			Console.WriteLine("  ;");

			for (int i = 0; i < rules_V.Count; i++) // Чуть сложного кода для карсивого вывода)
			{
				Console.Write("    " + rules_V[i][0] + " -> ");
				for (int j = 1; j < rules_V[i].Count; j++) // Проверить на ошибки при выводе 
				{
					if (j > 1) Console.Write(", ");
					Console.Write(rules_V[i][j]);
				}
				Console.WriteLine(" ");
			}
		}
	}
}
