using System;
using System.Collections.Generic;

[Serializable]
public class BaseStatsStatus : Status
{
    public int STR, DEX, CON, INT, WIS, CHA;
    public int HP;

    public override void Attach(ClickScript cs)
    {
        cs.onRecalculateStats[-100.0f] += OnRecalculateStats;
    }

    public override void Detach(ClickScript cs)
    {
        cs.onRecalculateStats[-100.0f] -= OnRecalculateStats;
    }

    void OnRecalculateStats(ClickScript cs, CharacterData data)
    {
    }

    public override string GetDescription()
    {
        return "This unit has base stats.";
    }

    public override string GetName()
    {
        return "Base Stats";
    }
}