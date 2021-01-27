using System.Collections;
using Game.GameActors.Units.Humans;
using Game.GameActors.Units.OnGameObject;
using Game.GameInput;
using Game.Manager;
using Game.Map;
using Game.Mechanics;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Testing.PlayMode
{
    public class UnitInputTest
    {

        // A UnityTest behaves like a coroutine in PlayMode
        // and allows you to yield null to skip a frame in EditMode
        private IEnumerator SetupScene()
        {
            if(SceneManager.GetActiveScene().name!="Level2")
                SceneManager.LoadScene("Level2", LoadSceneMode.Single);
            yield return new WaitForSeconds(0.2f);
        }

        [UnityTest]
        public IEnumerator _3_SelectEnemyUnit()
        {
            yield return SetupScene();
            
            UnitInputController unitInputController = GameObject.Find("Wolf").GetComponent<UnitInputController>();
            unitInputController.OnMouseDown();
            unitInputController.OnMouseUp();

            UnitSelectionSystem selectionSystem = GridGameManager.Instance.GetSystem<UnitSelectionSystem>();

            Assert.AreNotEqual(selectionSystem.SelectedCharacter, unitInputController.unit);
        }
        [UnityTest]
        public IEnumerator _4_DeselectPlayerUnit1()
        {
            yield return SetupScene();
            
            GridInputSystem inputSystem = GridGameManager.Instance.GetSystem<GridInputSystem>();
            inputSystem.GridClicked(6,6);
            UnitSelectionSystem selectionSystem = GridGameManager.Instance.GetSystem<UnitSelectionSystem>();
            Assert.Null( selectionSystem.SelectedCharacter);
            yield return null;
        }
        [UnityTest]
        public IEnumerator _5_MovePlayerUnit1()
        {
            yield return SetupScene();
            
            UnitInputController unitInputController = GameObject.Find("Hector").GetComponent<UnitInputController>();
            unitInputController.OnMouseDown();
            unitInputController.OnMouseUp();
            int x = unitInputController.unit.GridComponent.GridPosition.X;
            int y = unitInputController.unit.GridComponent.GridPosition.Y;
            UnitSelectionSystem selectionSystem = GridGameManager.Instance.GetSystem<UnitSelectionSystem>();
            Assert.AreEqual(unitInputController.unit, selectionSystem.SelectedCharacter);
            yield return null;
            GridInputSystem inputSystem = GridGameManager.Instance.GetSystem<GridInputSystem>();
            inputSystem.GridClicked(2,1);
            GridSystem gridSystem = GridGameManager.Instance.GetSystem<GridSystem>();

            yield return new WaitForSeconds(0.5f);
            unitInputController.OnMouseDown();
            unitInputController.OnMouseUp();
            //inputSystem.GridClicked(2,1);
            
            Debug.Log(unitInputController.unit.GridComponent.GridPosition.X+" "+unitInputController.unit.GridComponent.GridPosition.Y);
            Assert.AreEqual(unitInputController.unit.GridComponent.GridPosition.X, 2);
            Assert.AreEqual(unitInputController.unit.GridComponent.GridPosition.Y, 1);
            Assert.AreEqual(unitInputController.unit, gridSystem.Tiles[2, 1].Actor);
            Assert.Null(gridSystem.Tiles[x, y].Actor);

        }
        [UnityTest]
        public IEnumerator _6_DragPlayerUnit2()
        {
            yield return SetupScene();
            
            UnitInputController unitInputController = GameObject.Find("Leila").GetComponent<UnitInputController>();
          //  unitInputController.OnMouseDown();
            unitInputController.StartDrag();
            int x = unitInputController.unit.GridComponent.GridPosition.X;
            int y = unitInputController.unit.GridComponent.GridPosition.Y;
            UnitSelectionSystem selectionSystem = GridGameManager.Instance.GetSystem<UnitSelectionSystem>();
            yield return null;
            Assert.AreEqual(unitInputController.unit, selectionSystem.SelectedCharacter);
            //unitInputController.OnMouseDrag();
            unitInputController.Dragging(2,5);
            yield return new WaitForSeconds(0.5f);
            
            
            unitInputController.OnMouseUp();
            

            yield return null;
            GridSystem gridSystem = GridGameManager.Instance.GetSystem<GridSystem>();
            //inputSystem.GridClicked(2,1);
            
            Debug.Log(unitInputController.unit.GridComponent.GridPosition.X+" "+unitInputController.unit.GridComponent.GridPosition.Y);
            Assert.AreEqual(unitInputController.unit.GridComponent.GridPosition.X, 2);
            Assert.AreEqual(unitInputController.unit.GridComponent.GridPosition.Y, 5);
            Assert.AreEqual(unitInputController.unit, gridSystem.Tiles[2, 5].Actor);
            Assert.Null(gridSystem.Tiles[x, y].Actor);

        }
        [UnityTest]
        public IEnumerator _1_SelectPlayerUnit()
        {
            yield return SetupScene();
            
            UnitInputController unitInputController = GameObject.Find("Hector").GetComponent<UnitInputController>();
            unitInputController.OnMouseDown();
            unitInputController.OnMouseUp();

            UnitSelectionSystem selectionSystem = GridGameManager.Instance.GetSystem<UnitSelectionSystem>();

            Assert.AreEqual(unitInputController.unit, selectionSystem.SelectedCharacter);
        }
        [UnityTest]
        public IEnumerator _2_SelectPlayerUnit2()
        {
            yield return SetupScene();
            
            UnitInputController unitInputController = GameObject.Find("Leila").GetComponent<UnitInputController>();
            unitInputController.OnMouseDown();
            unitInputController.OnMouseUp();

            UnitSelectionSystem selectionSystem = GridGameManager.Instance.GetSystem<UnitSelectionSystem>();

            Assert.AreEqual(unitInputController.unit, selectionSystem.SelectedCharacter);
        }
    }
}