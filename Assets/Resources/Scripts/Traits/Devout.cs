using System;
using System.Collections.Generic;

[Serializable]
public class DevoutTrait : Trait
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
		bool ispray = false;
		bool isholy = false;
		var nume = data.type.GetEnumerator ();
		while (nume.MoveNext()) 
		{
			if (nume.Current.CompareTo("pray")==0)
				ispray = true;
		}
		nume = data.type.GetEnumerator ();
		while (nume.MoveNext()) 
		{
			if (nume.Current.CompareTo("holy")==0)
				isholy = true;
		}
		if (ispray && isholy) 
			data.bonus.Add (new KeyValuePair<List<string>, int> (new List<string> (), 2));
	}
	
	public override string GetDescription()
	{
		return "This unit gains a +2 bonus when praying at a holy altar.";
	}
	
	public override string GetName()
	{
		return "Devout";
	}
}

public class DevoutTraitFactory : TraitFactory
{
	public override Trait Instantiate()
	{
		return new DevoutTrait();
	}
	
	public override string GetDescription()
	{
		return "You gain a +2 bonus when praying at a holy altar.";
	}
	
	public override string GetName()
	{
		return "Devout";
	}
	
	public override List<string> GetCategories()
	{
		return new List<string>();
	}
}