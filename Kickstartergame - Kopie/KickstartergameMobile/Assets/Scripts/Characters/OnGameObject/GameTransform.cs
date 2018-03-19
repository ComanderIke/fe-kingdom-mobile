﻿using UnityEngine;

namespace Assets.Scripts.Characters
{
    public class GameTransform
    {
        public GameObject GameObject { get; set; }

        public void SetPosition(int x, int y)
        {
            GameObject.transform.localPosition = new Vector3(x, y, 0);
        }
        public Vector3 GetPosition()
        {
            return GameObject.transform.localPosition;
        }
        public Vector3 GetRotation()
        {
            return GameObject.transform.localRotation.eulerAngles;
        }
        public void EnableCollider()
        {
            GameObject.GetComponent<BoxCollider>().enabled = true;
        }
        public void DisableCollider()
        {
            GameObject.GetComponent<BoxCollider>().enabled = false;
        }
        public void Destroy()
        {
            GameObject.Destroy(GameObject);
        }
    }
}
