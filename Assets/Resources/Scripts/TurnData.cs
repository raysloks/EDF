using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public enum TurnType
{
    battle,
    dungeon
}

public class TurnData
{
    public float time;
    public TurnType type;
}