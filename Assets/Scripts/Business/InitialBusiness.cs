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
        MoneyManager.Instance.AddMoney(_profit);
    }
}
