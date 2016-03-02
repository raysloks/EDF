using System.Collections.Generic;

public class TraitList
{
    public Dictionary<string, TraitFactory> traits;

    public TraitList()
    {
        traits = new Dictionary<string, TraitFactory>();
        
        AddTrait(new BrandedTraitFactory());
        AddTrait(new CurseOfAgonyTraitFactory());
        AddTrait(new DarkvisionTraitFactory());
        AddTrait(new DeftTravellerTraitFactory());
        AddTrait(new DevilWorshipperTraitFactory());
        AddTrait(new DevoutTraitFactory());
        AddTrait(new EldritchHeritageTraitFactory());
        AddTrait(new EldritchScholarTraitFactory());
        AddTrait(new EnduranceTraitFactory());
        AddTrait(new ExemplarTraitFactory());
        AddTrait(new PerseveranceTraitFactory());
    }

    public void AddTrait(TraitFactory factory)
    {
        traits.Add(factory.GetName(), factory);
    }
}
