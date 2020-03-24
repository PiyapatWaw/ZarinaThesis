using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class custom : MonoBehaviour
{
    public static bool buildcomplete;
    public buttonactive customactive;
    public static GameObject mytower;
    public GameObject panelcore;
    public GameObject panelweapon;
    private static GameObject core_tmp;
    private static GameObject weapon_tmp;
    private static GameObject core_hold;
    private static GameObject weapon_hold;
    private GameObject currentweapon;
    private GameObject currentholdweapon;
    private int weapon_hold_slot;
    private tower currenttower;
    public tower holdscript;
    public GameObject holdtower;
    public GameObject panelposition;
    public Button buttonweapon;
    public Button[] buttonposition;
    public Button buildbutton;
    public Button custombutton;
    public Sprite buildcomp;
    public Image damagebar,rotatebar,fireratebar;
    private GameObject holdweapon;
    private bool canbuild = false;

    public GameObject Holdtower
    {
        get
        {
            return holdtower;
        }

        set
        {
            holdtower = value;
        }
    }


    // Use this for initialization
    void Start() {
        buildcomplete = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (core_tmp != null)
        {
            buttonweapon.interactable = true;
        }
        else
        {
            buttonweapon.interactable = false;

        }
        buildbutton.interactable = canbuild;
    }

    public void checkpositionactive()
    {
        foreach (Button item in buttonposition)
        {
            if (item.name == "Button top")
            {
                for (int i = 0; i < core_tmp.transform.childCount; i++)
                {
                    if (core_tmp.transform.GetChild(i).gameObject.name == "weapon top")
                    {
                        item.interactable = true;
                        break;
                    }
                    else
                    {
                        item.interactable = false;
                    }
                }
            }
            else if (item.name == "Button left")
            {
                for (int i = 0; i < core_tmp.transform.childCount; i++)
                {
                    if (core_tmp.transform.GetChild(i).gameObject.name == "weapon left")
                    {
                        item.interactable = true;
                        break;
                    }
                    else
                    {
                        item.interactable = false;
                    }
                }
            }
            else if (item.name == "Button right")
            {
                for (int i = 0; i < core_tmp.transform.childCount; i++)
                {
                    if (core_tmp.transform.GetChild(i).gameObject.name == "weapon right")
                    {
                        item.interactable = true;
                        break;
                    }
                    else
                    {
                        item.interactable = false;
                    }
                }
            }
        }
    }

    public void select_core(GameObject myobject)
    {
        currenttower = mytower.GetComponent<tower>();
        Vector3 position = new Vector3(0, 0, 0);
        position = GameObject.Find("corehere").transform.position;//mytower.transform.position;

        if (currenttower.Towercore != null)
        {
            Destroy(currenttower.Towercore);
            currenttower.Towerweapon=new GameObject[0];
            Destroy(holdscript.Towercore);
            holdscript.Towerweapon = new GameObject[0];
        }
        GameObject mycore = Instantiate(myobject);
        mycore.transform.position = new Vector3(position.x, position.y, position.z);
        core_tmp = mycore;
        currenttower.Towercore = mycore;

        int slot = 0;
        for (int i = 0; i < mycore.transform.childCount; i++)
        {
            if (mycore.transform.GetChild(i).tag == "weapon position")
            {
                slot++;
            }
        }
        currenttower.Weaponslot = new bool[slot];
        currenttower.Weaponposition = new GameObject[slot];
        currenttower.Towerweapon = new GameObject[slot];


        for (int i = 0; i < slot;)
        {
            for (int j = 0; j < mycore.transform.childCount; j++)
            {
                if (mycore.transform.GetChild(j).tag == "weapon position")
                {
                    currenttower.Weaponslot[i] = false;
                    currenttower.Weaponposition[i] = mycore.transform.GetChild(j).gameObject;
                    i++;
                }
            }
        }


        float speed = currenttower.towercore.GetComponent<core>().rotatespeed;
        rotatebar.fillAmount = speed / 10;
        canbuild = false;

    }

    public void select_weapon(GameObject myobject)
    {
        weapon_tmp = myobject;
        if (currenttower.Weaponposition.Length == 1)
        {
            if (currenttower.Weaponslot[0] == false)
            {
                createweapon(weapon_tmp, currenttower.Weaponposition[0].transform, core_tmp);
                currenttower.Weaponslot[0] = true;
            }
            else if (currenttower.Weaponslot[0] == true)
            {
                Destroy(currenttower.Towerweapon[0]);
                Destroy(holdscript.Towerweapon[0]);
                currenttower.Weaponslot[0] = true;
                createweapon(weapon_tmp, currenttower.Weaponposition[0].transform, core_tmp);
                //holdscript.Towerweapon.RemoveAt(0);
                holdscript.Weaponslot[0] = true;
            }
        }
        else if (currenttower.Weaponposition.Length > 1)
        {
            panelweapon.SetActive(false);
            panelposition.SetActive(true);
            checkpositionactive();
        }

    }

    public void selecttop()
    {
        for (int i = 0; i < currenttower.Weaponposition.Length; i++)
        {
            
            if (currenttower.Weaponposition[i].name == "weapon top")
            {
                Debug.Log("top "+i);


                //createweapon(weapon_tmp, currenttower.Weaponposition[i].transform, core_tmp);

                if (currenttower.Weaponslot[i] == false)
                {
                    weapon_hold_slot = i;
                    createweapon(weapon_tmp, currenttower.Weaponposition[i].transform, core_tmp,i);
                    currenttower.Weaponslot[i] = true;
                }
                else if (currenttower.Weaponslot[i] == true)
                {
                    GameObject target = currenttower.Towerweapon[i];
                    Destroy(currenttower.Towerweapon[i]);
                    Destroy(holdscript.Towerweapon[i]);
                    weapon_hold_slot = i;
                    currenttower.Weaponslot[i] = true;
                    createweapon(weapon_tmp, currenttower.Weaponposition[i].transform, core_tmp,i);
                }
                panelweapon.SetActive(true);
                break;
            }
        }
    }

    public void selectleft()
    {
        for (int i = 0; i < currenttower.Weaponposition.Length; i++)
        {
            if (currenttower.Weaponposition[i].name == "weapon left")
            {
                Debug.Log("left " + i);
                if (currenttower.Weaponslot[i] == false)
                {
                    weapon_hold_slot = i;
                    createweapon(weapon_tmp, currenttower.Weaponposition[i].transform, core_tmp,i);
                    currenttower.Weaponslot[i] = true;
                }
                else if (currenttower.Weaponslot[i] == true)
                {
                    GameObject target = currenttower.Towerweapon[i];
                    Destroy(currenttower.Towerweapon[i]);
                    Destroy(holdscript.Towerweapon[i]);
                    weapon_hold_slot = i;
                    currenttower.Weaponslot[i] = true;
                    createweapon(weapon_tmp, currenttower.Weaponposition[i].transform, core_tmp,i);
                }
                panelweapon.SetActive(true);
                break;
            }
        }
    }

    public void selectright()
    {
        for (int i = 0; i < currenttower.Weaponposition.Length; i++)
        {
            if (currenttower.Weaponposition[i].name == "weapon right")
            {
                Debug.Log("right " + i);
                if (currenttower.Weaponslot[i] == false)
                {
                    weapon_hold_slot = i;
                    createweapon(weapon_tmp, currenttower.Weaponposition[i].transform, core_tmp,i);
                    currenttower.Weaponslot[i] = true;
                }
                else if (currenttower.Weaponslot[i] == true)
                {
                    GameObject target = currenttower.Towerweapon[i];
                    Destroy(currenttower.Towerweapon[i]);
                    Destroy(holdscript.Towerweapon[i]);
                    weapon_hold_slot = i;
                    currenttower.Weaponslot[i] = true;
                    createweapon(weapon_tmp, currenttower.Weaponposition[i].transform, core_tmp,i);
                }
                panelweapon.SetActive(true);
                break;
            }
        }
    }

    public void createweapon(GameObject weaponobject, Transform slottranform, GameObject core,int index=0)
    {
        currenttower.haveweapon = true;
        GameObject myweapon = Instantiate(weaponobject);
        currenttower.Towerweapon[index] = myweapon;
        myweapon.transform.position = slottranform.transform.position;
        Transform eulerAngles = slottranform.transform;
        myweapon.transform.eulerAngles += eulerAngles.eulerAngles;
        myweapon.transform.SetParent(core.transform);
        currentweapon = myweapon;
        createholdweapon(index);
        Invoke("movehodweapon", 0.2f);

        if(myweapon.GetComponent<weapon>().airfire==true)
        {
            core_tmp.GetComponent<core>().airaim = true;
        }


        float damage = 0;
        float firerate = 0;
        int weaponcount = 0;
        foreach (var item in currenttower.Towerweapon)
        {
            if(item!=null)
            {
                damage += item.GetComponent<weapon>().bullet.GetComponent<bullet>().damage;
                firerate += item.GetComponent<weapon>().delayfire;
                weaponcount++;
            }
        }
        damage = damage / (100 * weaponcount);
        firerate = (0.5f*weaponcount) -  ((firerate*60 ) / (100 * weaponcount) /10*weaponcount);
        damagebar.fillAmount = damage;
        fireratebar.fillAmount = firerate;
        canbuild = true;
    }

    public void corebutton()
    {
        panelcore.SetActive(true);
        panelweapon.SetActive(false);
    }

    public void weaponbutton()
    {
        panelcore.SetActive(false);
        panelweapon.SetActive(true);
    }

    public void build()
    {
        Vector3 parentpos = new Vector3(0, 0, 0);

        if(canbuild==true)
        {
            for (int i = 0; i < core_tmp.transform.childCount; i++)
            {
                if (core_tmp.transform.GetChild(i).name == "parent position")
                {
                    parentpos = core_tmp.transform.GetChild(i).transform.position;
                    break;
                }
            }
            core_hold.SetActive(true);

            for (int i = 0; i < core_hold.transform.childCount; i++)
            {
                if (core_hold.transform.GetChild(i).gameObject.name == "select")
                {
                    holdscript.selectcircle = core_hold.transform.GetChild(i).gameObject;
                }
            }
            for (int i = 0; i < core_tmp.transform.childCount; i++)
            {
                if (core_tmp.transform.GetChild(i).gameObject.name == "select")
                {
                    currenttower.GetComponent<tower>().selectcircle = core_tmp.transform.GetChild(i).gameObject;
                }
            }

            currenttower.changeprice();
            mytower.transform.position = parentpos;
            core_tmp.transform.SetParent(mytower.transform);
            core_hold.transform.SetParent(Holdtower.transform);
            core_tmp = null;
            mytower = null;
            core_hold = null;
            weapon_hold = null;
            corebutton();
            gameObject.SetActive(false);
            customactive.Custom = false;
            customactive.Tower = true;
            buildcomplete = true;

            custombutton.GetComponent<Image>().sprite = buildcomp;
        }

        


        
    }

    public void selectholdcore(GameObject select)
    {
        if (holdscript.Towercore != null)
        {
            Destroy(holdscript.Towercore);
            holdscript.Towerweapon = new GameObject[0] ;
        }

        GameObject holdcore = Instantiate(select);
        GameObject corepos = Holdtower.transform.GetChild(0).gameObject;

        Vector3 pos = GameObject.Find("corehere").transform.position;
        holdcore.transform.position = new Vector3(pos.x, pos.y, pos.z);

        int slot = 0;
        for (int i = 0; i < holdcore.transform.childCount; i++)
        {
            if (holdcore.transform.GetChild(i).tag == "weapon position")
            {
                slot++;
            }
        }
        holdscript.Weaponslot = new bool[slot];
        holdscript.Weaponposition = new GameObject[slot];
        holdscript.towerweapon = new GameObject[slot];
        holdscript.Towercore = holdcore;
        for (int i = 0; i < slot;)
        {
            for (int j = 0; j < holdcore.transform.childCount; j++)
            {
                if (holdcore.transform.GetChild(j).tag == "weapon position")
                {
                    holdscript.Weaponslot[i] = false;
                    holdscript.Weaponposition[i] = holdcore.transform.GetChild(j).gameObject;
                    i++;
                }
            }
        }
        core_hold = holdcore;
        core_hold.SetActive(false);
    }

    public void createholdweapon(int index)
    {
        //Debug.Log(holdweapon.name);
        GameObject hold = Instantiate(holdweapon);
        currentholdweapon = hold;
        hold.GetComponent<tower>().enabled = false;
        if (holdscript.Weaponslot[weapon_hold_slot])
        {
            holdscript.Towerweapon[weapon_hold_slot] = hold;

        }
        else
        {
            holdscript.Towerweapon[index] = hold;
            holdscript.Weaponslot[weapon_hold_slot] = true;
        }

        hold.transform.SetParent(core_hold.transform);
    }

    public void createholdweapon(GameObject weaponhold)
    {
        holdweapon = weaponhold;
    }

    public void movehodweapon()
    {
       /* Debug.Log("currentholdweapon : " + currentholdweapon);
        Debug.Log("currentweapon : " + currentweapon);*/
        currentholdweapon.transform.position = currentweapon.transform.position;
        currentholdweapon.transform.eulerAngles = currentweapon.transform.eulerAngles;
    }
}
