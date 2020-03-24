using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class node
{
    public Vector3 position;
    public GameObject Placer;
    public GameObject nodebase;
    public bool havebase = false;
    public bool check = false;
    public bool select = false;
    public string name;
    public List<node> neighbor = new List<node>();
    public int finalblockdistance = 0;

    public node(Vector3 position, GameObject placer)
    {
        this.position = position;
        Placer = placer;
        name = placer.name;
    }

    public node()
    {

    }

    public void setposition(Vector3 _pos)
    {
        position = _pos;
    }
    public void setPlacer(GameObject _Placer)
    {
        Placer = _Placer;
    }

}
