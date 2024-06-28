using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class wave
{
    [System.Serializable]
    public struct wavedatabase
    {
        public enemydatabase[] enemydata;
    }
    [System.Serializable]
    public struct enemydatabase
    {
        public GameObject enemy;
        public waypoint Waypoint;
        public int enemycount;
        public Sprite enemyimage;
    }
    public wavedatabase spawnmanager;
}
