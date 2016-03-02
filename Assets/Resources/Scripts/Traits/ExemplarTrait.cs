using System;
using System.Collections.Generic;

[Serializable]
public class ExemplarTrait : Trait
{
    public override void Attach(ClickScript cs)
    {
        cs.onRoll[-100.0f] += OnRoll;
    }

    public override void Detach(ClickScript cs)
    {
        cs.onRoll[-100.0f] -= OnRoll;
    }

    void OnRoll(ClickScript cs, RollData data)
    {
        data.bonus.Add(new KeyValuePair<List<string>, int>(new List<string>(), 1));
    }

    public override string GetDescription()
    {
        return "This unit gains a +1 bonus on all rolls.";
    }

    public override string GetName()
    {
        return "Exemplar";
    }
}

public class ExemplarTraitFactory : TraitFactory
{
    public override Trait Instantiate()
    {
        return new ExemplarTrait();
    }

    public override string GetDescription()
    {
        return "You gain a +1 bonus on all rolls.";
    }

    public override string GetName()
    {
        return "Exemplar";
    }

    public override List<string> GetCategories()
    {
        return new List<string>();
    }
}