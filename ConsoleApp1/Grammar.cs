using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

//поставить в левой части терминал

namespace ConsoleApp1
{
	class Grammar
	{
		private List<string> noterminals = new List<string>(); //нетерминалы
		private List<string> terminals = new List<string>(); //терминалы
		private List<List<string>> rules = new List<List<string>>(); //правила
		private String initial;
		private List<string> goodSimvols = new List<string>(); //хорошие нетерминалы (алг 1)

		private bool termWord = false;          // Если false - то язык пуст		

		public List<string> V = new List<string>();//множество достижимых символов (алг 2)
		List<string> V_start = new List<string>();
		bool begin_ = true;//

		public List<string> noterminals_V = new List<string>(); //нетерминалы вывода 
		public List<string> terminals_V = new List<string>(); //терминалы вывода
		public List<List<string>> rules_V = new List<List<string>>(); //правила вывода

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
		// Находит все терминалы в правилах (только односимвольные), и отправляет их в Finder	
		/*
		 * идем по всем правилам и ищем терминал вправой части. сл аходим в стрке равила терминал, то нетерм, из которог следует правило,
		 * попадает в список хороших символов. 
		 * после идем в Finder с тим нетермом.
		 */
		private void CheckIsHaveTermOnrules()
		{
			for (int i = 0; i < rules.Count; i++)
			{
				for (int j = 1; j < rules[i].Count; j++) // Смотрим по всем правилам
				{
					for (int k = 0; k < terminals.Count; k++) // По всем терминалам
					{
						bool q = true;
                        for( int t = 0; t < rules[i][j].Length; t++ )
                        {
							if (rules[i][j][t] == terminals[k][0] && q)
							{
								q = false;
								// Нашли, что в каком-то правиле есть терминал										
								bool add_good = true;
								for (int e = 0; e < goodSimvols.Count; e++) if (goodSimvols[e] == rules[i][0]) add_good = false;
								if (add_good)
								{
									this.goodSimvols.Add(rules[i][0]);//хорошие не терминалы								 
									Finder(rules[i][0]); // Отправляем его в Finder
								}
							}
                        }
                       						
					}
				}
			}
		}
		// Получает на вход нетерм, и ищет существует ли правило, из которого эту лексему можно получить
		// Если такое правило есть, то рекурсивно отправляет в самого себя это правило, в качестве входного значения
		// Если это найденное правило является аксиомой - то язык не пуст
		private void Finder(string findCell)
		{
			//Console.WriteLine("Ищем элемент " + findCell + " во всех правилах");
			for (int i = 0; i < rules.Count; i++)
			{
				for (int j = 1; j < rules[i].Count; j++)
				{
					bool goo = true;
					for (int e = 0; e < goodSimvols.Count; e++) if (rules[i][0][0] == goodSimvols[e][0]) goo = false;
					if(goo)
					{
						bool add_good = false;
						for (int t = 0; t < rules[i][j].Length; t++)
						{
							if (rules[i][0][0] == rules[i][j][t]) break;
							for (int k = 0; k < goodSimvols.Count; k++)
							{
								if (goodSimvols[k][0] == rules[i][j][t]) add_good = true;
								else
								{
									for (int r = 0; r < terminals.Count; r++) // По всем терминалам
									{
										if (rules[i][j][t] == terminals[r][0]) add_good = true;
										else add_good = false;
									}
								}
							}
						}
						if (add_good)
						{
							this.goodSimvols.Add(rules[i][0]);
							Finder(rules[i][0]);
						}
					}
						
				}
			}
		}		
		// Ищет во множестве хороших смиволов начальный символ
		private void Search_for_original_symbol()
		{
			for (int i = 0; i < goodSimvols.Count; i++)
			{
				Console.Write(goodSimvols[i] + "  +  ");
				if (goodSimvols[i] == initial)
				{
					//Console.WriteLine("В множестве хороших смиволов нашли аксиому. Значит язык не пуст");
					termWord = true;
					break;
				}
			}
		}
		private void printCanBelanguage()
		{			
			Console.WriteLine(" ");
			Search_for_original_symbol();
			Console.WriteLine(" ");
			Console.WriteLine(goodSimvols.Count);
			if (termWord == true) Console.WriteLine("Language is not empty");
			else Console.WriteLine("Language is empty");
			Console.WriteLine(" ");
		}
		private List<string> Together(List<string> first, List<string> second)//пересечение двух объектов 
		{
			List<string> itog = new List<string>();
			for (int i = 0; i < first.Count; i++)
			{
				for (int j = 0; j < second.Count; j++)
				{
					if (first[i] == second[j])
					{
						string res = first[i];
						itog.Add(res);
					}
				}
			}
			return itog;
		}
		private void End_alg8_2()
		{
			noterminals_V = Together(noterminals, V);
			terminals_V = Together(terminals, V);

			for (int i = 0; i < rules.Count; i++)//проходимся по всем правилам
			{
				bool prov = true;
				for (int j = 0; j < rules[i].Count; j++)
				{
					string rule = rules[i][j];//с правила 
					for (int t = 0; t < rule.Length; t++)
					{
						string w = Convert.ToString(rule[t]);
						if (!V.Exists(x => x == w))//проверяет, существует ли символ в множестве достижимых символов 
						{
							prov = false;
						}
					}
				}
				if (prov == true && !rules_V.Exists(x => x == rules[i]))
				{
					rules_V.Add(rules[i]);//итоговый вывод правил
				}
			}
		}
		private void Selection()
		{
			if (begin_ == true)//если только начало алгоритма, то 
				V.Add(initial);//v_0={s}
							   //если не начало, то пропускается этот шаг
			int i;
			V_start = V;//это необходимо для сравнения V_i-1 = V_i, если равны, то алгоритм продолжается, а если нет - то рекурсия 
			for (i = 0; i < rules.Count; i++)
			{
				for (int k = 0; k < V.Count; k++)
				{
					if (rules[i][0] == V[k])//аксиома правила принадлежит ли множеству достижимых символов?
					{
						for (int j = 0; j < rules[i].Count; j++)//добавляет множество символов, которые принадлежат правилу и аксиома которых уже добавлена в множестве достижимых символов
						{
							string rule = rules[i][j];//с правила 
							for (int t = 0; t < rule.Length; t++)
							{
								string w = Convert.ToString(rule[t]);
								if (!V.Exists(x => x == w))//проверяет, существует ли символ в множестве достижимых символов 
								{
									V.Add(w);//если не существует, то добавляет его в  V(множ-во достиж-ых сим-лов)
								}

							}

						}
						if (V_start != V)
						{
							begin_ = false;
							Selection();//рекурсия 
						}
						else
						{
							End_alg8_2();//создает то что будет на выводе
						}
					}
				}
			}
		}
		public Grammar()
		{
			ReadFile();
		}
		public void WriteGrammar()
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

