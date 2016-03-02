using System;
using System.Collections.Generic;

[Serializable]
public class DarkvisionTrait : Trait
{
	public override void Attach(ClickScript cs)
	{
		cs.onRoll[100.0f] += OnRoll;
	}
	
	public override void Detach(ClickScript cs)
	{
		cs.onRoll[100.0f] -= OnRoll;
	}
	

	void OnRoll(ClickScript cs, RollData data)
	{
		for (int i=0;i<data.bonus.Count;++i)
		{
			var nume = data.bonus[i].Key.GetEnumerator();
			bool is_darkness_penalty = false;
			while (nume.MoveNext())
			{
				if (nume.Current.CompareTo("darkness") == 0)
					is_darkness_penalty = true;
			}
			if (is_darkness_penalty)
				data.bonus[i] = new KeyValuePair<List<string>, int>(data.bonus[i].Key, data.bonus[i].Value + 4);
		}
	}
	
	public override string GetDescription()
	{
		return "This unit's penalty for fighting in the dark is reduced by 4.";
	}
	
	public override string GetName()
	{
		return "Darkvision";
	}
}

public class DarkvisionTraitFactory : TraitFactory
{
	public override Trait Instantiate()
	{
		return new DarkvisionTrait();
	}
	
	public override string GetDescription()
	{
		return "Your penalty for fighting in the dark is reduced by 4.";
	}
	
	public override string GetName()
	{
		return "Darkvision";
	}
	
	public override List<string> GetCategories()
	{
		return new List<string>();
	}
}