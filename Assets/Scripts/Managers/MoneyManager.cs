using BayatGames.SaveGameFree;
using UnityEngine;

public class MoneyManager: MonoBehaviour
{
    public static MoneyManager Instance;
    public int CurrentMoney { get; private set; }
    
    public string MONEY_KEY = "MY_MONEY";

    private void Awake()
    {
        Instance = this;
        if (SaveGame.Exists(MONEY_KEY))
        {
            CurrentMoney = SaveGame.Load<int>(MONEY_KEY);
        }
    }

    public void AddMoney(int amount)
    {
        CurrentMoney += amount;
        SaveGame.Save(MONEY_KEY, CurrentMoney);
    }
    
    
    public void RemoveMoney(int amount)
    {
        if (CurrentMoney >= amount)
        {
            CurrentMoney -= amount;
            SaveGame.Save(MONEY_KEY, CurrentMoney);
        }
        else
        {
            Debug.Log("CurrentMoney: " + CurrentMoney + " is less than amount: " + amount);
        }
    }
}