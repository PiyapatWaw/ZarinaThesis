using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class homing : MonoBehaviour
{
    public GameObject enemy;
    private float goup = 2f;
    public bool lockon;
	// Use this for initialization
	void Start ()
    {
		if(enemy.transform.position.y>=45)
        {
            lockon = true;
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(enemy!=null&&lockon==false)
        {
            if (goup > 0 && enemy.transform.position.y <= 49)
            {
                goup -= Time.deltaTime;
            }
            else if (goup <= 0 || enemy.transform.position.y >= 50)
            {
                lockon = true;
            }
        }
		
        if(lockon&&enemy!=null)
        {
            transform.LookAt(enemy.transform.position);
            transform.Translate(Vector3.forward * Time.deltaTime * gameObject.GetComponent<bullet>().speed*2);
        }
        if(enemy==null)
        {
            Destroy(gameObject);
        }
	}
}
