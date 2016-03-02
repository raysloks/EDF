using System;
using System.Collections.Generic;

[Serializable]
public class CurseOfAgonyTrait : Trait
{
	public override void Attach(ClickScript cs)
	{
		cs.onTurnEnd[0.0f] += OnTurnEnd;
		cs.onRoll[100.0f] += OnRoll;
		cs.onHealthChanged [0.0f] += OnHealthChanged;
	}
	
	public override void Detach(ClickScript cs)
	{
		cs.onRoll[100.0f] -= OnRoll;
		cs.onTurnEnd[0.0f] -= OnTurnEnd;
	}

	int bonus;

	void OnTurnEnd(ClickScript cs, TurnData data)
	{
		if (data.type == TurnType.battle)
			bonus = 0;
	}
	void OnHealthChanged(ClickScript cs, int difference)
	{
		if (difference < 0)
			bonus -= 4;
	}
	void OnRoll(ClickScript cs, RollData data)
	{
		data.bonus.Add (new KeyValuePair<List<string>, int> (new List<string> (), bonus));
	}
	
	public override string GetDescription()
	{
		return "When this unit takes damage it gains a -4 penalty on all rolls until the end of its next turn. This doesn't function if it has the \"painless\" type.";
	}
	
	public override string GetName()
	{
		return "Curse of Agony";
	}
}

public class CurseOfAgonyTraitFactory : TraitFactory
{
	public override Trait Instantiate()
	{
		return new CurseOfAgonyTrait();
	}
	
	public override string GetDescription()
	{
		return "When you take damage you gain a -4 penalty on all rolls until the end of your next turn. This doesn't function if you have the \"painless\" type.";
	}
	
	public override string GetName()
	{
		return "Curse of Agony";
	}
	
	public override List<string> GetCategories()
	{
		return new List<string>();
	}
}