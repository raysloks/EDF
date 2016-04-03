using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public abstract class CharacterAction
{
    public abstract void Activate(Vector3 position, ClickScript cs);
    public abstract ActionData GetData(Vector3 position, ClickScript cs);
}