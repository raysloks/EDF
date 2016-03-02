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
}