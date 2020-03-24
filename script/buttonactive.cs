using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class buttonactive : MonoBehaviour
{
    public int resource;
    public int condition;
    public bool Base, Tower,Custom;
    public tower toweronbutton;
    public Button thisbutton;
    public GameObject costbar;
    public Text costxt;

	// Use this for initialization
	void Start ()
    {
        updatevariable();

	}
	
	// Update is called once per frame
	void Update ()
    {
		if(resource<condition)
        {
            thisbutton.interactable = false;
        }
        else
        {
            thisbutton.interactable = true;
        }

        updatevariable();
    }

    public void updatevariable()
    {
        if (Base)
        {
            resource = GameObject.Find("Canvas").GetComponent<stagecontrol>().getbase();
            condition = 1;
        }
        else if (Tower)
        {
            if(costbar.active==false)
            {
                costbar.SetActive(true);
            }
            resource = GameObject.Find("Canvas").GetComponent<stagecontrol>().getmoney();
            toweronbutton.changeprice();
            condition = toweronbutton.Price;
            costxt.text = condition.ToString();
        }
        else if (Custom)
        {

        }
    }
}
