using System;
using UnityEngine;

namespace BrewedInk.CardCheat.Game
{
    public class CameraBehaviour : GameComponent
    {

        public Transform rig;
        public Camera cam;

        public Transform[] importantSites;

        public Vector3 framePadding;

        public float rigSmoothTime = .1f;
        public float zoomSmoothTime = .1f;
        private Vector2 _rigVelocity;
        private float _cameraVelocity;
        
        void Update()
        {
            GetCameraDetails(out var position, out var size);
            rig.localPosition = Vector2.SmoothDamp(rig.localPosition, position, ref _rigVelocity, rigSmoothTime);
            cam.orthographicSize = Mathf.SmoothDamp(cam.orthographicSize, size, ref _cameraVelocity, zoomSmoothTime);
        }

        

        public void GetCameraDetails(out Vector2 position, out float size)
        {
            //https://pressstart.vip/tutorials/2018/06/14/37/understanding-orthographic-size.html
            var b = root.ComputeWorldBounds();

            foreach (var site in importantSites)
            {
                b.Encapsulate(site.position);
            }
            
            b.Expand(framePadding); // frame padding...
            

            position = b.center;


            // var screenRatio = (float)Screen.width / (float)Screen.height; // use this for screen camera 
            var screenRatio = cam.targetTexture.width / (float)cam.targetTexture.height; // use this for a render target
            var targetRatio = b.size.x / b.size.y;

            if (screenRatio >= targetRatio)
            {
                size = b.size.y / 2;
            }
            else
            {
                float differenceInSize = targetRatio / screenRatio;
                size = b.size.y / 2 * differenceInSize;
            }
        }


    }
}