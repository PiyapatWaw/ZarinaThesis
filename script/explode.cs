using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explode : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        Invoke("destroyme",2.5f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void destroyme()
    {
        Destroy(gameObject);
    }
}
