using System;
using System.Collections.Generic;

[Serializable]
public class Trait : Status
{
}

public abstract class TraitFactory
{
    public abstract Trait Instantiate();

    public abstract string GetDescription();
    public abstract string GetName();
    public abstract List<string> GetCategories();
}