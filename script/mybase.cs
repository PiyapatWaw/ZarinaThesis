using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mybase : MonoBehaviour {

    public bool havetower;
    public GameObject ui;
    public GameObject tower;
    public Transform towerposition;
    private GameObject hovertower;

    public GameObject Hovertower
    {
        get
        {
            return hovertower;
        }

        set
        {
            hovertower = value;
        }
    }

    public void destroybase()
    {
        Grid mygrid = GameObject.Find("grid").GetComponent<Grid>();
        stagecontrol stage = stage = GameObject.Find("Canvas").GetComponent<stagecontrol>();

        foreach (var item in mygrid.map)
        {
            if (item.nodebase == gameObject)
            {
                if(tower!=null)
                {
                    tower.GetComponent<tower>().destroytower();
                }
                item.Placer.SetActive(true);
                item.havebase = false;
                stage.increasebase(1);
                GameObject[] allwaypoint = GameObject.FindGameObjectsWithTag("waypoint");
                foreach (var point in allwaypoint)
                {
                    point.GetComponent<waypoint>().allbranch.Clear();
                    point.GetComponent<waypoint>().intothegraph();
                }
            }
        }
        Destroy(gameObject);
    }

    private void OnMouseEnter()
    {
        playsceneUI uicontrol = GameObject.Find("Canvas").GetComponent<playsceneUI>();
        if (uicontrol.Hovertower != null && uicontrol.Currenttower != null && havetower == false)
        {
            if (uicontrol.Hovertower.tag == "tower")
            {
                hovertower = Instantiate(uicontrol.Hovertower);
                hovertower.transform.position = towerposition.position;
                hovertower.GetComponent<BoxCollider>().enabled = false;
                hovertower.GetComponent<tower>().selectcircle.SetActive(true);
                hovertower.GetComponent<tower>().enabled = false;
            }
        }
        
    }
    private void OnMouseExit()
    {
        Destroy(hovertower);
    }

}
