using Game.WorldMapStuff.Controller;
using Game.WorldMapStuff.Interfaces;
using UnityEngine;

namespace Game.WorldMapStuff.Model
{
    public class WM_GameTransformManager
    {
        public GameObject GameObject { get; set; }

        public Transform Transform => GameObject.transform;
        

        public void SetPosition(int x, int y)
        {
            GameObject.transform.localPosition = new Vector3(x, y, 0);
        }
        
        public void SetPosition(Vector3 position)
        {
            GameObject.transform.position = position;
        }
        public Vector3 GetCenterPosition()
        {
            return GameObject.transform.localPosition+ new Vector3(0.5f,0.5f,0);
        }
        
        public Vector3 GetPosition()
        {
            return GameObject.transform.localPosition;
        }
        public float GetYRotation()
        {
            return GameObject.GetComponentInChildren<SpriteRenderer>().transform.localRotation.y;
        }
        public void SetYRotation(float y)
        {
            GameObject.GetComponentInChildren<SpriteRenderer>().transform.localRotation = new Quaternion(0, y, 0, 0);
        }
        public void EnableCollider()
        {
            GameObject.GetComponent<BoxCollider2D>().enabled = true;
        }

        public void DisableCollider()
        {
            GameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
        
        public void Destroy()
        {
            Object.Destroy(GameObject);
        }

        public void SetParent(Transform transform)
        {
            Transform.SetParent(transform, false);
            Transform.localPosition = new Vector3(0,0,-0.1f);
        }

        public IWorldMapUnitInputReceiver GetInputReceiver()
        {
            return Transform.GetComponent<WM_ActorController>().inputReceiver;
        }
        public void SetInputReceiver(IWorldMapUnitInputReceiver inputReceiver)
        {
            Transform.GetComponent<WM_ActorController>().inputReceiver=inputReceiver;
        }
    }
}