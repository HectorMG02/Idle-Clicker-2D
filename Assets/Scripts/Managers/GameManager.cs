using System;
using System.Collections;
using System.Collections.Generic;
using BayatGames.SaveGameFree;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Business")]
    [SerializeField] private Business[] allBusiness;
    
    [Header("Prices")]
    [SerializeField] private int priceFirstBusiness = 10;
    [SerializeField] private int priceBusinessMultiplier = 2;

    [Header("Test")]
    [SerializeField] private bool debug;
    
    public int PriceNewBusiness { get; set; }
    public Business[] AllBusiness => allBusiness;

    private void Awake()
    {
        Instance = this;
        PriceNewBusiness = priceFirstBusiness;

        LoadAllBusiness();

        if (debug)
        {
            SaveGame.Delete(SaveManager.SAVE_KEY);
        }
    }

    private void LoadAllBusiness()
    {
        if (SaveGame.Exists(SaveManager.SAVE_KEY))
        {
            BusinessData[] myBusiness = SaveGame.Load<BusinessData[]>(SaveManager.SAVE_KEY);
            for (int i = 0; i < myBusiness.Length; i++)
            {
                if (myBusiness[i] != null)
                {
                    allBusiness[i].LoadBusinessData(myBusiness[i]);   
                }
            }
        }
    }
    
    
    public void SetNewBusiness(Business business)
    {
        business.Bought = true;
        business.CanBuy = false;
        CanBuyNextBusiness(business);
    }

    private void CanBuyNextBusiness(Business business)
    {
        if (business.Index + 1 > allBusiness.Length)
        {
            allBusiness[business.Index + 1].CanBuy = true;
        }
    }
}