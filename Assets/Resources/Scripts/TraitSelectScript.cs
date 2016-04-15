using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TraitSelectScript : MonoBehaviour {

    TraitList traits;

    public TurnManagerScript tm;

    public GameObject tsgo;

    public UnitInfoDisplayScript uid; //maybe remove from here later

	void Start()
    {
        traits = new TraitList();

        ReconstructGUI();
    }

    public void OnTraitSelected(string trait_name)
    {
        var tf = traits.traits[trait_name];
        if (tm != null)
        {
            if (tf != null)
            {
                if (tm.current_turnholder != null)
                {
                    var trait = tf.Instantiate();
                    trait.Attach(tm.current_turnholder);
                    tm.current_turnholder.status.Add(trait);
                }
            }
        }
    }

    void ReconstructGUI()
    {
        var nume = traits.traits.GetEnumerator();
        float y = -20.0f;
        while (nume.MoveNext())
        {
            GameObject test = Instantiate(Resources.Load("Prefabs/Trait")) as GameObject;
            test.transform.SetParent(tsgo.transform);
            test.transform.localPosition = new Vector3(100.0f, y, 0.0f);
            var text = test.GetComponentsInChildren<Text>();
            text[0].text = nume.Current.Key;
            text[1].text = nume.Current.Value.GetDescription();
            var button = test.GetComponentInChildren<Button>();
            string key = nume.Current.Key;
            button.onClick.AddListener(delegate { OnTraitSelected(key); });
            y -= 40.0f;
        }
        ((RectTransform)tsgo.transform).sizeDelta = new Vector2(0.0f, -y - 20.0f);
    }

    void OnGUI()
    {
        if (tm != null)
        {
            if (uid.cs != null)
            {
                var nume = uid.cs.status.GetEnumerator();
                float y = 20.0f;
                while (nume.MoveNext())
                {
                    GUI.Label(new Rect(500.0f, y, 100.0f, 20.0f), nume.Current.GetName());
                    y += 20.0f;
                }
            }

            if (GUI.Button(new Rect(800.0f, 20.0f, 100.0f, 20.0f), "Save"))
                tm.save_manager.Save(tm);
            if (GUI.Button(new Rect(800.0f, 60.0f, 100.0f, 20.0f), "Load"))
                tm.save_manager.Load(tm);
            if (GUI.Button(new Rect(800.0f, 100.0f, 100.0f, 20.0f), "New Game"))
                tm.TempNewGame();
            if (GUI.Button(new Rect(800.0f, 140.0f, 100.0f, 20.0f), "Exit Game") || Input.GetKey("escape"))
                Application.Quit();
        }
    }
}
