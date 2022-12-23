using UnityEngine;

public class Business : MonoBehaviour
{
    [Header("Config")] [SerializeField] private int index;

    public int Milestones { get; set; }
    public int Index => index;
    public bool Bought { get; set; }
    public bool CanBuy { get; set; }
    public int Profit { get; private set; }
    public int ProfitToAdd { get; private set; }
    public int CostUpdate { get; private set; }
    public int CostUpdatePercentage { get; private set; }
    public float TimeToGenerateProfit { get; private set; }

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
        Milestones = businessData.Milestones;
        Bought = businessData.Bought;
        CanBuy = businessData.CanBuy;
        TimeToGenerateProfit = businessData.TimeToGenerateProfit;
        Profit = businessData.Profit;
        ProfitToAdd = businessData.ProfitToAdd;
    }

    public void SetProfits(int profit, int profitLevelIncrement)
    {
        this.Profit = profit;
        this.ProfitToAdd = profitLevelIncrement;
    }

    public void SetCosts(int costUpdate, int costUpdatePercent)
    {
        CostUpdate = costUpdate;
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

            MoneyManager.Instance.AddMoney(Profit);
        }
    }

    public string GetTimer()
    {
        return $"{_timeMinutes:00}:{_timeSeconds:00}";
    }

    public float GetProfitBarValue()
    {
        if (Milestones < 3)
        {
            return (TimeToGenerateProfit - _timer) / TimeToGenerateProfit;
        }

        return 1f;
    }
}