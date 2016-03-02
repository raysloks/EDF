using System;
using System.Collections.Generic;

[Serializable]
public class AlertTrait : Trait
{
	public override void Attach(ClickScript cs)
	{

	}
	
	public override void Detach(ClickScript cs)
	{

	}
	

	}
	
	public override string GetDescription()
	{
		return "This unit have a 2/3 chance not to be surprised.";
	}
	
	public override string GetName()
	{
		return "Alert";
	}
}

public class AlertTraitFactory : TraitFactory
{
	public override Trait Instantiate()
	{
		return new AlertTrait();
	}
	
	public override string GetDescription()
	{
		return "You have a 2/3 chance to not be surprised.";
	}
	
	public override string GetName()
	{
		return "Alert";
	}
	
	public override List<string> GetCategories()
	{
		return new List<string>();
	}
}