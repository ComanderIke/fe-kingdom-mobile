using System;
using LostGrace;
using UnityEngine;

public class ChooseDifficultyUI : MonoBehaviour
{
   
    public Action OnFinished { get; set; }
    
    [SerializeField] private ChooseDifficultyButtonUI difficultyButton1;
    [SerializeField] private ChooseDifficultyButtonUI difficultyButton2;
    [SerializeField] private ChooseDifficultyButtonUI difficultyButton3;
    [SerializeField] private CanvasGroup canvasGroup;
    
    private bool difficultySelected = false;
    private ChooseDifficultyButtonUI selected;

    

    public void Show()
    {
        
    
        canvasGroup.gameObject.SetActive(true);
        TweenUtility.FadeIn(canvasGroup);
        
        difficultySelected = false;
    
       
        selected = null;
        difficultyButton1.OnClick -= SkillButtonClicked;
        difficultyButton2.OnClick -= SkillButtonClicked;
        difficultyButton3.OnClick -= SkillButtonClicked;
        difficultyButton1.OnClick += SkillButtonClicked;
        difficultyButton2.OnClick += SkillButtonClicked;
        difficultyButton3.OnClick += SkillButtonClicked;
        difficultyButton1.Show();
        difficultyButton2.Show();
        difficultyButton3.Show();


    }

    void SkillButtonClicked(ChooseDifficultyButtonUI button)
    {
        if (selected != null)
            selected.Deselect();
        selected = button;
        button.Select();
    }

  
    public void ChooseClicked()
    {
        MyDebug.LogInput("Choose Clicked");
        if (selected == null||difficultySelected)
            return;
        difficultySelected = true;
        GameConfig.Instance.ConfigProfile.chosenDifficulty = selected.profile;
        //SaveGameManager.currentSaveData.slotData.difficulty = selected.profile.name;
        SaveGameManager.Save();
        Hide();
    }

    

    public void Hide()
    {
        TweenUtility.FadeOut(canvasGroup).setOnComplete(()=>
        {
           
            OnFinished?.Invoke();
            difficultyButton1.Hide();
            difficultyButton2.Hide();
            difficultyButton3.Hide();
            canvasGroup.gameObject.SetActive(false);
        });
    }

    
}