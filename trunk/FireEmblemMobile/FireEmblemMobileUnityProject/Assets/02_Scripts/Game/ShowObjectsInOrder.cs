using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class ShowObjectsInOrder : MonoBehaviour
    {
        [SerializeField] private float timeShown;
        [SerializeField] private bool loop;

        private float time;
        private GameObject current;
        void Start()
        {
        
        }

        void Update()
        {
            time += Time.deltaTime;
            if (time >= timeShown)
            {
                time = 0;
                ShowNextGameObject();
            }
        }
        
        private void ShowNextGameObject()
        {
            int index = 0;
            if (current != null)
            {
                current.SetActive(false);
                index = current.transform.GetSiblingIndex()+1;
            }

            List<Transform> children = GetComponentsInChildren<Transform>(true).ToList();
            children.Remove(transform);
            if (transform.childCount >= 1)
            {
                if (index >= transform.childCount)
                    index = 0;
                current = children[index].gameObject;
            }

            if (current != null)
            {
                current.gameObject.SetActive(true);
            }
        }
    }
}
