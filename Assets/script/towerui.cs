using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class towerui : MonoBehaviour
{
    public GameObject linkobject;

    public void destroyobject()
    {
        Destroy(linkobject);
    }
}
