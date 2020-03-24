using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ToLoadingScene : MonoBehaviour {

    // Use this for initialization\

    public Image loading;
    public Image loadingbar;
    public GameObject playbutton;
    AsyncOperation asyn;

    public void LoadSceneTutorial()
    {
        StartCoroutine(asynload("tutorial"));
    }
    public void LoadScenePlay()
    {
        StartCoroutine(asynload("stage test"));
    }
    public void ExitGame()
    {
        Application.Quit();
    }
	void Start () {
	}
	
	IEnumerator asynload(string name)
    {
        loading.gameObject.SetActive(true);
        asyn = SceneManager.LoadSceneAsync(name);
        asyn.allowSceneActivation = false;
        while (!asyn.isDone)
        {
            loadingbar.fillAmount = asyn.progress;
            if(asyn.progress==0.9f)
            {
                loadingbar.fillAmount = 1;
                loadingbar.gameObject.SetActive(false);
                playbutton.SetActive(true);
            }
            yield return null;
        }
        
    }

    public void play()
    {
        asyn.allowSceneActivation = true;
    }
}
