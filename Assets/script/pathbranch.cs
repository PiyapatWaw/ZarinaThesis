using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class pathbranch
{
    public List<Vector3> waypoint = new List<Vector3>();
    public bool finish;
    public string name;


    public pathbranch(string name, Vector3 startpoint)
    {
        this.name = name;
        finish = false;
        waypoint.Add(startpoint);
    }
    public pathbranch(string name)
    {
        this.name = name;
        finish = false;
    }
    
}
