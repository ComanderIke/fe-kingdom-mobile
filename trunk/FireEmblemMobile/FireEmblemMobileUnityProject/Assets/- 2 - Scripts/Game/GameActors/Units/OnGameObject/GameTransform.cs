using UnityEngine;

namespace Assets.GameActors.Units.OnGameObject
{
    public class GameTransform
    {
        public GameObject GameObject { get; set; }

        public UnitInputController UnitController => GameObject.GetComponent<UnitInputController>();
        public UnitAnimator UnitAnimator => GameObject.GetComponent<UnitAnimator>();

        public void SetPosition(int x, int y)
        {
            GameObject.transform.localPosition = new Vector3(x, y, 0);
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
        public void DeParentLight()
        {
            UnitController.DeParentLight();
           
        }
        public void EnableLight()
        {
            UnitController.ResetLight();
        }
        public void DisableCollider()
        {
            GameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
        
        public void Destroy()
        {
            Object.Destroy(GameObject);
        }
    }
}
