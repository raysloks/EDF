using System;
using System.Collections.Generic;

[Serializable]
public class PerseveranceTrait : Trait
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
        return "Perseverance";
    }
}

public class PerseveranceTraitFactory : TraitFactory
{
    public override Trait Instantiate()
    {
        return new PerseveranceTrait();
    }

    public override string GetDescription()
    {
        return "You regenerate 1 hp upon taking damage.";
    }

    public override string GetName()
    {
        return "Perseverance";
    }

    public override List<string> GetCategories()
    {
        return new List<string>();
    }
}