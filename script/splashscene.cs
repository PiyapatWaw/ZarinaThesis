using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class splashscene : MonoBehaviour
{
    public GameObject Logo;
    public GameObject TransitionPic;
    int uicounting;
    // Use this for initialization
    void Start () {
        uicounting = 0;

    }
	
	// Update is called once per frame
	void Update () {
        uicounting++;
        if(uicounting == 1)
        {
            Logo.gameObject.SetActive(true);
        }
        if (uicounting == 180)
        {
            TransitionPic.gameObject.SetActive(true);
        }
        if(uicounting == 240)
        {
            SceneManager.LoadScene("first story");
        }
    }
}
