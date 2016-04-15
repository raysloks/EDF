using System;
using System.Collections.Generic;

using UnityEngine;

[Serializable]
public class BaseStatsStatus : Status
{
    public int STR, DEX, CON, INT, WIS, CHA;
    public int HP;

    public BaseStatsStatus()
    {
        STR = RandomManager.d6();
        DEX = RandomManager.d6();
        CON = RandomManager.d6();
        INT = RandomManager.d6();
        WIS = RandomManager.d6();
        CHA = RandomManager.d6();
    }

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
        {
            data.STR = STR;
            data.DEX = DEX;
            data.CON = CON;
            data.INT = INT;
            data.WIS = WIS;
            data.CHA = CHA;
        }
        else
        {
            data.name += " Corpse";
        }
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