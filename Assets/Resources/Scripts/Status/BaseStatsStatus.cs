using System;
using System.Collections.Generic;

using UnityEngine;

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
        data.name = "Murk";
        if (cs.hp.current > 0)
            data.armor = 7;
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