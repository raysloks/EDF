using System;
using System.Collections.Generic;

[Serializable]
public class HungerStatus : Status
{
    int penalty;

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
        List<string> list = new List<string>();
        list.Add("starvation");
        data.bonus.Add(new KeyValuePair<List<string>, int>(list, -4));
    }

    public override string GetDescription()
    {
        return "This unit has a " + penalty + " penalty on all rolls.";
    }

    public override string GetName()
    {
        return penalty > -3 ? "Hungry" : penalty > -6 ? "Hunger" : "Starvation";
    }
}