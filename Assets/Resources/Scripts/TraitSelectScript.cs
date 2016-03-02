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
                    if (tm.current_turnholder != null)
                    {
                        var trait = nume.Current.Value.Instantiate();
                        trait.Attach(tm.current_turnholder);
                        tm.current_turnholder.status.Add(trait);
                    }
            }
            y += 30.0f;
        }
        GUI.Label(new Rect(260.0f, 20.0f, 200.0f, 100.0f), GUI.tooltip);

        if (tm != null)
        {
            if (tm.current_turnholder != null)
                GUI.Label(new Rect(500.0f, 20.0f, 100.0f, 20.0f), tm.current_turnholder.hp.current.ToString() + " / " + tm.current_turnholder.hp.max.ToString());
            if (GUI.Button(new Rect(800.0f, 20.0f, 100.0f, 20.0f), "Save"))
                tm.save_manager.Save(tm);
            if (GUI.Button(new Rect(800.0f, 60.0f, 100.0f, 20.0f), "Load"))
                tm.save_manager.Load(tm);
            if (GUI.Button(new Rect(800.0f, 100.0f, 100.0f, 20.0f), "New Game"))
                tm.TempNewGame();
        }
    }
}
