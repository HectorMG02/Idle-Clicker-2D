using System;
using UnityEngine;

public class MoneyManager: MonoBehaviour
{
    public static MoneyManager instance;
    
    
    
    public int CurrentMoney { get; private set; }

    private void Awake()
    {
        instance = this;
    }

    public void AddMoney(int amount)
    {
        CurrentMoney += amount;
    }
}