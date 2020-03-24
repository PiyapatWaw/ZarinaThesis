using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class stagecontrol : MonoBehaviour
{
    [Header("start resource")]
    [SerializeField]
    private int _money;
    [SerializeField]
    private int _base;
    



    public static int phase;
    [Header("utility")]
    public float standbytime;
    public int maxwave;
    public Grid map;
    public playsceneUI ui;
    public wave[] allwave;
    public Transform spawnzone;
    public float radarmaxhp;
    public float radarhp;
    public GameObject radar;
    public GameObject radarbroke;
    public GameObject radarexplode;
    public GameObject radarbeam;
    public GameObject radarpanel;
    public Animator Radar_Animator;
    public Transform radarbeamposition;
    public int timeperwave;
    public int bonusbase;
    public int bonusresource;
    public Camera endcamera;
    public Camera endcamera2;
    public GameObject fade;
    
    

    public static bool ready;
    private int enemyremaining;
    private int currentwave;
    private int uicount;
    private bool uitransit = false;

    [Header("ui")]
    public Text text_wave;
    public Text text_standbytime;
    public Text text_base;
    public Text text_money;
    public Image Radarhpbar;
    public GameObject win;
    public GameObject lose;
    public GameObject enemybar;
    public Image[] enemyimg;
    public Text[] detailbar;

    private bool[] phasecomplete = new bool[2];

    public Image startwave;
    public Image duringwave;
    public Image endwave;

    [Header("enemy")]
    public List<GameObject> allenemy = new List<GameObject>();
    private bool spawncomplete;
    private bool deploycomplete;
    private float hpmultiple;

    [Header("waypoint")]
    public List<GameObject> allwaypoint = new List<GameObject>();

    private bool hplow; 
    private bool resaltphase = false;
    private bool showonewaypoint = false;
    private bool showallwaypoint = false;
    private int showindex = 0;
    private float showonetime;
    private float showalltime;
    // Use this for initialization
    void Start ()
    {
        Time.timeScale = 1;
        hpmultiple = 0.25f;
        phase = 0;
        ready = false;
        phasecomplete[0] = false;
        phasecomplete[1] = false;
        enemyremaining = 1;
        currentwave = 1;
        spawncomplete = false;
        deploycomplete = false;
        radarhp = 1;
        showindex = 0;
        showonetime = 1f;
        showonewaypoint = true;
        showalltime = 0;
        hplow = false;
    }

    
    // Update is called once per frame
    void Update ()
    {
        
        text_base.text = _base.ToString();
        text_money.text = _money.ToString();

        if(hplow==false&&phase!=0)
        {
            if (radarhp / radarmaxhp <= 0.5)
            {
                hplow = true;
                radar.SetActive(false);
                radarbroke.SetActive(true);
            }
        }
        

        if (currentwave<=maxwave)
        {
            switch (phase)
            {
                case 0://initial phase
                    {

                        GameObject[] allbutton = GameObject.FindGameObjectsWithTag("button");
                        foreach (var item in allbutton)
                        {
                            item.GetComponent<Button>().interactable = false;
                        }

                        if (radarhp<radarmaxhp)
                        {
                            float plus = radarmaxhp / 3;
                            radarhp += plus*Time.deltaTime;
                        }
                        if (radarhp >= radarmaxhp)
                        {
                            radarhp = radarmaxhp;
                        }
                        if(showonewaypoint==true&&showallwaypoint==false)
                        {
                            showonetime -= Time.deltaTime;
                            if(showonetime<=0)
                            {
                                if(showindex<allwaypoint.Count)
                                {
                                    if(showindex>0)
                                        allwaypoint[showindex - 1].SetActive(false);
                                    allwaypoint[showindex].SetActive(true);
                                    showonetime = 2.5f;
                                    showindex += 1;
                                }
                                else
                                {
                                    foreach (var item in allwaypoint)
                                    {
                                        item.SetActive(true);
                                    }
                                    showallwaypoint = true;
                                }
                                
                            }
                        }
                        else if(showonewaypoint == true && showallwaypoint == true)
                        {
                            showalltime += Time.deltaTime;
                        }
                        Radarhpbar.fillAmount = radarhp/radarmaxhp;
                    }
                    break;

                case 1://standby phase
                    {
                        GameObject[] allbutton = GameObject.FindGameObjectsWithTag("button");
                        foreach (var item in allbutton)
                        {
                            item.GetComponent<Button>().interactable = true;
                        }
                        Radarhpbar.fillAmount = radarhp / radarmaxhp;
                        text_wave.text = "Wave " + (currentwave.ToString()) + "/" + (maxwave.ToString());
                        text_standbytime.text = Convert.ToInt32(standbytime).ToString();
                        standbytime -= Time.deltaTime;


                        
                        if (spawncomplete == false)
                            spawnenemy();
                        if (allwave[currentwave - 1].spawnmanager.enemydata.Length <= 8)
                        {
                            enemybar.SetActive(true);
                            for (int j = 0; j < allwave[currentwave - 1].spawnmanager.enemydata.Length; j++)
                            {
                                enemyimg[j].gameObject.SetActive(true);
                                detailbar[j].gameObject.SetActive(true);
                                enemyimg[j].sprite = allwave[currentwave - 1].spawnmanager.enemydata[j].enemyimage;
                                string count = allwave[currentwave - 1].spawnmanager.enemydata[j].enemycount.ToString();
                                detailbar[j].text = "x " + count;
                            }
                        }
                    }  
                    break;
                case 2://battle phase
                    {
                        Radarhpbar.fillAmount = radarhp / radarmaxhp;
                        if (deploycomplete == false)
                            StartCoroutine("movetofrontline");
                        decressenemy();
                    }
                    break;
                case 3://end phase
                    {
                        GameObject[] allwaypoint = GameObject.FindGameObjectsWithTag("waypoint");
                        for (int i = 0; i < enemyimg.Length; i++)
                        {
                            enemyimg[i].gameObject.SetActive(false);
                            detailbar[i].gameObject.SetActive(false);
                        }
                        foreach (var item in allwaypoint)
                        {
                            item.SetActive(false);
                        }
                        currentwave++;
                        _money += bonusresource;
                        _base += bonusbase;
                        phase = 1;
                        uicount = 0;
                        standbytime = timeperwave;
                        defaltphasecomplete();
                        spawncomplete = false;
                        deploycomplete = false;
                        ready = false;
                        startactive();
                        Invoke("startdisactive", 1);
                    }
                    break;
                case 4://over phase
                    {
                        // Time.timeScale = 0;
                        fade.SetActive(true);
                        radarpanel.gameObject.SetActive(false);
                        for (int i = 0; i < gameObject.transform.childCount; i++)
                        {
                            gameObject.transform.GetChild(i).gameObject.SetActive(false);
                        }
                        if (radarhp <= 0)
                        {
                            Invoke("losestate",1f);
                            endcamera.gameObject.SetActive(true);
                            phase = 5;
                            
                        }
                        else
                        {
                            Invoke("winstate", 1f);
                            endcamera2.gameObject.SetActive(true);
                            phase = 5;
                        }
                        resaltphase = true;
                    }
                    break;
                case 5://result phase
                    break;
            }
            if(showalltime>=3&&phase==0)
            {
                Radar_Animator.Play("RunAnim", -1, 0f);
                Radar_Animator.SetBool("RunAnim", false);
                foreach (var item in allwaypoint)
                {
                    item.SetActive(false);
                }
                startactive();
                Invoke("startdisactive", 1);
                phase = 1;

            }
            if ((radarhp <= 0||(currentwave==maxwave&&enemyremaining<=0&&spawncomplete==true))&&resaltphase==false)
            {
                phase = 4;
            }
            if ((standbytime <= 0 || ready) && phasecomplete[0] == false&&phase==1)
            {
                endactive();
                Invoke("enddisactive",0.8f);
                foreach (var item in map.map)
                {
                    if(item.Placer!=null)
                    {
                        if (item.Placer.GetComponent<placer>().Mybase != null)
                        {
                            Destroy(item.Placer.GetComponent<placer>().Mybase);
                        }
                    }
                    
                }
                ui.allplacer.SetActive(false);
                ui.selectnull();
                phase = 2;
                phasecomplete[0] = true;
            }
            else if (enemyremaining <= 0 &&phase==2)
            {
                phase = 3;
                phasecomplete[1] = true;
            }
            
        }
        
	}

    public void losestate()
    {

        GameObject radarexp = Instantiate(radarexplode);
        Vector3 position = new Vector3(radar.transform.position.x, radar.transform.position.y+30, radar.transform.position.z);
        radarexp.transform.position = position;
        radarbroke.SetActive(false);
        Invoke("loseresult",2);
    }

    public void loseresult()
    {
        lose.SetActive(true);
    }

    public void winstate()
    {
        GameObject beam = Instantiate(radarbeam);
        beam.transform.position = radarbeamposition.position;
        Invoke("winresult", 2);
    }

    public void winresult()
    {
        win.SetActive(true);
    }

    public int getphase()
    {
        return phase;
    }

    public void increasemoney(int value)
    {
        _money += value;
    }
    public void increasebase(int value)
    {
        _base += value;
    }

    public void decreasemoney(int value)
    {
        _money -= value;
    }
    public void decreasebase(int value)
    {
        _base -= value;
    }

    public int getbase()
    {
        return _base;
    }

    public int getmoney()
    {
        return _money;
    }

    public int getwave()
    {
        return currentwave;
    }

    public void playerready()
    {
        ready = true;
        standbytime = 0;
    }

    public void enddisactive()
    {
        endwave.gameObject.SetActive(false);
    }

    public void startdisactive()
    {
        startwave.gameObject.SetActive(false);
        duringwave.gameObject.SetActive(true);
        text_standbytime.gameObject.SetActive(true);
    }
    public void endactive()
    {
        text_standbytime.gameObject.SetActive(false);
        duringwave.gameObject.SetActive(false);
        endwave.gameObject.SetActive(true);
    }

    public void startactive()
    {
        startwave.gameObject.SetActive(true);
        duringwave.gameObject.SetActive(false);
    }

    public void spawnenemy()
    {
        System.Random rnd = new System.Random();
        for (int i = 0; i < allwave[currentwave - 1].spawnmanager.enemydata.Length; i++)
        {
            for (int j = 0; j < allwave[currentwave-1].spawnmanager.enemydata[i].enemycount; j++)
            {
                GameObject enemy_tmp = Instantiate(allwave[currentwave - 1].spawnmanager.enemydata[i].enemy);
                allwave[currentwave - 1].spawnmanager.enemydata[i].Waypoint.gameObject.SetActive(true);
                enemy_tmp.GetComponent<enemy>().mywaypoint = allwave[currentwave - 1].spawnmanager.enemydata[i].Waypoint;
                float hpbonus = enemy_tmp.GetComponent<enemy>().hp * (currentwave*hpmultiple);
                enemy_tmp.GetComponent<enemy>().hp += Convert.ToInt32(hpbonus);
                enemy_tmp.GetComponent<enemy>().resource *= currentwave;
                Vector3 spawnposition;
                int x, y, z;
                x = rnd.Next(-500,500);
                z = rnd.Next(-500, 500);
                y = 5;
                spawnposition = spawnzone.position + new Vector3(z,y,z);
                enemy_tmp.transform.position = spawnposition;
            }
        }
        spawncomplete = true;
        findallenemy();
        Debug.Log("spawn" + currentwave);
    }

    public void decressenemy()
    {
        enemyremaining = allenemy.Count;
    }

    public IEnumerator movetofrontline()
    {
        
        for (int i = 0; i < allenemy.Count; i++)
        {
            if(allenemy[i].GetComponent<enemy>().readytodeploy==false)
            {
                waypoint myenemywaypoint = allenemy[i].GetComponent<enemy>().mywaypoint;
                float x, z;
                System.Random rnd = new System.Random();
                x = rnd.Next(myenemywaypoint.MinvanguardX, myenemywaypoint.MaxvanguardX);
                z = rnd.Next(myenemywaypoint.MinvanguardY, myenemywaypoint.MaxvanguardY);
                Vector3 vanguardpoint =  allenemy[i].GetComponent<enemy>().mywaypoint.vanguardpoint.position;
                allenemy[i].transform.position = vanguardpoint + new Vector3(x,0,z);
                allenemy[i].transform.position = new Vector3(allenemy[i].transform.position.x, allenemy[i].GetComponent<enemy>().Y, allenemy[i].transform.position.z);
                allenemy[i].GetComponent<enemy>().readytodeploy = true;
                allenemy[i].transform.SetParent(null);
                allenemy[i].GetComponent<BoxCollider>().enabled = true;
                yield return new WaitForSeconds(2.0f);
            }
            
        }
        deploycomplete = true;

    }


    public void defaltphasecomplete()
    {
        phasecomplete[0] = false;
        phasecomplete[1] = false;
    }

    public void findallenemy()
    {
        GameObject[] tmp = GameObject.FindGameObjectsWithTag("enemy");
        allenemy.Clear();
        foreach (GameObject item in tmp)
        {
            allenemy.Add(item);
        }
        enemyremaining = allenemy.Count;
    }
}
