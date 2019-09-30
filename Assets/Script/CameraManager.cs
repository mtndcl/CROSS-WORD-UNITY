using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour
{
    public Camera cam;


 
    private GameObject canvas;
    
    public void Start()
    {

        canvas = GameObject.FindGameObjectWithTag("canvas");
        cam.transform.position = canvas.transform.position;
        cam.orthographicSize = cam.transform.position.y;


    }

}