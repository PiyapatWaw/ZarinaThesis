using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class tower : MonoBehaviour
{

    public GameObject towercore = null;
    public GameObject[] towerweapon;
    public bool[] weaponslot;
    private GameObject[] weaponposition;
    private int price;
    public GameObject mybase;
    public stagecontrol stage;
    public GameObject selectcircle;
    public GameObject towerui;
    public bool haveweapon;
    public float radius = 5.0f;

    public int Price
    {
        get
        {
            return price;
        }

        set
        {
            price = value;
        }
    }

    public GameObject Towercore
    {
        get
        {
            return towercore;
        }

        set
        {
            towercore = value;
        }
    }



    public bool[] Weaponslot
    {
        get
        {
            return weaponslot;
        }

        set
        {
            weaponslot = value;
        }
    }

    public GameObject[] Weaponposition
    {
        get
        {
            return weaponposition;
        }

        set
        {
            weaponposition = value;
        }
    }

    public GameObject[] Towerweapon
    {
        get
        {
            return towerweapon;
        }

        set
        {
            towerweapon = value;
        }
    }



    // Use this for initialization
    void Start ()
    {
        price = 0;
        if(Towerweapon.Length>0)
        {
            gameObject.GetComponentInChildren<core>().Towerweapon = new GameObject[Towerweapon.Length];
            for (int i = 0; i < Towerweapon.Length; i++)
            {
                if(Towerweapon[i]!=null)
                {
                    gameObject.GetComponentInChildren<core>().Towerweapon[i] = Towerweapon[i];
                }
                else
                {
                    gameObject.GetComponentInChildren<core>().Towerweapon[i] = null;
                }
            }
        }
        foreach (var item in Towerweapon)
        {
            if(item!=null)
            {
                haveweapon = true;
            }
        }

    }

    public void changeprice()
    {
        price = Towercore.GetComponent<core>().cost;
        for (int i = 0; i < Towerweapon.Length; i++)
        {
            price += Towerweapon[i].GetComponent<weapon>().cost;
        }
    }

    public void destroytower()
    {
        Grid mygrid = GameObject.Find("grid").GetComponent<Grid>();
        stagecontrol stage = GameObject.Find("Canvas").GetComponent<stagecontrol>();

        foreach (var item in mygrid.map)
        {
            if (item.nodebase == mybase)
            {
                changeprice();
                item.Placer.SetActive(true);
                item.nodebase.GetComponent<mybase>().havetower = false;
                stage.increasemoney(Convert.ToInt32(price / 3));
                
            }
        }
        Destroy(gameObject);
    }


}
