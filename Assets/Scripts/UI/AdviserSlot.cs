using System;
using BayatGames.SaveGameFree;
using UnityEngine;
using UnityEngine.UI;

public class AdviserSlot : MonoBehaviour
{
    public static event Action<AdviserSlot> EventAdviserSlotClicked; 

    [SerializeField] private Image icon;
    [SerializeField] private int index;

    public AdviserSO MyAdviser { get; set; }

    public string AdviserSaved
    {
        get => SaveGame.Load<string>(KeyAdviserSaved);
        set => SaveGame.Save(KeyAdviserSaved, value);
    }


    private string KEY_ADVISER = "ADVISER";
    public string KeyAdviserSaved => KEY_ADVISER + index;


    public void SetAdviser(AdviserCard adviserCard)
    {
        MyAdviser = adviserCard.MyAdviser;
        AdviserSaved = MyAdviser.adviserName;
    }


    public void ClickSlot()
    {
        EventAdviserSlotClicked?.Invoke(this);
        
        if (MyAdviser != null)
        {
            ShowAdviserData();
        }
    }
    
    
    public void ShowAdviserData()
    {
        icon.sprite = MyAdviser.icon;
    }
}
