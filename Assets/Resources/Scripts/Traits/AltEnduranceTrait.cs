using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public class AltEnduranceTrait : Trait
{
    public override void Attach(ClickScript cs)
    {
        cs.onTurnEnd += OnTurnEnd;
        cs.onRoll += OnRoll;
    }

    public override void Detach(ClickScript cs)
    {
        cs.onRoll -= OnRoll;
        cs.onTurnEnd -= OnTurnEnd;
    }

    void OnTurnEnd(ClickScript cs, TurnData data)
    {
        if (data.type == TurnType.dungeon)
        {
            if (cs.hp.current < 3)
            {
                cs.hp.current += 1;
                cs.OnHealthChanged(1);
            }
        }
    }

    void OnRoll(ClickScript cs, RollData data)
    {
        for (int i=0;i<data.bonus.Count;++i)
        {
            var nume = data.bonus[i].Key.GetEnumerator();
            bool is_starvation_penalty = false;
            while (nume.MoveNext())
            {
                if (nume.Current.CompareTo("starvation") == 0)
                    is_starvation_penalty = true;
            }
            if (is_starvation_penalty)
                data.bonus[i] = new KeyValuePair<List<string>, int>(data.bonus[i].Key, data.bonus[i].Value + 2);
        }
    }

    public override string GetDescription()
    {
        return "ENDURANCE";
    }

    public override string GetName()
    {
        return "AltEndurance";
    }
}

public class AltEnduranceTraitFactory : TraitFactory
{
    public override Trait Instantiate()
    {
        return new AltEnduranceTrait();
    }

    public override string GetDescription()
    {
        return "ENDURANCE";
    }

    public override string GetName()
    {
        return "AltEndurance";
    }

    public override List<string> GetCategories()
    {
        return new List<string>();
    }
}