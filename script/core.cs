using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class core : tower
{

    public GameObject enemy;
    public int cost;
    public Transform maxrange;
    public bool aim;
    public float rotatespeed;
    public bool airaim;

    void Start ()
    {
        stage = GameObject.Find("Canvas").GetComponent<stagecontrol>();
        enemy = null;
        maxrange.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + radius);
        aim = false;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(stage==null)
        {
            stage = GameObject.Find("Canvas").GetComponent<stagecontrol>();
        }
        if(enemy!=null&&Vector3.Distance(gameObject.transform.position,enemy.transform.position)>radius)
        {
            enemy = null;
        }
        if ((enemy == null)&&stagecontrol.phase==2)
        {
            float neardis = 10000;
            foreach (GameObject item in stage.allenemy)
            {
                {
                    float nowdis = Vector3.Distance(gameObject.transform.position, item.transform.position);
                    if(airaim && item.transform.position.y>=45)
                    {
                        
                        foreach (var item2 in gameObject.GetComponentInParent<tower>().Towerweapon)
                        {
                            if (item2.GetComponent<weapon>().airfire==true)
                            {
                                if (nowdis <= radius * 2 && nowdis < neardis)
                                {
                                    neardis = nowdis;
                                    enemy = item;
                                }
                            }
                        }
                    }
                    else if (nowdis<=radius&&nowdis<neardis && item.transform.position.y < 45)
                    {
                        Debug.Log("<=45");
                        neardis = nowdis;
                        enemy = item;
                    }
                    
                }
            }
            
        }
        if (enemy!=null)
        {
            var lookPos = enemy.transform.position - transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime* rotatespeed);
        }
        RaycastHit hit;
        if(Physics.Linecast(transform.position, maxrange.position,out hit))
        {
            if(hit.collider.gameObject.tag=="enemy")
            {
                aim = true;
            }
        }
        else
        {
            aim = false;
        }
        Debug.DrawLine(transform.position, maxrange.position,Color.cyan);

    }
}
