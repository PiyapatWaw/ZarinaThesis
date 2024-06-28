using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class splashscene : MonoBehaviour
{
    public GameObject Logo;
    public GameObject TransitionPic;
    
    IEnumerator Start () 
    {
        Logo.gameObject.SetActive(true);

        yield return new WaitForSeconds(1.5f);
        TransitionPic.gameObject.SetActive(true);
        yield return new WaitForSeconds(2.5f);
        SceneManager.LoadScene("MainMenu");
    }
}
