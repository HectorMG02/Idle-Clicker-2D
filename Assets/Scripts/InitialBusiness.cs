using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialBusiness : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private int initialProfit = 1;

    private int _profit;

    private void Start()
    {
        _profit = initialProfit;
    }


    public void GetProfit()
    {
        MoneyManager.instance.AddMoney(_profit);
    }
}
