using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UnitInfoDisplayScript : MonoBehaviour {

    public ClickScript cs;

    public TurnManagerScript tm;

    public Text nameText, healthText;
    public RectTransform healthFill;
    public Image healthFillImage;

    ClickScript last_turnholder;
    
	void Start()
    {
    }
	
	void Update()
    {
        if (tm.current_turnholder != null)
            if (tm.current_turnholder.player)
                last_turnholder = tm.current_turnholder;
        if (cs != null)
        {
            nameText.text = cs.stats.name;
            healthText.text = cs.hp.current + " / " + cs.hp.max;
            if (cs.team != last_turnholder.team)
                healthFillImage.color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
            else
                healthFillImage.color = new Color(0.0f, 1.0f, 0.0f, 1.0f);
            healthFill.anchorMax = new Vector2(Mathf.Max(0.0f, ((float)cs.hp.current) / ((float)cs.hp.max)), 1.0f);
        }
        else
        {
            gameObject.SetActive(false);
        }
	}
}
