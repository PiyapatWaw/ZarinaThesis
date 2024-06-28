using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class missile : weapon
{
    public List<Transform> allbulletout = new List<Transform>();
    private float waiting;
    private float maxwaiting = 1.5f;
    private bool waitcomplete;
    private int podindex = 0;

	// Use this for initialization
	void Start ()
    {
        waitcomplete = true;
        waiting = maxwaiting;
        Currentdelay = delayfire;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (enemy == null)
        {
            if (transform.parent.GetComponent<core>().enemy != null)
            {
                enemy = transform.parent.GetComponent<core>().enemy;
            }
        }
        if (enemy != null && waitcomplete == true && Fire == true)
        {
            attack();
            waitcomplete = false;
        }
        if (Fire == false&& Currentdelay>0)
        {
            Currentdelay -= Time.deltaTime;
        }
        if (Currentdelay <= 0)
        {
            Currentdelay = delayfire;
            Fire = true;
            waitcomplete = true;
        }
        if(waitcomplete==false)
        {
            waiting -= Time.deltaTime;
        }
        if(waiting<=0)
        {
            waiting = maxwaiting;
            waitcomplete = true;
        }
    }

    public override void attack()
    {
        if (podindex == 2)
        {
            Fire = false;
            podindex = 0;
            
        }
        else if(podindex < 2&&Fire==true)
        {
            GameObject clonebullet = Instantiate(bullet);
            clonebullet.GetComponent<homing>().enemy = gameObject.GetComponent<missile>().enemy;
            clonebullet.transform.position = allbulletout[podindex].transform.position;
            clonebullet.transform.eulerAngles = allbulletout[podindex].transform.eulerAngles;
            podindex++;
        }
    }
}
