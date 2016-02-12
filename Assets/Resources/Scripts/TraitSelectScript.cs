using UnityEngine;
using System.Collections;

public class TraitSelectScript : MonoBehaviour {

    TraitList traits;

    public TurnManagerScript tm;

	void Start()
    {
        traits = new TraitList();
	}

    void OnGUI()
    {
        var nume = traits.traits.GetEnumerator();
        float y = 20.0f;
        while (nume.MoveNext())
        {
            if (GUI.Button(new Rect(30.0f, y, 200.0f, 20.0f), new GUIContent(nume.Current.Value.GetName(), nume.Current.Value.GetDescription())))
            {
                if (tm != null)
                {
                    if (tm.current_turnholder != null)
                    {
                        nume.Current.Value.Instantiate().Attach(tm.current_turnholder);
                    }
                }
            }
            y += 30.0f;
        }
        GUI.Label(new Rect(260.0f, 20.0f, 200.0f, 100.0f), GUI.tooltip);
    }
}
