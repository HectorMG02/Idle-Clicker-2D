using System;
using BayatGames.SaveGameFree;
using UnityEngine;

public class MoneyManager: MonoBehaviour
{
    public static MoneyManager instance;
    public int CurrentMoney { get; private set; }
    
    private string MONEY_KEY = "MY_MONEY";

    private void Awake()
    {
        instance = this;
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
}