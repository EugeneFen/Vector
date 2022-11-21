using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace ConsoleApp1
{
	class Program
	{
		static void Main(string[] args)
		{
			string curfile = @"Grammar.txt";

			List<string> noterminals = new List<string>(); //нетерминалы
			List<string> terminals = new List<string>(); //терминалы
			List<List<string>> regulations = new List<List<string>>(); //правила

			if (File.Exists(curfile)) //проверяем открыт ли файл
			{
				StreamReader file = new StreamReader("Grammar.txt"); // говорим, что переменная file теперь ссылается на наш файл

				string value; //терминал или нетерминал
				value = ""; //нежно, чтобы коректно работал код
				int i = 1; 
				/*
				 * т.к. терминалы и нетерминалы это 1 символ, то на втором месте будет стоять либо * либо ^
				 * поэтому пока мы не нашли ^, то цикл будет работать
				 */
				int j = 0; //для поиска *
				string buff = file.ReadLine(); //первая строка файла
				string finish = "^"; // стопсивол
				string litter = "*"; //разделитель
				bool theend = true;
				List<string> expressions;

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
					else if (buff[j] == finish[0])
					{
						theend = false;
					}
					else theend = false;
				}
				int size_buff = buff.Length; //осталась последняя строка
				value = buff.Substring(0, size_buff-1); //копируем последнюю строку
				noterminals.Add(value); //добавляем строку в лист

				buff = file.ReadLine(); //читаем вторую строку файла
				theend = true;
				

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
					else if (buff[j] == finish[0])
					{
						theend = false;
					}
					else theend = false;
				}
				size_buff = buff.Length; //осталась последняя строка
				value = buff.Substring(0, size_buff - 1); //копируем последнюю строку
				terminals.Add(value); //добавляем строку в лист

				while (!file.EndOfStream)
                {
					buff = file.ReadLine();
					theend = true;
					expressions = new List<string>();

					while (theend) //пока не нашли ^
					{


						while (buff[j] != litter[0] && buff[j] != finish[0]) j++; //отмеряем размер строки до *
                        if(buff[j] == litter[0])
						{
							value = buff.Substring(0, j); //копируем строку до * в отдельную переменную
							expressions.Add(value); //добавляет строку в лист
							buff = buff.Remove(0, j + 1); //удаляем строку вместе с *
							j = 0; //начинаем с начала строки
							theend = true;

						}
                        else if(buff[j] == finish[0])
                        {
							theend = false;
						}
						else theend = false;
					}
					size_buff = buff.Length; //осталась последняя строка
					value = buff.Substring(0, size_buff - 1); //копируем последнюю строку
					expressions.Add(value); //добавляем строку в лист
					regulations.Add(expressions);					
				}

					file.Close(); //закрываем файл
			}

			//вывод перваго списка
			for (int i = 0; i < noterminals.Count; i++)
			{
				if (i > 0) Console.Write(", ");
				Console.Write(noterminals[i]);
			}

			Console.WriteLine(); //строка между выводами

			//вывод второго списка
			for (int i = 0; i < terminals.Count; i++)
			{
				if (i > 0) Console.Write(", ");
				Console.Write(terminals[i]);
			}

			Console.WriteLine(); //строка между выводами
			List<string> buffer = new List<string>();

			for (int i = 0; i < regulations.Count; i++) // Чуть сложного кода для карсивого вывода)
			{
				Console.Write("    " + regulations[0][0] + " -> ");
				for (int j = 1; j < regulations[i].Count; j++) 
				{
					if (j > 1) Console.Write(", ");
					Console.Write(regulations[i][j]);
				}
				Console.WriteLine(" ");
			}
		}
	}
}
