using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class laser : weapon
{

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (enemy == null)
        {
            if (transform.parent.GetComponent<core>().enemy != null)
            {
                enemy = transform.parent.GetComponent<core>().enemy;
            }
        }
        if (enemy != null && Fire == true)
        {
            if(transform.parent.GetComponent<core>().aim)
            {
                attack();
                Fire = false;
            }
            
        }
        if (Fire == false)
        {
            Currentdelay -= Time.deltaTime;
        }
        if (Currentdelay <= 0)
        {
            Currentdelay = delayfire;
            Fire = true;
        }
    }

    public override void attack()
    {
        GameObject clonebullet = Instantiate(bullet);
        clonebullet.transform.position = bulletout.transform.position;
        clonebullet.transform.eulerAngles = bulletout.transform.eulerAngles;
    }
}
