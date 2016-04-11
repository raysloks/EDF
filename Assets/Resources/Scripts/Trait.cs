using System;
using System.Collections.Generic;

[Serializable]
public class Trait : Status
{
}

public abstract class TraitFactory
{
    public abstract Trait Instantiate();

    public Trait AddTo(ClickScript cs)
    {
        var trait = Instantiate();
        trait.Attach(cs);
        cs.status.Add(trait);
        return trait;
    }

    public abstract string GetDescription();
    public abstract string GetName();
    public abstract List<string> GetCategories();
}