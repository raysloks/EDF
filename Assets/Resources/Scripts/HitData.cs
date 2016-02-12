using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class HitData
{
    public HitData(ClickScript source, ClickScript target)
    {
        this.source = source;
        this.target = target;
        damage = new List<KeyValuePair<List<string>, int>>();
    }

    public List<KeyValuePair<List<String>, int>> damage;
    public ClickScript target, source;
}
