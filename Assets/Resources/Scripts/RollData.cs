using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class RollData
{
    public ClickScript source, target;
    public List<string> type;
    public List<KeyValuePair<List<string>, int>> bonus;
    public List<int> roll;
    public RollData other;

    public RollData(RollData other)
    {
        source = other.source;
        target = other.target;

        type = new List<string>(other.type);

        bonus = new List<KeyValuePair<List<string>, int>>();

        roll = new List<int>();

        this.other = other;
        other.other = this;
    }

    public RollData()
    {
        type = new List<string>();

        bonus = new List<KeyValuePair<List<string>, int>>();

        roll = new List<int>();
    }

    public int Get()
    {
        int sum = 0;

        var nume = bonus.GetEnumerator();
        while (nume.MoveNext())
            sum += nume.Current.Value;

        var nume2 = roll.GetEnumerator();
        while (nume2.MoveNext())
            sum += nume2.Current;

        return sum;
    }

    public int GetBoth()
    {
        int sum = Get();
        if (other != null)
            sum -= other.Get();
        return sum;
    }
}