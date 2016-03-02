using System;
using System.Collections.Generic;

[Serializable]
public class CarnivoreTrait : Trait
{
	public override void Attach(ClickScript cs)
	{
		cs.onHealthChanged[-100.0f] += OnHealthChanged;
	}
	
	public override void Detach(ClickScript cs)
	{
		cs.onHealthChanged[-100.0f] -= OnHealthChanged;
	}
	
	void OnHealthChanged(ClickScript cs, int difference)
	{
		if (difference < 0)
			cs.hp.Damage(-1);
	}
	
	public override string GetDescription()
	{
		return "This unit regenerates 1 hp upon taking damage.";
	}
	
	public override string GetName()
	{
		return "Carnivore";
	}
}

public class CarnivoreTraitFactory : TraitFactory
{
	public override Trait Instantiate()
	{
		return new CarnivoreTrait();
	}
	
	public override string GetDescription()
	{
		return "You may create a unit of rotten food from an animal corpse";
	}
	
	public override string GetName()
	{
		return "Carnivore";
	}
	
	public override List<string> GetCategories()
	{
		return new List<string>();
	}
}