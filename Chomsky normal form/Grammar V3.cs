using System;
using System.Collections.Generic;
using System.Text;
using System.IO;


namespace Chomsky_normal_form
{
    class Grammar
    {
		class Lexem
		{
			private string value; // нетерминал из которого выводиться
			private List<List<string>> rules; //выводимая строка
			public Lexem()
			{
				rules = new List<List<string>>();
			}
			public string getValue() //выдает нетерминал
            {
				return value;
            }
			public void setValue(string value) //задает нетерминал
            {
				this.value = value;
            }
			public void setRules(List<string> expressions) //задает строку вывода для нетерминала
            {
				this.rules.Add(expressions);
            }
			public List<string> getRules(int i) //выдает строку
            {
				return rules[i];
            }
			public int getRules_rows() //выдает количество строк
            {
				return rules.Count;
            }
			public int getRules_column(int i)
            {
				return rules[i].Count;
            }			
		}

        private List<string> noterminals = new List<string>(); //нетерминалы
        private List<string> terminals = new List<string>(); //терминалы
        private List<Lexem> rules = new List<Lexem>(); //правила
		private List<Lexem> rules_duble = new List<Lexem>();
		private String initial;

        private List<string> noterminals_V = new List<string>(); //нетерминалы вывода 
       // private List<string> terminals_V = new List<string>(); //терминалы вывода
		private List<Lexem> rules_V = new List<Lexem>(); //правила вывода

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
					else if (buff[j] == finish[0]) theend = false; //нашли стопсимвол и выходим из цикла
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
					else if (buff[j] == finish[0]) theend = false;//нашли стопсимвол и выходим из цикла
					else
					{
						Console.WriteLine("Something went wrong while reading the second line");
						theend = false; // в любой непонятной ситуайии выходим из цикла
					}
				}
				size_buff = buff.Length; //осталась последняя строка
				value = buff.Substring(0, size_buff - 1); //копируем последнюю строку
				terminals.Add(value); //добавляем строку в лист
				Lexem rules_value;

				while (!file.EndOfStream) //пока не конец файла
				{
					buff = file.ReadLine(); //читаем строку. правила
					theend = true; //запускаем цикл
					expressions = new List<string>(); //создаем новую строку таблицы листов
					int k = 0;
					bool duble = false;
					int duble_num = -1;
					rules_value = new Lexem();

					while (theend) //пока не нашли ^
					{
						while (buff[j] != litter[0] && buff[j] != finish[0]) j++; //отмеряем размер строки до *
						if (buff[j] == litter[0] && k == 0) //нашли разделяющий символ
						{
							value = buff.Substring(0, j); //копируем строку до * в отдельную переменную							
							for (int i = 0; i < rules.Count; i++) //проверяем есть ли уже правило с таким нетерминалом
							{
								if (rules[i].getValue() == value)
								{
									duble = true;
									duble_num = i;
									buff = buff.Remove(0, j + 1); //удаляем строку вместе с *
								}
							}
							if (!duble)
							{
								rules_value.setValue(value);
								buff = buff.Remove(0, j + 1); //удаляем строку вместе с *								
							}
							theend = true;
						}
						else if (buff[j] == litter[0])
						{
							value = buff.Substring(0, j); //копируем строку до * в отдельную переменную
							expressions.Add(value); //добавляет строку в лист
							buff = buff.Remove(0, j + 1); //удаляем строку вместе с *							
							theend = true;
						}
						else if (buff[j] == finish[0]) theend = false; //нашли стопсимвол и выходим из цикла
						else
						{
							Console.WriteLine("something went wrong while reading the rules");
							theend = false; // в любой непонятной ситуайии выходим из цикла
						}
						j = 0; //начинаем с начала строки
						k++;
					}
					size_buff = buff.Length; //осталась последняя строка
					value = buff.Substring(0, size_buff - 1); //копируем последнюю строку
					expressions.Add(value); //добавляем строку в лист

					if (duble) rules[duble_num].setRules(expressions);
					else if (!duble)
					{
						rules_value.setRules(expressions);
						rules.Add(rules_value);
					}
				}
				file.Close(); //закрываем файл

