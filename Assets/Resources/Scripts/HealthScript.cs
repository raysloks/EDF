using UnityEngine;
using System.Collections;

public class HealthScript : MonoBehaviour {

    public int current, max;

    public void Damage(int damage) {
        current -= damage;
        if (current > max)
            current = max;
    }
}
