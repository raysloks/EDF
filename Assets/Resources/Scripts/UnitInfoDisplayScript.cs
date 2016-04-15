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

    Text[] statTexts;
    
	void Start()
    {
        statTexts = GetComponentsInChildren<Text>();
    }
	
	void Update()
    {
        if (tm.current_turnholder != null)
            if (tm.current_turnholder.player)
                last_turnholder = tm.current_turnholder;
        if (cs != null)
        {
            nameText.text = cs.stats.name;

            if (cs.team == last_turnholder.team)
            {
                healthFillImage.color = new Color(0.0f, 1.0f, 0.0f, 1.0f);
                healthFill.anchorMax = new Vector2(Mathf.Max(0.0f, ((float)cs.hp.current) / ((float)cs.hp.max)), 1.0f);

                healthText.text = cs.hp.current + " / " + cs.hp.max;

                statTexts[0].text = "STR: " + cs.stats.STR;
                statTexts[1].text = "DEX: " + cs.stats.DEX;
                statTexts[2].text = "CON: " + cs.stats.CON;
                statTexts[3].text = "INT: " + cs.stats.INT;
                statTexts[4].text = "WIS: " + cs.stats.WIS;
                statTexts[5].text = "CHA: " + cs.stats.CHA;
            }
            else
            {
                healthFillImage.color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
                healthFill.anchorMax = new Vector2(cs.hp.current > 0 ? 1.0f : 0.0f, 1.0f);

                healthText.text = (cs.hp.current - cs.hp.max).ToString();

                statTexts[0].text = "STR: ?";
                statTexts[1].text = "DEX: ?";
                statTexts[2].text = "CON: ?";
                statTexts[3].text = "INT: ?";
                statTexts[4].text = "WIS: ?";
                statTexts[5].text = "CHA: ?";
            }
        }
        else
        {
            gameObject.SetActive(false);
        }
	}
}
