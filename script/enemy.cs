using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour
{
    public waypoint mywaypoint;
    public bool readytodeploy;
    private int thispoint = 0;
    private int pointcount = 1;
    private stagecontrol stage;
    public int hp = 100;
    public int damage =0;
    public int speed=0;
    public int armor=0;
    public int barrier=0;
    public bool attack = false;
    public int resource;
    public float Y;
    public GameObject explosion;
    private bool isdie = false;
    public GameObject barrierdamage;

    public Material shade2,shade3;
    public GameObject[] allpartchange; 


    void Start()
    {
        readytodeploy = false;
        stage = GameObject.Find("Canvas").GetComponent<stagecontrol>();
        GetComponent<BoxCollider>().enabled = false;


        int a = stage.maxwave/3;
        Debug.Log(a);
        if(stage.getwave() > a && stage.getwave() <= a * 2)
        {
            foreach (var item in allpartchange)
            {
                item.GetComponent<Renderer>().material = shade2;
            }
        }
        else if(stage.getwave() > a && stage.getwave() >= a * 2)
        {
            foreach (var item in allpartchange)
            {
                item.GetComponent<Renderer>().material = shade3;
            }
        }
        


    }

    // Update is called once per frame
    void Update()
    {
        if (stagecontrol.phase == 2 && readytodeploy && isdie ==false)
        {
            Vector3 target = new Vector3(mywaypoint.point[thispoint].x, Y, mywaypoint.point[thispoint].z);
            transform.LookAt(target);
            transform.Translate(Vector3.forward * Time.deltaTime * speed);

            float disx = Mathf.Abs(transform.position.x - mywaypoint.point[thispoint].x);
            float disz = Mathf.Abs(transform.position.z - mywaypoint.point[thispoint].z);

            if (disx < 0.5f && disz < 0.5f)
            {
                changewaypoint();
            }
        }

    }


    public void incressresourse()
    {
        stage.increasemoney(resource);
    }


    public void calculatedamage(int damage,bool antibarrier)
    {
        if(barrier>0&& antibarrier == false)
        {
            barrier -= damage;
            barrierdamage.SetActive(true);
            Invoke("closebarrierdamage",2);
        }
        else if (barrier > 0 && antibarrier == false)
        {
            damage = Mathf.Abs(damage * 2)+armor;
            barrier -= damage;
            hp -= damage;
        }
        else if(armor>0)
        {
            damage -= Mathf.Abs(damage * (armor/100));
            hp -= damage;
        }
        else
        {
            hp -= damage;
        }
        
    }


    public void destroyme()
    {
        if(isdie==false)
            StartCoroutine(effect());
    }



    public void changewaypoint()
    {
        if (thispoint < mywaypoint.point.Count - 1)
            thispoint++;
        else
        {
            if (attack == false&&isdie==false)
            {
                stage.radarhp -= damage;
                attack = true;
                stage.allenemy.Remove(gameObject);
                destroyme();
            }

        }
    }

    public IEnumerator effect()
    {
        isdie = true;
        GameObject exp = Instantiate(explosion);
        exp.transform.position = gameObject.transform.position;
        exp.transform.eulerAngles = gameObject.transform.eulerAngles;
        Destroy(gameObject);
        yield return new WaitForSeconds(1f);
    }

    public void closebarrierdamage()
    {
        barrierdamage.SetActive(false);
    }

}
