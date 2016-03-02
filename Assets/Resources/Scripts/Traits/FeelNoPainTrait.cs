using System;
using System.Collections.Generic;

[Serializable]
public class FeelNoPainTrait : Trait
{
	public override void Attach(ClickScript cs)
	{

	}
	
	public override void Detach(ClickScript cs)
	{

	}
	

	
	public override string GetDescription()
	{
		return "This unit You gain the \"painless\" type.";
	}
	
	public override string GetName()
	{
		return "Feel No Pain";
	}
}

public class FeelNoPainTraitFactory : TraitFactory
{
	public override Trait Instantiate()
	{
		return new FeelNoPainTrait();
	}
	
	public override string GetDescription()
	{
		return "You gain the \"painless\" type.";
	}
	
	public override string GetName()
	{
		return "Feel No Pain";
	}
	
	public override List<string> GetCategories()
	{
		return new List<string>();
	}
}