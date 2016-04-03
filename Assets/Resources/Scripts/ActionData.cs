using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class ActionData
{
    public ActionData()
    {
        path = new List<Vector3>();
    }

    string tooltip;
    List<Vector3> path;
}