				if (File.Exists(curfile)) //проверяем открыт ли файл
				{
					StreamReader file1 = new StreamReader("Grammar.txt");
					string buff_1;
					for (int i = 0; i < 3; i++) buff_1 = file1.ReadLine();

					Lexem rules_value_1;
					bool theend_1;
					List<string> expressions_1 = new List<string>();
					string value_1;
					int size_buff_1;

					while (!file1.EndOfStream) //пока не конец файла
					{
						buff_1 = file1.ReadLine(); //читаем строку. правила
						theend_1 = true; //запускаем цикл
						expressions_1 = new List<string>(); //создаем новую строку таблицы листов
						int k_1 = 0;
						bool duble_1 = false;
						int duble_num_1 = -1;
						rules_value_1 = new Lexem();

						while (theend_1) //пока не нашли ^
						{
							while (buff_1[j] != litter[0] && buff_1[j] != finish[0]) j++; //отмеряем размер строки до *
							if (buff_1[j] == litter[0] && k_1 == 0) //нашли разделяющий символ
							{
								value_1 = buff_1.Substring(0, j); //копируем строку до * в отдельную переменную							
								for (int i = 0; i < rules_duble.Count; i++) //проверяем есть ли уже правило с таким нетерминалом
								{
									if (rules_duble[i].getValue() == value_1)
									{
										duble_1 = true;
										duble_num_1 = i;
										buff_1 = buff_1.Remove(0, j + 1); //удаляем строку вместе с *
									}
								}
								if (!duble_1)
								{
									rules_value_1.setValue(value_1);
									buff_1 = buff_1.Remove(0, j + 1); //удаляем строку вместе с *								
								}
								theend_1 = true;
							}
							else if (buff_1[j] == litter[0])
							{
								value_1 = buff_1.Substring(0, j); //копируем строку до * в отдельную переменную
								expressions_1.Add(value_1); //добавляет строку в лист
								buff_1 = buff_1.Remove(0, j + 1); //удаляем строку вместе с *							
								theend_1 = true;
							}
							else if (buff_1[j] == finish[0]) theend_1 = false; //нашли стопсимвол и выходим из цикла
							else
							{
								Console.WriteLine("something went wrong while reading the rules");
								theend_1 = false; // в любой непонятной ситуайии выходим из цикла
							}
							j = 0; //начинаем с начала строки
							k_1++;
						}
						size_buff_1 = buff_1.Length; //осталась последняя строка
						value_1 = buff_1.Substring(0, size_buff_1 - 1); //копируем последнюю строку
						expressions_1.Add(value_1); //добавляем строку в лист

						if (duble_1) rules_duble[duble_num_1].setRules(expressions_1);
						else if (!duble_1)
						{
							rules_value_1.setRules(expressions_1);
							rules_duble.Add(rules_value_1);
						}
					}
					file1.Close();
				}				
			}
		}

		private bool Check() //проверяет есть ли првило S->&
        {
			bool lambda_rule = false; //есть лямда правило из S
			bool right_axiom = false; //есть S в правой части
			List<string> expressions = new List<string>();
			for (int i = 0; i < rules.Count; i++)
				for (int j = 0; j < rules[i].getRules_rows(); j++) //проходимся по всем правилам
				{
					expressions = rules[i].getRules(j);
					for (int t = 0; t < expressions.Count; t++)
					{
						if (rules[i].getValue() == "S" && expressions[t] == "&") lambda_rule = true;
						if (expressions[t] == "S") right_axiom = true;
					}
				}				
			if ((lambda_rule && !right_axiom) || (!lambda_rule)) return true; //если есть лямда правило и нет s в правой части то гуд или лямда правила вообще нет
			else return false;
        }
		private bool Check_term_and_noterm(string x, List<string> term_noterm) //проверяет принадлежит ли нетерм/терм списку
        {
			bool cheking = false; //переменная показывающая, есть ли такой нетерм/терм в списке
			int i = 0;
			while(!cheking && i< term_noterm.Count) //ходим по списку пока не найдем нетерм/терм или не дойдем до конца
			{
				if (x == term_noterm[i]) cheking = true;
				i++;
            }
			return cheking; //выдаем true or false
		}
		private void If_check(string noterm, string term)  //проверяет есть ли в нетерминалах такой нетерминал и добавляет если нет
		{
			List<string> expressions;
			Lexem rules_value;
			if (!Check_term_and_noterm(noterm, noterminals_V)) //проверяем есть ли такой нетерм в списке
			{
				noterminals_V.Add(noterm); //добавляем новый нетерм
				expressions = new List<string>(); //создаем строку для вывода
				rules_value = new Lexem(); //создаем класс правила
				rules_value.setValue(noterm); //добавляем нетерм, из которого выводиться строка
				expressions.Add(term); //добавляем терминал в строку
				rules_value.setRules(expressions); //добавляем в правило строку вывода
				rules_V.Add(rules_value); //добавляем новое правило в список классов
			}
		}
		private void Add_two_liter(List<string> two, string value) //метод анализирует два символа в троке и создает новые нетермы в список и добавляем правило в класс
		{
			List<string> expressions;
			Lexem rules_value;			
			string noterm; //новый нетерм. нужно для добавления нового нетерма
			int num = -1;
			for (int j = 0; j < rules_V.Count; j++) if (value == rules_V[j].getValue()) num = j;

			if (Check_term_and_noterm(two[0], noterminals) && Check_term_and_noterm(two[1], noterminals)) // правило у которого с права два нетерма
			{
				if(num == -1) //если такого нетерма еще небыло добавленно в список правил
                {
					rules_value = new Lexem();
					rules_value.setValue(value);
					rules_value.setRules(two);
					rules_V.Add(rules_value); //просто добавляем его в правила без изменений
				}
                else rules_V[num].setRules(two); //просто добавляем его в правила без изменений
			}
			else if (Check_term_and_noterm(two[0], terminals) && Check_term_and_noterm(two[1], noterminals)) //если вправа первый символ терминал, а второй нетерм
			{
				noterm = two[0] + "*"; //новый нетерм
				expressions = new List<string>(); //создаем лист вывода
				expressions.Add(noterm); //добавляем первый нетерм
				expressions.Add(two[1]); //добавляем второй нетерм

				if (num == -1)
                {
					rules_value = new Lexem(); //создаем класс
					rules_value.setValue(value); //добавляем нетерм из котрого выводиться строка
					rules_value.setRules(expressions); //добавляем строку правила
					rules_V.Add(rules_value); //добавляем новое правило в лист классов
				}
                else rules_V[num].setRules(expressions); //добавляем новое правило в уже существующий класс

				If_check(noterm, two[0]); //проверяет есть ли в нетерминалах такой нетерминал и добавляет если нет
			}
			else if (Check_term_and_noterm(two[0], noterminals) && Check_term_and_noterm(two[1], terminals)) //если у нас справа первый символ нетерм, а второй терм
			{
				noterm = two[1] + "*"; //создаем новый нетерм				
				expressions = new List<string>();
				expressions.Add(two[0]); //добавляем в строку нетерм
				expressions.Add(noterm); //добавляем в троку новый нетерм

				if (num == -1)
                {
					rules_value = new Lexem();
					rules_value.setValue(value); //добавляем в класс нетерм из которого выводиться правило
					rules_value.setRules(expressions); //добавляем в класс новое правило
					rules_V.Add(rules_value); //добавляем новое правило в список классов
				}
				else rules_V[num].setRules(expressions); //добавляем новое правило в уже существующий класс

				If_check(noterm, two[1]);
			}
            else
            {
				string noterm_2 = two[0] + "*";
				noterm = two[1] + "*"; //создаем новый нетерм
				expressions = new List<string>();
				expressions.Add(noterm_2); //добавляем новый нетрм в список
				expressions.Add(noterm); //добавляем в список новый нетерм

				if (num == -1)
				{
					rules_value = new Lexem();
					rules_value.setValue(value);
					rules_value.setRules(expressions);
					rules_V.Add(rules_value); //добавляем новое правило
				}
				else rules_V[num].setRules(expressions); //добавляем новое правило

				If_check(noterm, two[1]); //проверяем есть ли уже такой нетерм в списке
				If_check(noterm_2, two[0]);
			}
		}
		private void Chomsky() //сам алгоритм Хомского
		{
			List<string> expressions; //буффер лист
			List<string> buff_rules = new List<string>();
			Lexem rules_value;	//буффер класс
			string noterm; //новый нетерм. нужно для добавления нового нетерма

			for (int i = 0; i < noterminals.Count; i++) noterminals_V.Add(noterminals[i]); //копирую все нетерминалы в новый список нерминалов
			for (int i = 0; i < rules.Count; i++) //сколько классов
				for (int t = 0; t < rules[i].getRules_rows(); t++) //сколько строк в классе
				{
					buff_rules = rules[i].getRules(t); //строка правила
					if (buff_rules.Count == 1 && ((rules[i].getValue() == "S" && buff_rules[0] == "&") || (Check_term_and_noterm(buff_rules[0], terminals)))) //если есть правило S->&, то его добавляем в правила || правило у которого с права один символ && если справа терминал то добавляем его в новые правила
					{
						int num = -1; //если такой нетерм уже есть, то правило добавляется в уже сущ класс
						for (int j = 0; j < rules_V.Count; j++) if (rules[i].getValue() == rules_V[j].getValue()) num = j;

						expressions = new List<string>();
						expressions.Add(buff_rules[0]);

						if (num == -1) //такого нетерма еще небыло
                        {							
							rules_value = new Lexem(); //создаем новый класс
							rules_value.setValue(rules[i].getValue()); //задаем нетерм из которого выводиться строка				
							rules_value.setRules(expressions); //строка правила
							rules_V.Add(rules_value); //класс добавляется в список классов
						}
                        else rules_V[num].setRules(expressions); //такой нетерм уже есть и мы просто добавляем строку праавила в сущ класс		
					}
					else if (buff_rules.Count == 2) Add_two_liter(buff_rules, rules[i].getValue());
					else if (buff_rules.Count > 2) //если вывод больше чем из 2 символов
					{
						string rule_start = rules[i].getValue();
						while (buff_rules.Count != 2) //в случае если символов больше двух, то циклически уменишаем их численность
						{
							string line_noterm = "<";
							int num = -1; //если такой нетерм уже есть, то правило добавляется в уже сущ класс
							for (int j = 0; j < rules_V.Count; j++) if (rule_start == rules_V[j].getValue()) num = j;

							if (Check_term_and_noterm(buff_rules[0], terminals)) //если первым символом вывода был терминал
							{
								noterm = buff_rules[0] + "*"; //создаем новый нетерминал
								expressions = new List<string>();
								expressions.Add(noterm); //добавляем вывод
								for (int w = 1; w < buff_rules.Count; w++) line_noterm = line_noterm + buff_rules[w]; //создаем новый нетерм
								line_noterm = line_noterm + ">";
								expressions.Add(line_noterm); //добавляем новый нетерм в вывод

								if (num == -1) //если такого нетерма еще небыло
                                {
									rules_value = new Lexem(); //создаем новый класс для правил нетерма
									rules_value.setValue(rule_start); //в новый класс добавляем нетерм из которого выводиться
									rules_value.setRules(expressions); //добавляем строку вывода в класс
									rules_V.Add(rules_value); //добавляем новый класс правила в список								
								}
								else rules_V[num].setRules(expressions); //добавляем новое правило в класс, если такой нетерм уже был

								noterminals_V.Add(line_noterm); //добавляем также новый нетерминал
								If_check(noterm, buff_rules[0]); //проверяем был ли такой терминал ранее, если нет, то добавляется в список
								buff_rules.RemoveAt(0); //удаляем первый символ в строке
								rule_start = line_noterm; //теперь нетерм из которой выводиться правило новая
							}
							else
							{
								for (int w = 1; w < buff_rules.Count; w++) line_noterm = line_noterm + buff_rules[w]; //создаем новый нетерм
								line_noterm = line_noterm + ">";

								expressions = new List<string>();
								expressions.Add(buff_rules[0]);
								expressions.Add(line_noterm);

								if (num == -1)
								{
									rules_value = new Lexem();
									rules_value.setValue(rule_start); //добавляем из чего выводиться вывод
									rules_value.setRules(expressions); //добавляем новую строку вывода в класс
									rules_V.Add(rules_value); //добавляем новый класс
								}
                                else rules_V[num].setRules(expressions); //добавляем новое правило

								noterminals_V.Add(line_noterm); //добавляем также новый нетерминал
								buff_rules.RemoveAt(0);
								rule_start = line_noterm; //теперь строка из которой выводиться правило новая
							}
						}
						if (buff_rules.Count == 2) Add_two_liter(buff_rules, rule_start); //если осталось только 2 сивола
					}
				}
		}

		public void Start_metod() //проверяет на корректность данных (ну только на наличие лямда правила) и запускает алгоритм
        {
			if (Check()) Chomsky();
			else Console.Write("Error!"); //если в грамматике есть S->& и при этом справа в каком-то правиле есть S
		}
		public void Write_Grammar()
		{
			List<string> buff_rules = new List<string>();
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
			for (int i = 0; i < rules_duble.Count; i++) // Чуть сложного кода для карсивого вывода)
			{
				Console.WriteLine("    " + rules_duble[i].getValue() + " -> ");
				for (int j = 0; j < rules_duble[i].getRules_rows(); j++)
				{
					buff_rules = rules_duble[i].getRules(j);
					for(int t=0; t< buff_rules.Count; t++) Console.Write(buff_rules[t]);
					Console.WriteLine(" ");
				}
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
				Console.WriteLine("    " + rules_V[i].getValue() + " -> ");
				for (int j = 0; j < rules_V[i].getRules_rows(); j++)
				{
					buff_rules = rules_V[i].getRules(j);
					for (int t = 0; t < buff_rules.Count; t++) Console.Write(buff_rules[t]);
					Console.WriteLine(" ");
				}
			}
		}
	}
}
