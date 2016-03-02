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

    public void FlipBonus()
    {
        for (int i=0;i<bonus.Count;++i)
            bonus[i] = new KeyValuePair<List<string>, int>(bonus[i].Key, -bonus[i].Value);
    }
}