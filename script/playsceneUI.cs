using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class playsceneUI : MonoBehaviour
{
    public Camera maincam;
    private GameObject currenttower;
    private GameObject hovertower;
    public GameObject allplacer;
    public Grid mygrid;
    public stagecontrol stage;
    private int constructioncost;
    public GameObject custompanel;
    public GameObject pausepanel;
    private bool ispause = false;
    public GameObject loading;
    public Image loadingbar;
    public GameObject playbutton;
    AsyncOperation asyn;

    public GameObject Currenttower
    {
        get
        {
            return currenttower;
        }

        set
        {
            currenttower = value;
        }
    }

    public GameObject Hovertower
    {
        get
        {
            return hovertower;
        }

        set
        {
            hovertower = value;
        }
    }
    private void Start()
    {
        custompanel.SetActive(false);
        pausepanel.SetActive(false);
    }

    private void Update()
    {
        if(currenttower!= null)
        {
            if (Input.GetMouseButtonDown(0) && currenttower.tag == "base")
            {
                RaycastHit hitInfo;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                Debug.DrawRay(ray.origin, ray.direction * 10, Color.yellow);

                if (Physics.Raycast(ray, out hitInfo))
                {

                    bool found = false;
                    foreach (var item in mygrid.map)
                    {
                        if(item.name== hitInfo.collider.gameObject.name)
                        {
                            
                            Destroy(hitInfo.collider.gameObject.GetComponent<placer>().Mybase);
                            
                            item.Placer.SetActive(false);
                            item.havebase = true;
                            item.nodebase = place(item.position);
                            stage.decreasebase(1);
                            found = true;
                        }
                    }

                    if(found)
                    {
                        GameObject[] allwaypoint = GameObject.FindGameObjectsWithTag("waypoint");

                        foreach (var item2 in allwaypoint)
                        {
                            for (int i = 0; i < item2.GetComponent<waypoint>().point.Count; i++)
                            {
                                if (hitInfo.collider.gameObject.transform.position == item2.GetComponent<waypoint>().point[i])
                                {
                                    item2.GetComponent<waypoint>().stackpoint = i - 1;
                                }
                            }
                        }
                        foreach (var point in allwaypoint)
                        {
                            point.GetComponent<waypoint>().intothegraph();
                        }
                    }
                }
            }
            else if (Input.GetMouseButtonDown(0) && currenttower.tag == "tower")
            {
                RaycastHit hitInfo;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hitInfo))
                {
                    if(hitInfo.collider.gameObject.tag=="base")
                    {
                        if(hitInfo.collider.gameObject.GetComponent<mybase>().havetower==false)
                        {
                            Destroy(hitInfo.collider.gameObject.GetComponent<mybase>().Hovertower);
                            hitInfo.collider.gameObject.GetComponent<mybase>().havetower = true;
                            Vector3 tmposition = hitInfo.collider.gameObject.transform.GetChild(0).transform.position;
                            Vector3 useposition = new Vector3(tmposition.x, tmposition.y + 2, tmposition.z);
                            GameObject buildtower = place(useposition);
                            buildtower.GetComponent<tower>().mybase = hitInfo.collider.gameObject;
                            hitInfo.collider.gameObject.GetComponent<mybase>().tower = buildtower;
                            stage.decreasemoney(constructioncost);
                        }
                        
                    }
                    
                }
            }
        }
        else if(currenttower==null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hitInfo;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hitInfo))
                {
                    if (hitInfo.collider.gameObject.tag=="tower")
                    {
                        GameObject selectobject = hitInfo.collider.gameObject;
                        tower selecttower = selectobject.GetComponent<tower>();
                        selecttower.selectcircle.SetActive(!selecttower.selectcircle.active);
                        selecttower.towerui.SetActive(!selecttower.towerui.active);
                    }
                    if (hitInfo.collider.gameObject.tag == "base")
                    {
                        if(stage.getphase()==1)
                        {
                            GameObject selectobject = hitInfo.collider.gameObject;
                            mybase selectbase = selectobject.GetComponent<mybase>();
                            selectbase.ui.SetActive(!selectbase.ui.active);
                        }
                        
                    }
                }
            }
        }


        
    }

    public GameObject place(Vector3 clickPoint)
    {
        var finalPosition = clickPoint;
        GameObject clonetower = Instantiate(currenttower);
        clonetower.transform.position = finalPosition;
        selectnull();
        allplacer.SetActive(false);
        return clonetower;
    }

    public void selecthover(GameObject select)
    {
        hovertower = select;
    }

    public void deploytower(GameObject select)
    {
        allplacer.SetActive(false);
        select.GetComponent<tower>().changeprice();
        constructioncost = select.GetComponent<tower>().Price;
        if (stage.getmoney()>= constructioncost)
        {
            currenttower = select;
        }
        
    }

    public void deploybase(GameObject select)
    {
        if(stage.getphase() == 1&&stage.getbase()>0)
        {
            GameObject agrid = GameObject.Find("grid");
            Grid map = agrid.GetComponent<Grid>();
            GameObject[] allwaypoints = GameObject.FindGameObjectsWithTag("waypoint");
            waypoint[] waypoints = new waypoint[allwaypoints.Length];
            for (int i = 0; i < waypoints.Length; i++)
            {
                waypoints[i] = allwaypoints[i].GetComponent<waypoint>();
                waypoints[i].allbranch.Clear();
            }
            foreach (var item in map.map)
            {
                if(item.havebase==true)
                {
                    item.Placer.SetActive(false);
                }
                foreach (var item2 in waypoints)
                {
                    if(item2.start==item.position|| item2.final == item.position)
                    {
                        item.Placer.SetActive(false);
                    }
                }
            }
            foreach (var item in map.map)
            {
                foreach (var item2 in waypoints)
                {
                    if(item2.groundpath)
                    {
                        if (item2.checker(item.Placer) <= 0)
                        {
                            item.Placer.SetActive(false);
                        }
                    }
                    
                }
            }
            currenttower = select;
            allplacer.SetActive(true);
        }
        
    }

    public void selectnull()
    {
        currenttower = null;
        hovertower = null;
    }

    public void onofpanel(GameObject panel)
    {
        panel.SetActive(!panel.active);
    }

    public void customtower(GameObject target)
    {
        if(target.transform.childCount<=2)
        {
            custompanel.SetActive(true);
            target.SetActive(true);
            custom.mytower = target;
        }
        else
        {
            constructioncost = target.GetComponent<tower>().Price;
            if (stage.getmoney() >= constructioncost)
            {
                currenttower = target;
               
            }
        }
    }

    public void customholder(GameObject target)
    {
        if (target.transform.childCount >= 1)
        {
            hovertower = target;
        }
            
    }

    public void pause()
    {
        ispause = !ispause;
        if(ispause)
        {
            pausepanel.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            pausepanel.SetActive(false);
            Time.timeScale = 1;
        }
    }


    IEnumerator asynload(string name,bool usebutton=false)
    {
        loading.gameObject.SetActive(true);
        asyn = SceneManager.LoadSceneAsync(name);
        asyn.allowSceneActivation = false;
        while (!asyn.isDone)
        {
            loadingbar.fillAmount = asyn.progress;
            if (asyn.progress == 0.9f)
            {
                loadingbar.fillAmount = 1;
                loadingbar.gameObject.SetActive(false);
                if(usebutton==true)
                {
                    playbutton.SetActive(true);
                }
                else
                {
                    asyn.allowSceneActivation = true;
                }
                
            }
            yield return null;
        }

    }

    public void play()
    {
        asyn.allowSceneActivation = true;
    }

    public void resetmission(string name)
    {
        StartCoroutine(asynload(name, true));
    }
    public void exit()
    {
        Application.Quit();
    }
    public void retunmainmenu()
    {
        StartCoroutine(asynload("MainMenu"));
    }
    public void nextmission(string name)
    {
        StartCoroutine(asynload(name,true));
    }
}
