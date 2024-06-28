using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletsound : MonoBehaviour
{
    private AudioSource audio;
	// Use this for initialization
	void Start () {
        audio = gameObject.GetComponent<AudioSource>();
        transform.parent = null;

    }
	
	// Update is called once per frame
	void Update ()
    {
        
		if(audio.isPlaying==false)
        {
            Destroy(gameObject);
        }
	}
}
