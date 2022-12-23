using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BusinessUI : MonoBehaviour
{
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

    [Header("Profit Progress Bar")]
    [SerializeField] private Image profitBar;
    [SerializeField] private TextMeshProUGUI profitTMP;
    
    private Business _myBusiness;
    
    public static event Action<Business> EventBusinessBought;


    private void OnEnable()
    {
        EventBusinessBought += ResponseBusinessBought;
    }

    private void OnDisable()
    {
        EventBusinessBought -= ResponseBusinessBought;
    }

    private void ResponseBusinessBought(Business businessBought)
    {
        if (businessBought.Index + 1 == _myBusiness.Index)
        {
            ActiveBuyPanel(true);
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
            SaveManager.SaveBusiness();
            
            EventBusinessBought?.Invoke(_myBusiness);
        }
    }

    private void ActiveBusinessDataPanel(bool active)
    {
        profitsPanel.SetActive(active);
    }
}