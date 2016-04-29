using UnityEngine;
using System.Collections;

public class CameraBounds : MonoBehaviour {

    private BoxCollider2D boundsBox;
    private Camera cam;

    void Start()
    {
        boundsBox = GetComponent<BoxCollider2D>();
        cam = Camera.main;
    }

    void OnTriggerStay2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            cam.SendMessage("CameraStop", boundsBox);
            //coll.gameObject.SendMessage("CameraStop", boundsBox);
            //Debug.Log("Colidindo com " + coll.gameObject.tag);
        }
    }
}
