using UnityEngine;
using System.Collections.Generic;

public class TraitList
{
    public Dictionary<string, TraitFactory> traits;

    public TraitList()
    {
        traits = new Dictionary<string, TraitFactory>();

        AddTrait(new ExemplarTraitFactory());
        AddTrait(new PerseveranceTraitFactory());
        AddTrait(new AltEnduranceTraitFactory());
    }

    public void AddTrait(TraitFactory factory)
    {
        traits.Add(factory.GetName(), factory);
    }
}
