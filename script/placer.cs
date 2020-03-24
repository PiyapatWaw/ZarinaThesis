using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class placer : MonoBehaviour
{
    private waypoint[] waypoints;
    public GameObject prebase;
    private GameObject mybase;
    public GameObject debugcanvas;
    public Text nodedebug1, nodedebug2, nodedebug3, nodedebug4;

    public GameObject Mybase
    {
        get
        {
            return mybase;
        }

        set
        {
            mybase = value;
        }
    }

    private void Start()
    {
        GameObject[] allwaypoints = GameObject.FindGameObjectsWithTag("waypoint");
        waypoints = new waypoint[allwaypoints.Length];
        for (int i = 0; i < waypoints.Length; i++)
        {
            waypoints[i] = allwaypoints[i].GetComponent<waypoint>();
        }
        //nodedebug.text = "true";
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag=="obstracle")
        {
            GameObject agrid = GameObject.Find("grid");
            Grid map = agrid.GetComponent<Grid>();
            foreach (var item in map.map)
            {
                foreach (var item2 in item.neighbor)
                {
                    if (item2.Placer == gameObject)
                    {
                        //item.neighbor.Remove(item2);
                    }
                }
            }
            Destroy(gameObject);
        }
        
    }


    private void OnMouseEnter()
    {
        Mybase = Instantiate(prebase);
        Mybase.transform.position = gameObject.transform.position;
        Mybase.GetComponent<BoxCollider>().enabled = false;
        if(Input.GetMouseButtonDown(0))
        {
            Destroy(Mybase);
        }
    }

    private void OnMouseExit()
    {
        Destroy(Mybase); 
    }






}
