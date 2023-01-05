using BayatGames.SaveGameFree;
using UnityEngine;

public class InitialBusiness : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private int initialProfit = 1; 
    [SerializeField] private int buyBusinessProfitMultiplier = 15;
    [SerializeField] private int milestoneProfitMultiplier = 2;

    [Header("VFX")]
    [SerializeField] private Transform positionProfitText;
    
    private int _profit;
    private int _profitTemp;

    private string KEY_INITIAL_PROFIT = "INITIAL_PROFIT";

    private void OnEnable()
    {
        BusinessUI.EventBusinessBought += OnBusinessBought;
        Business.EventNewMilestone += OnNewMilestone;
    }
    
    private void OnDisable()
    {
        BusinessUI.EventBusinessBought -= OnBusinessBought;
        Business.EventNewMilestone -= OnNewMilestone;
    }
    
    
    private void OnBusinessBought(Business businessBought)
    {
        if (GameManager.Instance.BusinessesPurchased > 1)
        {
            _profit *= buyBusinessProfitMultiplier;
            SaveGame.Save(KEY_INITIAL_PROFIT, _profit);
        }
    }
    
    private void OnNewMilestone(Business business)
    {
        _profit += milestoneProfitMultiplier * business.Milestones;
        SaveGame.Save(KEY_INITIAL_PROFIT, _profit);
    }

    private void Start()
    {
        LoadData();
    }

    private void LoadData()
    {
        _profit = SaveGame.Exists(KEY_INITIAL_PROFIT) ? SaveGame.Load<int>(KEY_INITIAL_PROFIT) : initialProfit;
    }
    
    public void GetProfit()
    {
        _profitTemp = _profit;
        
        float utilityClick = AdviserManager.Instance.GetAdviserProfit(AdviserType.ClickUtility);
        if (utilityClick != 0f)
        {
            _profitTemp *= (int) utilityClick;
        }
        
        MoneyManager.Instance.AddMoney(_profitTemp);
        VFXManager.Instance.ShowText(positionProfitText, $"+ ${_profitTemp.MoneyToText()}");
        QuestsManager.Instance.AddProgress("initialBusiness");
    }
}
