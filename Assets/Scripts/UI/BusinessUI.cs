using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
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


    [Header("Business Panel")] [SerializeField]
    private GameObject businessDataPanel;

    private Business _myBusiness;

    private void Awake()
    {
        _myBusiness = GetComponent<Business>();
    }

    private void Start()
    {
        ShowBuyInformation();
        LoadBusinessInformation();
    }

    private void LoadBusinessInformation()
    {
        floor_1.sprite = businessSo.floor_1;
        floor_2.sprite = businessSo.floor_2;
        roof.sprite = businessSo.roof;

        businessIcon.sprite = businessSo.icon;
        nameToBuyTMP.text = businessSo.businessName;
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

            SaveManager.SaveBusiness();
        }
    }

    private void ActiveBusinessDataPanel(bool active)
    {
        businessDataPanel.SetActive(active);
    }
}