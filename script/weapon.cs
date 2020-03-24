using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class weapon : tower
{
    public GameObject enemy;
    public int cost;
    public int weaponnumber;
    public Transform bulletout;
    public GameObject bullet;
    private bool fire = true;
    public float delayfire;
    private float currentdelay;
    public bool airfire;


    public float Currentdelay
    {
        get
        {
            return currentdelay;
        }

        set
        {
            currentdelay = value;
        }
    }

    public bool Fire
    {
        get
        {
            return fire;
        }

        set
        {
            fire = value;
        }
    }

    public virtual void attack()
    {
        
    }
}
