using System;
using System.Collections.Generic;

[Serializable]
public class BrandedTrait : Trait
{
	public override void Attach(ClickScript cs)
	{
		cs.onRoll[0f] += OnRoll;
	}
	
	public override void Detach(ClickScript cs)
	{
		cs.onRoll [0f] -= OnRoll;
	}
	void OnRoll()

	}
	
	public override string GetDescription()
	{
		return "This unit gain a -4 penalty when praying";
	}
	
	public override string GetName()
	{
		return "Branded";
	}
}

public class BrandedTraitFactory : TraitFactory
{
	public override Trait Instantiate()
	{
		return new BrandedTrait();
	}
	
	public override string GetDescription()
	{
		return "You gain a -4 penalty when praying";
	}
	
	public override string GetName()
	{
		return "Branded";
	}
	
	public override List<string> GetCategories()
	{
		return new List<string>();
	}
}