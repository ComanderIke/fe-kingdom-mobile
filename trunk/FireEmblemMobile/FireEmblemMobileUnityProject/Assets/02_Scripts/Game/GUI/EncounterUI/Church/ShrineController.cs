using System.Collections;
using System.Collections.Generic;
using Game.GameActors.Units;
using UnityEngine;

namespace LostGrace
{
    public class ShrineController : MonoBehaviour
    {
        [SerializeField] private ShrineCameraController cameraController;

        [SerializeField] private ShrineUnitController unitController;
       

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void SetGods(List<God> gods)
        {
            
        }
        public void SetUnit(Unit unit)
        {
            unitController.SetUnit(unit);
        }
        // Update is called once per frame
        public void NextStatue()
        {
           // cameraController.RotateLeft();
            unitController.MoveLeft();
        }
        public void PrevStatue()
        {
            //cameraController.RotateRight();
            unitController.MoveRight();
        }
    }
}
