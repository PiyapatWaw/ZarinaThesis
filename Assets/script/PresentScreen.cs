using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class PresentScreen : MonoBehaviour {

    // Use this for initialization
    private int uicounting = 0;
    public VideoPlayer VideoPlayer;
    public GameObject loading;
    public Image loadingbar;
    AsyncOperation asyn;
    void Start () {
		
	}
    bool checkonetime = false;
	// Update is called once per frame
	void Update ()
    {
        if(VideoPlayer.isPlaying==false&& checkonetime==false)
        {
            StartCoroutine(asynload("MainMenu"));
            checkonetime = true;
        }
       
	}

    IEnumerator asynload(string name, bool usebutton = false)
    {
        loading.gameObject.SetActive(true);
        asyn = SceneManager.LoadSceneAsync(name);
        asyn.allowSceneActivation = false;
        while (!asyn.isDone)
        {
            loadingbar.fillAmount = asyn.progress;
            if (asyn.progress == 0.9f)
            {
                loadingbar.fillAmount = 1;
                loadingbar.gameObject.SetActive(false);
                asyn.allowSceneActivation = true;

            }
            yield return null;
        }

    }

    public void skip()
    {
        StartCoroutine(asynload("MainMenu"));
    }
}
