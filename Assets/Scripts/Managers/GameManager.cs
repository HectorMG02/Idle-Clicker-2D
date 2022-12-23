using BayatGames.SaveGameFree;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Test")]
    [SerializeField] private bool debug;
    
    [Header("Business")]
    [SerializeField] private Business[] allBusiness;
    
    [Header("Prices")]
    [SerializeField] private int priceFirstBusiness = 10;
    [SerializeField] private int priceBusinessMultiplier = 2;

    [Header("Profits")]
    [SerializeField] private int initialProfit = 2;
    [SerializeField] private int profitLevelMultiplier = 2;
    [SerializeField] private int newBusinessProfitIncrement = 400;

    [Header("Initial Costs")]
    [SerializeField] private int initialCost = 2;
    [SerializeField] [Range(0, 100)] private int costUpdatePercentage = 2;

    [Header("Initial Profit Times")]
    [SerializeField] private float timeToGenerateProfit = 2f;
    [SerializeField] private float timeToGenerateProfitMultiplier = 2f;
    public int PriceNewBusiness { get; private set; }
    public Business[] AllBusiness => allBusiness;

    private int _myProfit;
    private int _myProfitLevelMultiplier;
    private int _myCost;
    private int _myCostUpdatePercentage;
    private float _myTimeToGenerateProfit;
    
    
    private string KEY_PRICE_NEW_BUSINESS = "PRICE_NEW_BUSINESS";
    private string KEY_PROFIT = "PROFIT";
    private string KEY_PROFIT_LEVEL_MULTIPLIER = "PROFIT_LEVEL_MULTIPLIER";
    private string KEY_COST = "COST";
    private string KEY_COST_UPDATE_PERCENTAGE = "COST_UPDATE_PERCENTAGE";
    private string KEY_TIME_TO_GENERATE_PROFIT = "TIME_TO_GENERATE_PROFIT";
    
    private void Awake()
    {
        Instance = this;
        PriceNewBusiness = priceFirstBusiness;
        
        _myProfit = initialProfit;
        _myProfitLevelMultiplier = profitLevelMultiplier;
        _myCost = initialCost;
        _myCostUpdatePercentage = costUpdatePercentage;
        _myTimeToGenerateProfit = timeToGenerateProfit;

        LoadAllBusiness();
        LoadGameData();

        if (debug)
        {
            SaveGame.Delete(SaveManager.SAVE_KEY);
            SaveGame.Delete(KEY_PRICE_NEW_BUSINESS);
            SaveGame.Delete(MoneyManager.Instance.MONEY_KEY);
            
            SaveGame.Delete(KEY_PROFIT);
            SaveGame.Delete(KEY_PROFIT_LEVEL_MULTIPLIER);
            SaveGame.Delete(KEY_COST);
            SaveGame.Delete(KEY_COST_UPDATE_PERCENTAGE);
            SaveGame.Delete(KEY_TIME_TO_GENERATE_PROFIT);

            Debug.Log("Game Data Deleted");
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
        NewBusinessBought();
        business.Bought = true;
        business.CanBuy = false;
        
        business.SetProfits(_myProfit, _myProfitLevelMultiplier);
        business.SetCosts(_myCost, _myCostUpdatePercentage);
        business.SetProfitTimes(_myTimeToGenerateProfit);
        
        CanBuyNextBusiness(business);
        UpdateValuesNewBusiness();
    }

    private void CanBuyNextBusiness(Business business)
    {
        if (business.Index + 1 > allBusiness.Length)
        {
            allBusiness[business.Index + 1].CanBuy = true;
        }
    }

    private void NewBusinessBought()
    {
        PriceNewBusiness *= priceBusinessMultiplier;
        SaveGame.Save(KEY_PRICE_NEW_BUSINESS, PriceNewBusiness);
    }

    private void LoadGameData()
    {
        if (SaveGame.Exists(KEY_PRICE_NEW_BUSINESS))
        {
            PriceNewBusiness = SaveGame.Load<int>(KEY_PRICE_NEW_BUSINESS);
        }
        
        if (SaveGame.Exists(KEY_PROFIT))
        {
            _myProfit = SaveGame.Load<int>(KEY_PROFIT);
        }
        
        if (SaveGame.Exists(KEY_PROFIT_LEVEL_MULTIPLIER))
        {
            _myProfitLevelMultiplier = SaveGame.Load<int>(KEY_PROFIT_LEVEL_MULTIPLIER);
        }
        
        if (SaveGame.Exists(KEY_COST))
        {
            _myCost = SaveGame.Load<int>(KEY_COST);
        }
        
        if (SaveGame.Exists(KEY_COST_UPDATE_PERCENTAGE))
        {
            _myCostUpdatePercentage = SaveGame.Load<int>(KEY_COST_UPDATE_PERCENTAGE);
        }
        
        if (SaveGame.Exists(KEY_TIME_TO_GENERATE_PROFIT))
        {
            _myTimeToGenerateProfit = SaveGame.Load<float>(KEY_TIME_TO_GENERATE_PROFIT);
        }
    }

    private void UpdateValuesNewBusiness()
    {
        _myProfit *= newBusinessProfitIncrement;
        _myProfitLevelMultiplier = _myProfit;
        _myCostUpdatePercentage = _myProfit * 3;
        _myTimeToGenerateProfit *= timeToGenerateProfitMultiplier;

        SaveGame.Save(KEY_PROFIT, _myProfit);
        SaveGame.Save(KEY_PROFIT_LEVEL_MULTIPLIER, _myProfitLevelMultiplier);
        SaveGame.Save(KEY_COST, _myCost);
        SaveGame.Save(KEY_COST_UPDATE_PERCENTAGE, _myCostUpdatePercentage);
        SaveGame.Save(KEY_TIME_TO_GENERATE_PROFIT, _myTimeToGenerateProfit);
    }
}