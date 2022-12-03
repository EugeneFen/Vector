    	class Grammar
	{
		private List<string> noterminals = new List<string>(); //нетерминалы
		private List<string> terminals = new List<string>(); //терминалы
		private List<List<string>> rules = new List<List<string>>(); //правила
		private String initial;
    
    public List<string> V = new List<string>();//множество достижимых символов (алг 2)
		List<string> V_start = new List<string>();
		bool begin_ = true;//

    public List<string> noterminals_V = new List<string>(); //нетерминалы вывода 
		public List<string> terminals_V = new List<string>(); //терминалы вывода
		public List<List<string>> rules_V = new List<List<string>>(); //правила вывода
    
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

public void Algorithm_2()
        {
			noterminals_V.Clear();
			terminals_V.Clear();
			rules_V.Clear();
			Selection();
		}
    
    }
