using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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