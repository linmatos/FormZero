using System;
using UnityEngine;

namespace UnityStandardAssets._2D
{
    public class MetroidvaniaCamera : MonoBehaviour
    {
        private BoxCollider2D cameraBox;                //The boxCollider of the camera
        private BoxCollider2D boundsBox;

        private Vector2 velocity;
        
        public Transform target;
        
        public float smoothTimeX;
        public float smoothTimeY;
        
        private void Start()
        {
            boundsBox = GetComponent<BoxCollider2D>();
            cameraBox = GetComponent<BoxCollider2D>();
            transform.parent = null;
        }

        private void Update()
        {
            AspectRatioBoxChange();
            if (target == null)
            {
                return;                     //Pra não dar aquele erro de não encontrar o target
            }

            float posX = Mathf.SmoothDamp(transform.position.x, target.position.x, ref velocity.x, smoothTimeX);
            float posY = Mathf.SmoothDamp(transform.position.y, target.position.y, ref velocity.y, smoothTimeY);

            transform.position = new Vector3(Mathf.Clamp(posX, boundsBox.bounds.min.x + cameraBox.size.x / 2, boundsBox.bounds.max.x - cameraBox.size.x / 2),
                                          Mathf.Clamp(posY, boundsBox.bounds.min.y + cameraBox.size.y / 2, boundsBox.bounds.max.y - cameraBox.size.y / 2),
                                          transform.position.z);

        }

        public void CameraStop(BoxCollider2D b)
        {
            //Debug.Log("CameraStop method activated");
            boundsBox = b;
        }

        void AspectRatioBoxChange()
        {
            //WARNING: Everything here depends on your CameraSize here's the config for 14

            //16:10 ratio
            if (Camera.main.aspect >= (1.6f) && Camera.main.aspect <= 1.7f)
            {
                cameraBox.size = new Vector2(45.5f, 28);
            }
            //16:9 ratio
            if (Camera.main.aspect >= (1.7f) && Camera.main.aspect <= 1.8f)
            {
                cameraBox.size = new Vector2(50, 28);
            }
            //5:4 ratio
            if (Camera.main.aspect >= 1.25f && Camera.main.aspect <= 1.3f)
            {
                cameraBox.size = new Vector2(35.2f, 28);
            }
            //4:3 ratio
            if (Camera.main.aspect >= 1.3f && Camera.main.aspect <= 1.4f)
            {
                cameraBox.size = new Vector2(37.5f, 28);
            }
            //3:2 ratio
            if (Camera.main.aspect >= (1.5f) && Camera.main.aspect <= 1.6f)
            {
                cameraBox.size = new Vector2(42, 28);
            }
        }

    }
}
