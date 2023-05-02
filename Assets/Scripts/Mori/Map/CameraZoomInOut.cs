using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoomInOut : MonoBehaviour
{
    public Camera cam;

    public bool camZoomIn;
    public float zoomSpeed;

    public bool mirrorMod;
    public float mirrorSpeed;
    float angleY;
    int camPosY;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CamZoomInOut();

        CamMirror();
    }

    void CamZoomInOut()
    {
        if (camZoomIn)
        {
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, 3, zoomSpeed);
        }
        else
        {
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, 5, zoomSpeed);
    
        }
    }

    void CamMirror()
    {
        if(mirrorMod)
        {
            angleY = Mathf.Lerp(transform.localEulerAngles.z, 180, mirrorSpeed);
            transform.localEulerAngles = new Vector3(transform.rotation.x, transform.rotation.y, angleY);
        }
        else
        {
            angleY = Mathf.Lerp(transform.localEulerAngles.z, 0, mirrorSpeed);
            transform.localEulerAngles = new Vector3(transform.rotation.x, transform.rotation.y, angleY);
        }

    }
}
