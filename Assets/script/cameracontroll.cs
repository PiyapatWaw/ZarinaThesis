using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameracontroll : MonoBehaviour {

    public float mouseborder = 10f;
    public float speed = 5;
    public Vector2 limitMax;
    public Vector2 limitMin;
    // Update is called once per frame
    void Update ()
    {
        Vector3 pos = transform.position;

		if(Input.mousePosition.y>=Screen.height-mouseborder&&transform.position.x<limitMax.x)
        {
            pos.x += speed * Time.deltaTime;
        }
        if (Input.mousePosition.y <= mouseborder && transform.position.x > limitMin.x)
        {
            pos.x -= speed * Time.deltaTime;
        }
        if (Input.mousePosition.x >= Screen.width - mouseborder && transform.position.z > limitMin.y)
        {
            pos.z -= speed * Time.deltaTime;
        }
        if (Input.mousePosition.x <= mouseborder && transform.position.z < limitMax.y)
        {
            pos.z += speed * Time.deltaTime;
        }

        transform.position = pos;

    }
}
