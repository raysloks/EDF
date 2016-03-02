using System;
using System.Collections.Generic;

[Serializable]
public class DeftTravellerTrait : Trait
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
		var nume = data.type.GetEnumerator ();
		while (nume.MoveNext()) 
		{
			if (nume.Current.CompareTo("trap")==0 || nume.Current.CompareTo("ambush")==0)
				data.bonus.Add(new KeyValuePair<List<string>, int>(new List<string>(), 4));
		}
	}
	
	public override string GetDescription()
	{
		return "This unit gains a +4 bonus when rolling to avoid ambushes and traps.";
	}
	
	public override string GetName()
	{
		return "Deft Traveller";
	}
}

public class DeftTravellerTraitFactory : TraitFactory
{
	public override Trait Instantiate()
	{
		return new DeftTravellerTrait();
	}
	
	public override string GetDescription()
	{
		return "You gain a +4 bonus when rolling to avoid ambushes and traps.";
	}
	
	public override string GetName()
	{
		return "Deft Traveller";
	}
	
	public override List<string> GetCategories()
	{
		return new List<string>();
	}
}