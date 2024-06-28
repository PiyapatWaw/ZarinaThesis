using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class tutorial : MonoBehaviour
{
    public stagecontrol stage;
    public int phase;
    public List<Button> Allbutton = new List<Button>();
    public Text tutorialtxt;
    public GameObject tutorialpanel;
    public List<GameObject> standbytime;
    public GameObject custompanel;
    public GameObject weaponpanel;
    private float blinktime;
    private bool blinked;
    private float Highlightime = 1f;
    private Button select = null;
    // Use this for initialization
    void Start()
    {
        phase = 0;
        blinktime = 0.5f;
        blinked = false;

    }
    private bool phase5resource = false;
    private bool phase7resource = false;
    private int phase7missile;
    private int phase10tower;
    private bool phase17resource = false;
    // Update is called once per frame
    void Update()
    {
        if (stage.getphase() == 1 && phase == 0)
        {
            phase = 1;
        }
        if (phase == 1)
        {
            if (stage.getbase() == 0)
            {
                normalhiglight(select);
                phase = 2;
            }
        }
        else if (phase == 2)
        {
            if (stage.getmoney() == 0)
            {
                normalhiglight(select);
                phase = 3;
            }
        }
        else if (phase == 3)
        {
            if (stage.getphase() == 2)
            {
                normalhiglight(select);
                phase = 4;
            }
        }
        else if (phase == 4)
        {
            if (stage.getphase() == 3)
            {
                if (phase5resource == false)
                {
                    phase5resource = true;
                    stage.increasemoney(250);
                }
                phase = 5;
            }
        }
        else if (phase == 5)
        {
            if (stage.getphase() == 2)
            {
                phase = 6;
            }
        }
        else if (phase == 6)
        {
            if (stage.getphase() == 3)
            {
                if (phase7resource == false)
                {
                    phase7resource = true;
                    phase7missile = 0;
                    stage.increasemoney(200);
                    GameObject[] alltower = GameObject.FindGameObjectsWithTag("tower");
                    foreach (var item in alltower)
                    {
                        if (item.name == "Missile(Clone)")
                        {
                            phase7missile++;
                        }
                    }
                }
                phase = 7;
            }
        }
        else if (phase == 7)
        {
            int allmissile = 0;
            GameObject[] alltower = GameObject.FindGameObjectsWithTag("tower");
            foreach (var item in alltower)
            {
                if (item.name == "Missile(Clone)")
                {
                    allmissile++;
                }
            }
            if (allmissile - phase7missile == 2)
            {
                normalhiglight(select);
                phase = 8;
            }
        }
        else if (phase == 8)
        {
            if (stage.getphase() == 2)
            {
                normalhiglight(select);
                phase = 9;
            }
        }
        else if (phase == 9)
        {
            if (stage.getphase() == 3)
            {
                phase = 10;
            }
        }
        else if (phase == 10)
        {
            GameObject[] towerbutton = GameObject.FindGameObjectsWithTag("button");
            GameObject[] alltower = GameObject.FindGameObjectsWithTag("tower");
            phase10tower = alltower.Length;
            foreach (var item in towerbutton)
            {
                if (item.name == "tower button" && item.active)
                {
                    select = item.GetComponent<Button>();
                    phase = 11;

                }
            }
        }
        else if (phase == 11)
        {
            GameObject[] alltower = GameObject.FindGameObjectsWithTag("tower");
            if (phase10tower > alltower.Length)
            {
                select = null;
                Highlightime = 1;
                phase = 12;
            }
        }
        else if (phase == 12)
        {
            if (custompanel.active)
            {
                normalhiglight(select);
                phase = 13;
            }
        }
        else if (phase == 13)
        {
            GameObject coreclone = GameObject.Find("core2_1(Clone)");
            if (coreclone != null)
            {
                normalhiglight(select);
                GameObject weaponbutton = GameObject.Find("Button weapon");
                select = weaponbutton.GetComponent<Button>();
                higlight(select);
                phase = 14;
            }
        }
        else if (phase == 14)
        {
            if (weaponpanel.active)
            {
                normalhiglight(select);
                phase = 15;
            }
        }
        else if (phase == 15)
        {
            GameObject alpha = GameObject.Find("tower alpha");
            int weaponcount = 0;
            foreach (GameObject item in alpha.GetComponent<tower>().Towerweapon)
            {
                if(item!=null)
                {
                    weaponcount++;
                }
            }
            if (weaponcount == alpha.GetComponent<tower>().Towerweapon.Length)
            {
                GameObject build = GameObject.Find("build");
                select = build.GetComponent<Button>();
                higlight(select);
                phase = 16;
            }
        }
        else if (phase == 16)
        {
            if (custompanel.active == false)
            {
                normalhiglight(select);
                phase = 17;
                if (phase17resource == false)
                {
                    phase17resource = true;
                    stage.increasemoney(300);
                }
            }
        }
        else if (phase == 17)
        {
            playsceneUI ui = GameObject.Find("Canvas").GetComponent<playsceneUI>();
            if(ui.Currenttower!=null)
            {
                if (ui.Currenttower.name == "tower alpha")
                {
                    normalhiglight(select);
                    phase = 18;
                }
            }
            
        }
        else if (phase == 18)
        {
            GameObject custom = GameObject.Find("tower alpha(Clone)");
            if (custom != null)
            {
                phase = 19;
            }
        }
        else if (phase == 19)
        {
            if (stage.getphase() == 2)
            {
                phase = 20;
            }
        }


    }



    void LateUpdate()
    {

        Allbutton.Clear();
        GameObject[] allbutton = GameObject.FindGameObjectsWithTag("button");
        foreach (var item in allbutton)
        {
            this.Allbutton.Add(item.GetComponent<Button>());
        }
        switch (phase)
        {
            case 1:
                stage.standbytime = 60;
                foreach (var item in this.Allbutton)
                {
                    if (item.gameObject.name == "base" && stage.getbase() > 0)
                    {
                        item.interactable = true;
                        select = item;
                    }
                    else
                    {
                        item.interactable = false;
                    }
                }
                foreach (var item in standbytime)
                {
                    item.SetActive(false);
                }
                tutorialpanel.SetActive(true);
                tutorialtxt.gameObject.SetActive(true);
                tutorialtxt.text = "click base button and deploy 3 base on field";
                if (Highlightime == 1)
                {
                    higlight(select);
                }

                break;
            case 2:
                foreach (var item in this.Allbutton)
                {
                    if (item.gameObject.name == "tw1")
                    {
                        item.interactable = true;
                        select = item;
                    }
                    else
                    {
                        item.interactable = false;
                    }
                }
                if (Highlightime == 1)
                {
                    higlight(select);
                }
                tutorialtxt.text = "deploy 3 gatling tower on base";
                break;
            case 3:
                foreach (var item in allbutton)
                {
                    this.Allbutton.Add(item.GetComponent<Button>());
                }
                foreach (var item in this.Allbutton)
                {
                    if (item.gameObject.name == "ready")
                    {
                        item.interactable = true;
                        select = item;
                    }
                    else
                    {
                        item.interactable = false;
                    }
                }
                if (Highlightime == 1)
                {
                    higlight(select);
                }
                tutorialtxt.text = "click play button to battle phase";
                break;
            case 4:
                foreach (var item in standbytime)
                {
                    item.SetActive(false);
                }

                tutorialpanel.SetActive(false);
                tutorialtxt.text = "";
                tutorialtxt.gameObject.SetActive(false);
                break;
            case 5:
                stage.standbytime = 60;
                foreach (var item in standbytime)
                {
                    item.SetActive(false);
                }
                tutorialpanel.SetActive(true);
                tutorialtxt.gameObject.SetActive(true);
                tutorialtxt.text = "control enemy path with your base and deploy tower you want";
                break;
            case 6:
                tutorialpanel.SetActive(false);
                tutorialtxt.text = "";
                tutorialtxt.gameObject.SetActive(false);
                break;
            case 7:
                foreach (var item in standbytime)
                {
                    item.SetActive(false);
                }
                stage.standbytime = 60;
                tutorialpanel.SetActive(true);
                tutorialtxt.gameObject.SetActive(true);
                tutorialtxt.text = "deploy 2 missile tower on base to attack air enemy";
                foreach (var item in Allbutton)
                {
                    if (item.name == "tw3" || item.name == "base")
                    {
                        item.interactable = true;
                        if (item.name == "tw3")
                        {
                            select = item;
                        }
                    }
                    else
                    {
                        item.interactable = false;
                    }
                }
                if (Highlightime == 1)
                {
                    higlight(select);
                }
                break;
            case 8:
                tutorialtxt.text = "click play button";
                foreach (var item in this.Allbutton)
                {
                    if (item.gameObject.name == "ready")
                    {
                        item.interactable = true;
                        select = item;
                    }
                    else
                    {
                        item.interactable = false;
                    }
                }
                higlight(select);
                break;
            case 9:
                tutorialpanel.SetActive(false);
                tutorialtxt.text = "";
                tutorialtxt.gameObject.SetActive(false);
                break;
            case 10:
                foreach (var item in standbytime)
                {
                    item.SetActive(false);
                }
                foreach (var item in Allbutton)
                {
                    if(item.gameObject.name == "tower button")
                    {
                        item.interactable = true;
                    }
                    else
                    {
                        item.interactable = false;
                    }
                }
                stage.standbytime = 60;
                tutorialpanel.SetActive(true);
                tutorialtxt.text = "click one tower to open sell button";
                tutorialtxt.gameObject.SetActive(true);
                break;
            case 11:
                tutorialtxt.text = "click sell button to sell tower";
                stage.standbytime = 60;
                foreach (var item in Allbutton)
                {
                    if (item.name == "tower button")
                    {
                        item.interactable = true;
                        select = item;
                    }
                    else
                    {
                        item.interactable = false;
                    }
                }
                higlight(select);
                break;
            case 12:
                foreach (var item in standbytime)
                {
                    item.SetActive(false);
                }
                foreach (var item in Allbutton)
                {
                    if (item.name == "alpha")
                    {
                        item.interactable = true;
                        select = item;
                    }
                    else
                    {
                        item.interactable = false;
                    }
                }
                higlight(select);
                stage.standbytime = 60;
                tutorialpanel.SetActive(true);
                tutorialtxt.text = "click custom button to open custom tower panel";
                tutorialtxt.gameObject.SetActive(true);
                break;
            case 13:
                foreach (var item in standbytime)
                {
                    item.SetActive(false);
                }
                stage.standbytime = 60;
                tutorialtxt.text = "click Core - 2 Arm to create Core - 2 Arm part";
                break;
            case 14:
                foreach (var item in standbytime)
                {
                    item.SetActive(false);
                }

                stage.standbytime = 60;
                tutorialtxt.text = "click Weapon button to open Wepon panel";
                break;
            case 15:
                foreach (var item in standbytime)
                {
                    item.SetActive(false);
                }
                stage.standbytime = 60;
                tutorialtxt.text = "select 2 weapon to build your tower";
                break;
            case 16:
                foreach (var item in standbytime)
                {
                    item.SetActive(false);
                }
                stage.standbytime = 60;
                tutorialtxt.text = "click build button";
                break;
            case 17:
                foreach (var item in standbytime)
                {
                    item.SetActive(false);
                }
                foreach (var item in Allbutton)
                {
                    if (item.name == "alpha" || item.name == "base")
                    {
                        item.interactable = true;
                        if(item.name=="alpha")
                        {
                            select = item;
                        }
                    }
                    else
                    {
                        item.interactable = false;
                    }
                }
                higlight(select);
                Debug.Log(select);
                stage.standbytime = 60;
                tutorialtxt.text = "click custom button";
                break;
            case 18:
                tutorialtxt.text = "deploy cuttom tower on base";
                break;
            case 19:
                foreach (var item in Allbutton)
                {
                    item.interactable = true;
                }
                    tutorialtxt.text = "defense final wave";
                break;
            case 20:
                foreach (var item in standbytime)
                {
                    item.SetActive(false);
                }
                tutorialpanel.SetActive(false);
                tutorialtxt.gameObject.SetActive(false);
                tutorialtxt.text = "";
                break;


        }
    }

    public void normalhiglight(Button select)
    {
        select.animator.SetBool("Normal", true);
        select = null;
        Highlightime = 1;
    }

    public void higlight(Button select)
    {
        if (Highlightime == 1)
        {
            select.GetComponent<Animator>().SetBool("Highlighted", true);
            Highlightime--;
        }
        

    }

}
