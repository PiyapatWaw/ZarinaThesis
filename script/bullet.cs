using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour {

    // Use this for initialization
    private stagecontrol stage;
    public int damage;
    public int speed;
    public bool projectile;
    public bool antibarrier;

    void Start ()
    {
        if(!projectile)
            Invoke("Destroyme",2.5f);
        stage = GameObject.Find("Canvas").GetComponent<stagecontrol>();
        
    }
	
	// Update is called once per frame
	void Update () 
    {
        if (projectile)
        {
            if(gameObject.GetComponent<homing>().lockon==false)
                transform.Translate(Vector3.forward * Time.deltaTime * speed);
        }
        else
        {
            transform.Translate(Vector3.forward * Time.deltaTime * speed);
        }
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.tag == "enemy")
        {

            collision.gameObject.GetComponent<enemy>().calculatedamage(damage, antibarrier);
            
            if (collision.gameObject.GetComponent<enemy>().hp <= 0)
            {
                collision.gameObject.GetComponent<enemy>().incressresourse();
                stage.allenemy.Remove(collision.gameObject);
                collision.gameObject.GetComponent<enemy>().destroyme();
            }
            Destroy(gameObject);
        }
        
    }

    private void Destroyme()
    {
        Destroy(gameObject);
    }
}
