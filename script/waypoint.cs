using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class waypoint : MonoBehaviour
{
    public Transform vanguardpoint;
    public int MaxvanguardX, MinvanguardX, MaxvanguardY, MinvanguardY;
    public List<Vector3> point = new List<Vector3>();
    public List<nodescore> allscore = new List<nodescore>();
    public Grid map;
    public Vector3 start;
    public Vector3 final;
    public GameObject box;
    public List<GameObject> showbox = new List<GameObject>();
    public List<pathbranch> allbranch = new List<pathbranch>();
    public Vector3 sattelitepos;
    public Vector3 sattelitepoint;
    public bool useturnvanguard;
    public Vector3 turnvanguard;
    public bool airpath;
    public bool groundpath;
    public bool boss;
    public bool complete;

    public int stackpoint = -1;

    private void Start()
    {
        foreach (var item in map.map)
        {
            allscore.Add(new nodescore(item.position));
        }
        intothegraph();
    }

    private void Update()
    {
        if (stagecontrol.phase == 1||stagecontrol.phase==0)
        {
            LineRenderer line = gameObject.GetComponent<LineRenderer>();
            line.enabled = true;
        }
        else
        {
            LineRenderer line = gameObject.GetComponent<LineRenderer>();
            line.enabled = false;
        }

    }


    public void check(GameObject current)
    {

    }

    public void generatepoint(Vector3 current)
    {
        foreach (var item in map.map)
        {
            if (item.position == current)
            {
                Debug.Log(item.neighbor.Count);
            }
        }
    }





    int gotofianl = 0;
    int checkcount = 0;

    public int checker(GameObject current)
    {
        gotofianl = 0;
        checkcount = 0;
        node findstart = null;
        foreach (var item in map.map)
        {
            if (item.Placer == current)
            {
                item.check = true;
            }
            else
                item.check = false;
        }
        foreach (var item in map.map)
        {
            if (item.position == start)
            {
                findstart = item;
            }
        }

        foreach (var item in findstart.neighbor)
        {
            if (item.havebase == false)
            {
                checkcount++;
            }
        }

        graphdriving(findstart);
        return gotofianl;
    }

    public void graphdriving(node node)
    {
        foreach (var item in node.neighbor)
        {
            if (item.havebase == false && item.check == false)
                checkcount++;
        }
        if (checkcount > 0)
        {
            foreach (var item in node.neighbor)
            {
                if (item.havebase == false && item.position != final && item.check == false)
                {
                    item.check = true;
                    checkcount--;
                    graphdriving(item);

                }
                else if (item.position == final)
                {
                    gotofianl++;
                }
            }
        }

    }



    public void intothegraph()
    {
        
        if (groundpath||boss)
        {
            foreach (var item in showbox)
            {
                Destroy(item);
            }
            showbox.Clear();

            //point.Clear();
            
            setscore();
            allbranch.Add(new pathbranch("start1",start));
            allbranch.Add(new pathbranch("start2",start));
            allbranch.Add(new pathbranch("start3",start));
        }
        
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        foreach (var item in map.map)
        {
            item.select = false;

        }

        node startnode = new node();
        foreach (var item in map.map)
        {

            if ((item.position == start))
            {
                item.select = true;
                startnode = item;
                break;

            }
        }
        if(startnode!=null)
        {
            foreach (var item2 in allbranch)
            {
                if (groundpath)
                {
                    generategroundpath(startnode, item2);
                }
                else if (airpath)
                {
                    showpath();
                }
            }
        }




        pathbranch selectbranch = null;
        int minpath = 1000;
        foreach (var item2 in allbranch)
        {
            if (item2.finish && item2.waypoint.Count < minpath)
            {
                minpath = item2.waypoint.Count;
                selectbranch = item2;
                point.Clear();
                if(useturnvanguard)
                {
                    point.Add(turnvanguard);
                }
                point.AddRange(selectbranch.waypoint);
                
            }
        }
        point.Add(sattelitepoint);
        showpath();
        complete = true;

    }

    public void showpath()
    {


        LineRenderer line = gameObject.GetComponent<LineRenderer>();
        line.positionCount = 0;
        line.SetVertexCount(point.Count+2);
        Vector3[] allposition = new Vector3[point.Count+2];
        Vector3 vanguad = new Vector3(vanguardpoint.position.x, point[0].y, vanguardpoint.position.z);
        allposition[0] = vanguad;
        for (int i = 0; i < point.Count; i++)
        {
            allposition[i+1] = point[i];
        }
        int finalindex = allposition.Length-1;
        allposition[finalindex] = sattelitepos;
        line.SetWidth(2,2);
        line.SetPositions(allposition);

    }

    public void generategroundpath(node node,pathbranch current)
    {
        if (node == null)
        {
            Debug.Log("node null");
            return;
        }
        
        List<node> neighbor = new List<node>();
        foreach (var item in map.map)
        {
            if (node == item)
            {
                neighbor = item.neighbor;
            }
        }
        float mindis = 10000000;
        Vector3 select = new Vector3(0, 0, 0);
        node selectnode = null;

        //select last node
        foreach (var item in neighbor)
        {
            if (checker(item.Placer) <= 0 && item.select == false)
            {
                select = item.position;
                selectnode = item;
                item.select = true;
            }
        }

        //select many node
        if (selectnode == null)
        {
            foreach (var item in neighbor)
            {
                {
                    //find min score for all neighbor
                    mindis = findminscore(item, mindis);

                    foreach (var item2 in allscore)
                    {
                        if (item.position == item2.position && item.havebase == false && item.select == false)
                        {
                            if (mindis == item2.finaldistant)
                            {
                                if(selectnode!=null)
                                {
                                    selectnode.select = false;
                                }

                                select = item2.position;
                                selectnode = item;
                                if(item.position!=final)
                                {
                                    item.select = true;
                                }
                                    
                                
                            }
                        }
                    }
                }
            }
        }

        current.waypoint.Add(select);
        if (select != final && selectnode != null)
        {
            generategroundpath(selectnode,current);
        }
        else if(select == final && selectnode != null)
        {
            current.finish = true;
        }
        else
        {
            /*current.waypoint.RemoveAt(current.waypoint.Count-1);

            foreach (var item in current.waypoint)
            {
                foreach (var item2 in map.map)
                {
                    if(item2.position==item)
                    {
                        selectnode = item2;
                    }
                }
            }
            Debug.Log(selectnode.name);*/
            //generategroundpath(selectnode, current);
        }
    }

    public void setscore()
    {
        foreach (var item in allscore)
        {
            item.finaldistant = Vector3.Distance(item.position, final);
        }
    }

    public float findminscore(node basenode, float mindis)
    {

        foreach (var item2 in allscore)
        {

            if (basenode.position == item2.position && basenode.havebase == false && basenode.select == false)
            {

                if (mindis > item2.finaldistant)
                {
                    mindis = item2.finaldistant;
                }
            }
        }
        
        return mindis;

    }

    public void newbranch(pathbranch current,string name)
    {
        allbranch.Add(new pathbranch(name));
        
    }


}
