using System.Collections;
using System.Collections.Generic;
using BayatGames.SaveGameFree;
using UnityEngine;

public static class SaveManager
{
    public static string SAVE_KEY = "MY_DATA";

    public static void SaveBusiness()
    {
        int businessQuantity = GameManager.Instance.AllBusiness.Length;
        BusinessData[] allBusinessData = new BusinessData[businessQuantity];
        
        for (int i = 0; i < businessQuantity; i++)
        {
            allBusinessData[i] = new BusinessData();
            allBusinessData[i].bought = GameManager.Instance.AllBusiness[i].Bought;
            allBusinessData[i].canBuy = GameManager.Instance.AllBusiness[i].CanBuy;
        }
        
        SaveGame.Save(SAVE_KEY, allBusinessData);
    }
}
