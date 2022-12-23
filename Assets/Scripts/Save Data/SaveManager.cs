using BayatGames.SaveGameFree;

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
            allBusinessData[i].Milestones = GameManager.Instance.AllBusiness[i].Milestones;
            allBusinessData[i].Bought = GameManager.Instance.AllBusiness[i].Bought;
            allBusinessData[i].CanBuy = GameManager.Instance.AllBusiness[i].CanBuy;
            allBusinessData[i].TimeToGenerateProfit = GameManager.Instance.AllBusiness[i].TimeToGenerateProfit;
            allBusinessData[i].Profit = GameManager.Instance.AllBusiness[i].Profit;
            allBusinessData[i].ProfitToAdd = GameManager.Instance.AllBusiness[i].ProfitToAdd;
        }
        
        SaveGame.Save(SAVE_KEY, allBusinessData);
    }
}
