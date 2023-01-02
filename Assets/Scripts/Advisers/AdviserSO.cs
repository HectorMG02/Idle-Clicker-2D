using BayatGames.SaveGameFree;
using UnityEngine;

public enum AdviserType
{
    BusinessUtility,
    ClickUtility,
    UpdateDiscountUtility,
    MilestoneDiscountUtility,
}

[CreateAssetMenu(fileName = "New Adviser", menuName = "Advisers")]
public class AdviserSO : ScriptableObject
{
    public string adviserName;
    public Sprite icon;
    public AdviserType adviserType;
    public float valueMultiplier; // if improve the business x5 this will multiply the business by 5
    
    [TextArea] public string description;
    
    private string PURCHASED = "ADVISER_PURCHASED";
    private string SELECTED = "ADVISER_SELECTED";
    
    public string KeyPurchased => adviserName + PURCHASED;
    public string KeySelected => SELECTED + adviserName;


    public bool Purchased
    {
        get => SaveGame.Load<bool>(KeyPurchased);
        set => SaveGame.Save(KeyPurchased, value);
    }

    public bool Selected
    {
        get => SaveGame.Load<bool>(KeySelected);
        set => SaveGame.Save(KeySelected, value);
    }
    
}
