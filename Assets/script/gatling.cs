using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gatling : weapon
{
    public Animator animetor;
    public int maxfire;
    public int currentfire;
    public float reloadtime;
    public float currentreload;
    private bool canfire;


    // Use this for initialization
    void Start ()
    {
        Currentdelay = delayfire;
        currentfire = 0;
        currentreload = reloadtime;
        canfire = true;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(enemy==null)
        {
            
            if(transform.parent.GetComponent<core>().enemy!=null)
            {
                enemy = transform.parent.GetComponent<core>().enemy;
            }
            animetor.SetBool("Fire", false);
        }
        if(enemy!=null)
        {
            if(transform.parent.GetComponent<core>().enemy == null)
            {
                enemy = null;
            }
        }
		if(enemy!=null&&Fire==true)
        {
            if(canfire&& transform.parent.GetComponent<core>().aim)
            {
                animetor.SetBool("Fire", true);
                attack();
                currentfire--;
                canfire = false;
            }
        }
        if(canfire == false)
        {
            Currentdelay -= Time.deltaTime;
        }
        if(Currentdelay<=0)
        {
            Currentdelay = delayfire;
            canfire = true;
        }
        if(currentfire<=0)
        {
            currentfire = maxfire;
            Fire = false;
        }
        if(Fire == false)
        {
            currentreload-=Time.deltaTime;
            animetor.SetBool("Fire", false);
        }
        if(currentreload<=0)
        {
            currentreload = reloadtime;
            Fire = true;
        }
    }

    public override void attack()
    {
        GameObject clonebullet = Instantiate(bullet);
        if(Random.Range(0, 10)>7)
        {
            clonebullet.GetComponent<bullet>().damage *= currentfire;
        }
        clonebullet.transform.position = bulletout.transform.position;
        clonebullet.transform.eulerAngles = bulletout.transform.eulerAngles;

    }
}
