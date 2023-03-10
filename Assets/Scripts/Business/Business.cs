using System;
using UnityEngine;

public class Business : MonoBehaviour
{
    public static event Action<Business> EventNewMilestone;
    
    [Header("Config")] [SerializeField] private int index;
    
    [Header("VFX")]
    [SerializeField] private Transform positionProfitText;
    
    
    public int Milestones { get; private set; }
    public int Level { get; private set; }
    public int Index => index;
    public bool Bought { get; set; }
    public bool CanBuy { get; set; }
    public int Profit { get; private set; }
    public int ProfitToAddAfterLevelUp { get; private set; }
    public int UpdatePrice { get; private set; }
    public int CostUpdatePercentage { get; private set; }
    public float TimeToGenerateProfit { get; private set; }
    public bool NextLevelIsNewMilestone => (Level + 1) % 25 == 0;
    public bool CanHideTimer => TimeToGenerateProfit <= 1;

    private float _timer;
    private float _timeMinutes;
    private float _timeSeconds;

    private Business _myBusiness;

    private void Awake()
    {
        _myBusiness = GetComponent<Business>();
    }

    private void Start()
    {
        // we use because if not the money updates after start the game
        if (TimeToGenerateProfit != 0)
        {
            _timer = TimeToGenerateProfit;
        }
    }

    private void Update()
    {
        if (Bought == false)
        {
            return;
        }

        GenerateProfits();
    }

    public void LoadBusinessData(BusinessData businessData)
    {
        Level = businessData.Level;
        Milestones = businessData.Milestones;
        Bought = businessData.Bought;
        CanBuy = businessData.CanBuy;
        TimeToGenerateProfit = businessData.TimeToGenerateProfit;
        Profit = businessData.Profit;
        ProfitToAddAfterLevelUp = businessData.ProfitToAdd;
        
        UpdatePrice = businessData.CostUpdate;
        CostUpdatePercentage = businessData.CostUpdatePercentage;

        if (Bought)
        {
            BusinessUI[] businessUIs = FindObjectsOfType<BusinessUI>();
            
            foreach (BusinessUI businessUI in businessUIs)
            {
                businessUI.ResponseBusinessBought(this);
            }
            
        }
    }

    public void SetProfits(int profit, int profitLevelIncrement)
    {
        this.Profit = profit;
        this.ProfitToAddAfterLevelUp = profitLevelIncrement;
    }

    public void SetCosts(int costUpdate, int costUpdatePercent)
    {
        UpdatePrice = costUpdate;
        CostUpdatePercentage = costUpdatePercent;
    }

    public void SetProfitTimes(float timeToGenerateProfit)
    {
        TimeToGenerateProfit = timeToGenerateProfit;
    }

    public void SetTimes(float profitTime)
    {
        TimeToGenerateProfit = profitTime;
        _timer = TimeToGenerateProfit;
    }

    private void GenerateProfits()
    {
        _timer -= Time.deltaTime;
        _timeMinutes = Mathf.Floor(_timer / 60);
        _timeSeconds = _timer % 60;

        if (_timeMinutes < 0f)
        {
            _timeMinutes = 0f;
            _timeSeconds = 0f;
            _timer = TimeToGenerateProfit;

            MoneyManager.Instance.AddMoney(GetProfit());
            VFXManager.Instance.ShowText(positionProfitText, $"+ ${GetProfit().MoneyToText()}");
        }
    }

    public int GetProfit()
    {
        int profitTemp = Profit;
        float businessUtility = AdviserManager.Instance.GetAdviserProfit(AdviserType.BusinessUtility);

        if (businessUtility != 0f)
        {
            profitTemp *= (int) businessUtility;
        }

        return profitTemp;
    }

    public string GetTimer()
    {
        return $"{_timeMinutes:00}:{_timeSeconds:00}";
    }

    public float GetValueLevelBar()
    {
        int percentage = Level / 25;
        return (Level - (25f * percentage)) / 25f;
    }

    public float GetProfitBarValue()
    {
        if (Milestones < 3)
        {
            return (TimeToGenerateProfit - _timer) / TimeToGenerateProfit;
        }

        return 1f;
    }


    public void UpdateBusiness()
    {
        Level++;
        Profit += ProfitToAddAfterLevelUp;

        if (Level % 25 == 0)
        {
           NextMilestone();
        }
        else
        {
            int extraCost = Mathf.CeilToInt(UpdatePrice * (CostUpdatePercentage / 100f));
            UpdatePrice += extraCost;
        }

        if (Milestones > 2) // the same for milestone 3, 4, 5, etc
        {
            Profit *= 4;
            ProfitToAddAfterLevelUp *= 2;
        }
    }

    private void NextMilestone()
    {
        Milestones++;
        UpdatePrice *= 2;

        if (TimeToGenerateProfit > 1)
        {
            TimeToGenerateProfit /= 2;
        }
        
        EventNewMilestone?.Invoke(this);
    }

    public int GetUpdatePrice()
    {
        int milestonePrice = UpdatePrice * 15;
        int newUpdatePrice = UpdatePrice;

        float discountUpdate = AdviserManager.Instance.GetAdviserProfit(AdviserType.UpdateDiscountUtility);
        float discountMilestone = AdviserManager.Instance.GetAdviserProfit(AdviserType.MilestoneDiscountUtility);

        if (discountUpdate != 0f)
        {
            newUpdatePrice -= Mathf.CeilToInt(newUpdatePrice * discountUpdate);
        }
        
        if (discountMilestone != 0f)
        {
            milestonePrice -= Mathf.CeilToInt(milestonePrice * discountMilestone);
        }
        
        return NextLevelIsNewMilestone ? milestonePrice : newUpdatePrice;
    }
    
}