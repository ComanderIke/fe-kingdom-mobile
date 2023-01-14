using Game.GameInput;
using UnityEngine;

namespace Game.GameActors.Units.OnGameObject
{
    public class GameTransformManager
    {
        public GameObject GameObject { get; set; }

        public Transform Transform => GameObject.transform;

        public UnitInputController UnitController
        {
            get
            {
                if(GameObject!=null)
                    return GameObject.GetComponentInChildren<UnitInputController>();
                return null;
            }
        }

        public UnitAnimator UnitAnimator
        {
            get
            {
                if(GameObject!=null)
                    return GameObject.GetComponentInChildren<UnitAnimator>();
                return null;
            }
        }

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
            return GameObject.transform.position+ new Vector3(0.5f,0.5f,0);
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

        public void Die()
        {
            if (GameObject == null)
            {
                
                return;
            }
            GameObject.SetActive(false);
            //Destroy();
        }
    }
}
