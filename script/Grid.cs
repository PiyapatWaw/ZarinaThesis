using UnityEngine;
using System.Collections.Generic;

public class Grid : MonoBehaviour
{
    public List<node> map = new List<node>();
    public GameObject placer;
    public GameObject allplacer;
    public Vector3 startpos;
    public int x, z;

    private void Awake()
    {
        allplacer.SetActive(true);
        int count = 1;
        GameObject[] _allplacer = GameObject.FindGameObjectsWithTag("placer");
        foreach (var item in _allplacer)
        {
            map.Add(new node(item.transform.position, item));
        }

        foreach (var item in map)
        {
            RaycastHit hit;
            Vector3 checkposition;
            checkposition = new Vector3(item.position.x + 20, item.position.y, item.position.z);
            if (Physics.Linecast(item.position, checkposition, out hit))
            {
                if (hit.collider.gameObject.tag == "placer")
                {
                    foreach (var node in map)
                    {
                        if (node.position == hit.collider.gameObject.transform.position)
                        {
                            item.neighbor.Add(node);
                        }
                    }
                }
            }
            checkposition = new Vector3(item.position.x - 20, item.position.y, item.position.z);
            if (Physics.Linecast(item.position, checkposition, out hit))
            {
                if (hit.collider.gameObject.tag == "placer")
                {
                    foreach (var node in map)
                    {
                        if (node.position == hit.collider.gameObject.transform.position)
                        {
                            item.neighbor.Add(node);
                        }
                    }
                }
            }
            checkposition = new Vector3(item.position.x, item.position.y , item.position.z + 20);
            if (Physics.Linecast(item.position, checkposition, out hit))
            {
                if (hit.collider.gameObject.tag == "placer")
                {
                    foreach (var node in map)
                    {
                        if (node.position == hit.collider.gameObject.transform.position)
                        {
                            item.neighbor.Add(node);
                        }
                    }
                }
            }
            checkposition = new Vector3(item.position.x, item.position.y , item.position.z - 20);
            if (Physics.Linecast(item.position, checkposition, out hit))
            {
                if (hit.collider.gameObject.tag == "placer")
                {
                    foreach (var node in map)
                    {
                        if (node.position == hit.collider.gameObject.transform.position)
                        {
                            item.neighbor.Add(node);
                        }
                    }
                }
            }
        }

        allplacer.SetActive(false);
    }

}