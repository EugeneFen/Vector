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
			Grammar a = new Grammar();
		
			a.CheckIsHaveTermOnrules();
			a.printCanBelanguage();
			a.getGrammar();
		}
	}
}
