using System;

[Serializable]
public class Status
{
    public virtual void Attach(ClickScript cs) { }
    public virtual void Detach(ClickScript cs) { }

    public virtual string GetDescription()
    {
        return "Someone's forgotten to give this status effect a description.";
    }

    public virtual string GetName()
    {
        return "Someone's forgotten to give this status effect a name.";
    }
}
