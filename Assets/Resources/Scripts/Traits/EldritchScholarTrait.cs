using System;
using System.Collections.Generic;

[Serializable]
public class EldritchScholarTrait : Trait
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
		bool identify = false;
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
		nume = data.type.GetEnumerator ();
		while (nume.MoveNext()) 
		{
			if (nume.Current.CompareTo("identify")==0)
				identify = true;
		}
		if (use && iseldritch || identify)
		{
			int smallest_id = 0;
			for (int i=1;i<data.roll.Count;++i)
			{
				if (data.roll[i]<data.roll[smallest_id])
					smallest_id = i;
			}
			data.roll[smallest_id] = Math.Max(data.roll[smallest_id], RandomManager.d6());
		}
	}
	
	public override string GetDescription()
	{
		return "This unit rerolls the lowest die and uses the highest of that result and the original one when attempting to identify an item or when using an eldritch artifact.";
	}
	
	public override string GetName()
	{
		return "Eldritch Scholar";
	}
}

public class EldritchScholarTraitFactory : TraitFactory
{
	public override Trait Instantiate()
	{
		return new EldritchScholarTrait();
	}
	
	public override string GetDescription()
	{
		return "You reroll the lowest die and use the highest of that result and the original one when attempting to identify an item or when using an eldritch artifact.";
	}
	
	public override string GetName()
	{
		return "Eldritch Scholar";
	}
	
	public override List<string> GetCategories()
	{
		return new List<string>();
	}
}