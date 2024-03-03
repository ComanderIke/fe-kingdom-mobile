using System.Collections.Generic;
using Game.GameActors.Units;
using UnityEngine;

namespace Game.GUI.EncounterUI.Church
{
    public class ShrineController : MonoBehaviour
    {
        [SerializeField] private ShrineCameraController cameraController;

        [SerializeField] private ShrineUnitController unitController;
        [SerializeField] private GodStatueController godStatueController;
       
        public void Show(List<God> gods)
        {
            gameObject.SetActive(true);
            
            godStatueController.SetGods(gods);
            godStatueController.Reset();
            unitController.onMoveFinished -= UpdateGods;
            unitController.onMoveFinished += UpdateGods;
        }

        void UpdateGods(bool directionLeft)
        {
            if(directionLeft)
                godStatueController.Next();
            else
            {
                godStatueController.Previous();
            }
        }
        public void Hide()
        {
            unitController.onMoveFinished -= UpdateGods;
            gameObject.SetActive(false);
            
        }
        
        public void SetUnit(Unit unit)
        {
            unitController.SetUnit(unit);
        }
        // Update is called once per frame
        public void NextStatue()
        {
            cameraController.RotateRight();
            unitController.MoveLeft();
            //godStatueController.Next();
        }
        public void PrevStatue()
        {
            cameraController.RotateLeft();
            unitController.MoveRight();
            //godStatueController.Previous();
        }

        public void JumpToGod(int selectedGod)
        {
            MyDebug.LogTest("Jump To: "+selectedGod);
            cameraController.JumpTo(selectedGod);
            unitController.MoveTo(selectedGod);
        }
    }
}
