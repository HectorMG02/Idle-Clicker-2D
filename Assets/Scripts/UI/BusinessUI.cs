using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class BusinessUI : MonoBehaviour
{
    public static event Action<Business> EventBusinessBought;
    
    [Header("Config")] [SerializeField] private BusinessSO businessSo;

    [Header("Sprites")] [SerializeField] private SpriteRenderer floor_1;
    [SerializeField] private SpriteRenderer floor_2;
    [SerializeField] private SpriteRenderer roof;
    
    [Header("Buy panel")] [SerializeField] private GameObject buyPanel;
    [SerializeField] private Image businessIcon;
    [SerializeField] private TextMeshProUGUI nameToBuyTMP;
    [SerializeField] private TextMeshProUGUI priceToBuyTMP;

    [Header("Profits Panel")]
    [SerializeField] private GameObject profitsPanel;
    [SerializeField] private Image businessImage;
    [SerializeField] private TextMeshProUGUI timerTMP;
    [SerializeField] private TextMeshProUGUI businessNameTMP;
    [SerializeField] private TextMeshProUGUI costUpdateTMP;
    [SerializeField] private Button updateButton; 
    [SerializeField] private GameObject timerContainer;
    [SerializeField] private Sprite updateNormalLevelSprite; 
    [SerializeField] private Sprite updateNewMilestoneLevelSprite;

    [Header("Profit Progress Bar")]
    [SerializeField] private Image profitBar;
    [SerializeField] private TextMeshProUGUI profitTMP;
    
    [Header("Level Progress Bar")]
    [SerializeField] private Image levelBar;
    [SerializeField] private TextMeshProUGUI levelTMP;
    
    
    private Business _myBusiness;


    private void OnEnable()
    {
        EventBusinessBought += ResponseBusinessBought;
        Business.EventNewMilestone += ResponseNewMilestone;
    }

    private void OnDisable()
    {
        EventBusinessBought -= ResponseBusinessBought;
        Business.EventNewMilestone -= ResponseNewMilestone;
    }

    private void ResponseBusinessBought(Business businessBought)
    {
        if (businessBought.Index + 1 == _myBusiness.Index)
        {
            ActiveBuyPanel(true);
        }
    }

    private void ResponseNewMilestone(Business business)
    {
        if (_myBusiness == business)
        {
            if (_myBusiness.CanHideTimer)
            {
                timerContainer.SetActive(false);
            }
        }
    }
    
    private void Awake()
    {
        _myBusiness = GetComponent<Business>();
    }

    private void Start()
    {
        ShowBuyInformation();
        LoadBusinessInformation();
    }

    private void Update()
    {
        if (businessSo == null)
        {
            return;
        }
        
        UpdateProfitValues();
        UpdateBusinessUpdateButtons();
    }

    private void LoadBusinessInformation()
    {
        floor_1.sprite = businessSo.floor_1;
        floor_2.sprite = businessSo.floor_2;
        roof.sprite = businessSo.roof;

        businessIcon.sprite = businessSo.icon;
        nameToBuyTMP.text = businessSo.businessName;
        
        businessImage.sprite = businessSo.icon;
        businessNameTMP.text = businessSo.businessName;
    } 

    private void ShowBuyInformation()
    {
        if (_myBusiness.Bought)
        {
            ActiveBuyPanel(false);
            ActiveBusinessDataPanel(true);

            if (_myBusiness.CanHideTimer)
            {
                timerContainer.SetActive(false);
            }
        }
        else
        {
            if (_myBusiness.Index == 0)
            {
                ActiveBuyPanel(true);
            }

            if (_myBusiness.CanBuy)
            {
                ActiveBuyPanel(true);
                ActiveBusinessDataPanel(false);
            }
            
        }
    }

    private void UpdateProfitValues()
    {
        if (!_myBusiness.Bought)
        {
            return;
        }

        timerTMP.text = _myBusiness.GetTimer();
        profitBar.fillAmount = _myBusiness.GetProfitBarValue();
        costUpdateTMP.text = $"Update \n ${_myBusiness.GetUpdateCost()}";
        
        levelBar.fillAmount = _myBusiness.GetValueLevelBar();
        levelTMP.text = _myBusiness.Level.ToString();
        
        if (_myBusiness.TimeToGenerateProfit > 1)
        {
            profitTMP.text = $"${_myBusiness.Profit}";
        }
        else
        {
            profitTMP.text = $"${_myBusiness.Profit}/s";
        }
    }
    
    private void ActiveBuyPanel(bool active)
    {
        buyPanel.SetActive(active);

        if (active)
        {
            UpdateBuyPanel();
        }
    }

    private void UpdateBuyPanel()
    {
        int price = GameManager.Instance.PriceNewBusiness;
        priceToBuyTMP.text = $"Buy \n ${price}";
    }

    public void BuyBusiness()
    {
        int currentMoney = MoneyManager.Instance.CurrentMoney;
        int priceNewBusiness = GameManager.Instance.PriceNewBusiness;

        if (currentMoney >= priceNewBusiness)
        {
            ActiveBuyPanel(false);
            ActiveBusinessDataPanel(true);
            
            GameManager.Instance.SetNewBusiness(_myBusiness);
            MoneyManager.Instance.RemoveMoney(priceNewBusiness);
            SaveManager.SaveAllBusiness();
            
            EventBusinessBought?.Invoke(_myBusiness);
        }
    }

    private void ActiveBusinessDataPanel(bool active)
    {
        profitsPanel.SetActive(active);
    }

    public void UpdateBusiness()
    {
        int currentMoney = MoneyManager.Instance.CurrentMoney;
        int updateCost = _myBusiness.GetUpdateCost();

        if (currentMoney >= updateCost)
        {
            MoneyManager.Instance.RemoveMoney(updateCost);
            _myBusiness.UpdateBusiness();
            SaveManager.SaveAllBusiness();
        }
    }

    public void UpdateBusinessUpdateButtons()
    {
        if (_myBusiness.NextLevelIsNewMilestone)
        {
            updateButton.GetComponent<Image>().sprite = updateNewMilestoneLevelSprite;
        }
        else
        {
            updateButton.GetComponent<Image>().sprite = updateNormalLevelSprite;
        }
    }
    
}