			Console.WriteLine(this.goodSimvols.Count);
			for (int i = 0; i < this.goodSimvols.Count; i++)
			{
				if (i > 0) Console.Write(", ");
				Console.Write(this.goodSimvols[i]);
			}
			Console.WriteLine();

			for (int i = 0; i < noterminals_V.Count; i++)
			{
				if (i > 0) Console.Write(", ");
				Console.Write(noterminals_V[i]);
			}
			Console.WriteLine("  ;");
			for (int i = 0; i < terminals_V.Count; i++)
			{
				if (i > 0) Console.Write(", ");
				Console.Write(terminals_V[i]);
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
		public void Algorithm_1()
        {
			goodSimvols.Clear();
			CheckIsHaveTermOnrules();
			printCanBelanguage();
		}
		public void Algorithm_2()
        {
			noterminals_V.Clear();
			terminals_V.Clear();
			rules_V.Clear();
			Selection();
		}
		public void Algorithm_3()
        {
			Algorithm_1();
			WriteGrammar();
			List<List<string>> regulations = new List<List<string>>();
			List<string> expressions; //каждое новое правило сначала заноситься в промежуточный лист
			for (int i = 0; i < rules.Count; i++) //столбец правила
            {
				bool litter = false; //нашли терм или нетерм в правиле
				bool found = true;
				for (int j=0; j < rules[i].Count; j++) //строка правила
                {
					int size_rules = rules[i][j].Length; //размер строки правила
					String buff = rules[i][j]; //ячейка правила			
					int t = 0;
					while((t < size_rules) && (found)) //ходим по строке правила
                    {
						int k = 0;
						while((k<goodSimvols.Count) && (!litter)) //смотрим есть ли нетерм
                        {
							String buff_good = goodSimvols[k];
							if (buff[t] == buff_good[0]) litter = true;
							k++;	
                        }
						k = 0;
						while((k<terminals.Count) && (!litter)) //смотрим есть ли терм
						{
							String buff_term = terminals[k];
							if (buff[t] == buff_term[0]) litter = true;
							k++;
						}
						if (litter) found = true;
						else found = false;
						litter = false;
						t++;
                    }                    
				}
				if (found)
                {
					expressions = new List<string>(); //создаем новую строку таблицы листов
					for (int j = 0; j < rules[i].Count; j++) //строка правила
                    {
						expressions.Add(rules[i][j]);
                    }
					regulations.Add(expressions);
				}
            }
			Console.WriteLine();
			Console.WriteLine("-------------------");
			for (int i = 0; i < regulations.Count; i++) // Чуть сложного кода для карсивого вывода)
			{
				Console.Write("    " + regulations[i][0] + " -> ");
				for (int j = 1; j < regulations[i].Count; j++) // Проверить на ошибки при выводе 
				{
					if (j > 1) Console.Write(", ");
					Console.Write(regulations[i][j]);
				}
				Console.WriteLine(" ");
			}
			Algorithm_2();
		}
	}
}
