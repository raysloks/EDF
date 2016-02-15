using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public class EnduranceTrait : Trait
{
	public override void Attach(ClickScript cs)
	{
		cs.onTurnEnd += OnTurnEnd;
		cs.onRoll += OnRoll;
	}
	
	public override void Detach(ClickScript cs)
	{
		cs.onRoll -= OnRoll;
		cs.onTurnEnd -= OnTurnEnd;
	}
	
	void OnTurnEnd(ClickScript cs, TurnData data)
	{
		if (data.type == TurnType.dungeon) 
		{
			if (cs.hp.current < 3)
			{
				cs.hp.current += 1;
				cs.OnHealthChanged(1);
			}
		}
	}

	void OnRoll(ClickScript cs, RollData data)
	{
		for (int i=0;i<data.bonus.Count;++i)
		{
			var nume = data.bonus[i].Key.GetEnumerator();
			bool is_starvation_penalty = false;
			while (nume.MoveNext())
			{
				if (nume.Current.CompareTo("starvation") == 0)
					is_starvation_penalty = true;
			}
			if (is_starvation_penalty)
				data.bonus[i] = new KeyValuePair<List<string>, int>(data.bonus[i].Key, data.bonus[i].Value + 2);
		}
	}
	
	public override string GetDescription()
	{
		return "This unit regenerates 1 hp per dungeon round if the unit is below 3 hp. Penalty from starving is reduced by 2";
	}
	
	public override string GetName()
	{
		return "Endurance";
	}
}

public class EnduranceTraitFactory : TraitFactory
{
	public override Trait Instantiate()
	{
		return new EnduranceTrait();
	}
	
	public override string GetDescription()
	{
		return "You regenerate 1 hp every dungeon round if you are below 3 hp. Your penalty from starving gets reduced by 2.";
	}
	
	public override string GetName()
	{
		return "Endurance";
	}
	
	public override List<string> GetCategories()
	{
		return new List<string>();
	}
}