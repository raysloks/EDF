using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class TargetData
{
    public Vector2 start, end;
    public bool use_end, random;
    public ClickScript searcher;
    public List<List<Vector2>> paths = new List<List<Vector2>>();
}
