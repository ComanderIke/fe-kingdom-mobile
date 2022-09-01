using System;
using System.Collections;
using System.Collections.Generic;
using __2___Scripts.Game.Utility;
using Game.GameActors.Units.Numbers;
using Game.Mechanics;
using Game.WorldMapStuff.Model;
using TMPro;
using UnityEngine;

public class UIEventController : MonoBehaviour
{
    public Canvas canvas;
    public EventEncounterNode node;
    [HideInInspector]
    public Party party;
    private RandomEvent randomEvent;
    public TextMeshProUGUI headline;
    public TextMeshProUGUI description;
    public Transform layout;
    [SerializeField] private UICharacterFace characterFace;
    [SerializeField] private UIUnitIdleAnimation unitIdleAnimation;
    public GameObject textOptionPrefab;
    public GameObject itemOptionPrefab;
    public GameObject blessingOptionPrefab;
    public GameObject skillOptionPrefab;
    public GameObject fightOptionPrefab;
    public GameObject goldStoneOptionPrefab;

    private EventScene currentScene;

    // Start is called before the first frame update
    public void Show(EventEncounterNode node, Party party)
    {
        this.node = node;
        canvas.enabled = true;
        this.party = party;
        this.randomEvent = node.randomEvent;
        currentScene = randomEvent.scenes[0];
       
        // if(instantiatedObjects==null)
        //     instantiatedObjects = new List<GameObject>();
        UpdateUI();
        //FindObjectOfType<UICharacterViewController>().Show(party.members[party.ActiveUnitIndex]);
        // for (int i=0; i<church.shopItems.Count; i++)
        // {
        //     var item = church.shopItems[i];
        //     shopItems[i].SetValues(new ShopItem(item.cost, item.Sprite, item.Description));
        // }
    }
    public void NextClicked()
    {
        party.ActiveUnitIndex++;
        UpdateUI();
    }

    public void PrevClicked()
    {
        party.ActiveUnitIndex--;
        UpdateUI();
    }

    void BattleEnded()
    {
        BattleSystem.OnBattleFinished -= BattleEnded;
        currentScene = randomEvent.scenes[current.nextSceneIndex];
        UpdateUI();
    }

    private ResponseOption current;
    public void OptionClicked(TextOptionController textOptionController)
    {
        current = textOptionController.Option;
        if (current.nextSceneIndex != -1)
        {
            if (current.type == EventSceneType.Fight)
            {
               
                var battleSystem = AreaGameManager.Instance.GetSystem<BattleSystem>();
                var enemy = Instantiate(current.EnemyToFight);
                enemy.Initialize();
                Debug.Log("Enemy Weapon: "+enemy.EquippedWeapon.name);
                battleSystem.StartBattle(party.ActiveUnit, enemy, false);
                BattleSystem.OnBattleFinished += BattleEnded;
                Debug.Log("Fight!");
            }
            else
            {
                currentScene = randomEvent.scenes[current.nextSceneIndex];

                UpdateUI();
            }
        }
        else
        {
            Debug.Log("END EVENT Clicked!");
            Hide();
            node.Continue();
        }
        
    }

    void Hide()
    {
        canvas.enabled = false;
    }
    public void UpdateUI()
    {
        unitIdleAnimation.Show(party.ActiveUnit);
        characterFace.Show(party.ActiveUnit);
        headline.SetText(randomEvent.headline);
        layout.DeleteAllChildren();
        this.description.text = currentScene.MainText;
        foreach (var textoption in currentScene.textOptions)
        {
            GameObject prefab = textOptionPrefab;
            if (textoption.fight)
                prefab = fightOptionPrefab;
            if (textoption.reward != null)
            {
                if (textoption.reward.item != null)
                {
                    prefab = itemOptionPrefab;
                }
                else if (textoption.reward.skill !=null)
                {
                    prefab = skillOptionPrefab;
                }
                else if (textoption.reward.Blessing !=null)
                {
                    prefab = blessingOptionPrefab;
                }
                else if (textoption.reward.gold !=0 ||textoption.reward.smithingStones!=0||textoption.reward.experience!=0)
                {
                    prefab = goldStoneOptionPrefab;
                }
            }
                
            var go=Instantiate(prefab, layout);
            int stat = party.ActiveUnit.Stats.Attributes.GetFromIndex(textoption.StatIndex);
            string statText = stat+" "+Attributes.GetAsText(textoption.StatIndex);
            TextOptionState state = TextOptionState.Normal;
            if(stat < textoption.StatRequirement)
                state = TextOptionState.Impossible;
            else if(stat >= (textoption.StatRequirement+10))
                state = TextOptionState.High;
            go.GetComponent<TextOptionController>().Setup(textoption, textoption.Text,statText,state, this);
        }
    }
}
