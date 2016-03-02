using System;
using System.Collections.Generic;

[Serializable]
public class EldritchHeritageTrait : Trait
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
		bool use = false;
		bool iseldritch = false;
		var nume = data.type.GetEnumerator ();
		while (nume.MoveNext()) 
		{
			if (nume.Current.CompareTo("use")==0)
				use = true;
		}
		nume = data.type.GetEnumerator ();
		while (nume.MoveNext()) 
		{
			if (nume.Current.CompareTo("eldritch")==0)
				iseldritch = true;
		}
		if (use && iseldritch) 
			data.bonus.Add (new KeyValuePair<List<string>, int> (new List<string> (), 4));
	}
	
	public override string GetDescription()
	{
		return "This unit gains a +4 bonus when using eldritch artifacts.";
	}
	
	public override string GetName()
	{
		return "Eldritch Heritage";
	}
}

public class EldritchHeritageTraitFactory : TraitFactory
{
	public override Trait Instantiate()
	{
		return new EldritchHeritageTrait();
	}
	
	public override string GetDescription()
	{
		return "You gain a +4 bonus when using eldritch artifacts.";
	}
	
	public override string GetName()
	{
		return "Eldritch Heritage";
	}
	
	public override List<string> GetCategories()
	{
		return new List<string>();
	}
}