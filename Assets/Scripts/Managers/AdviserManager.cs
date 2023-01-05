using System;
using System.Collections;
using System.Collections.Generic;
using BayatGames.SaveGameFree;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AdviserManager : Singleton<AdviserManager>
{
    [Header("Config")] [SerializeField] private GameObject advisersPanel;
    [SerializeField] private AdviserCard adviserCardPrefab;
    [SerializeField] private Transform cardsContainer;
    [SerializeField] private AdviserSO[] advisers;
    [SerializeField] private AdviserSlot[] slots;

    [Header("Buy adviser config")] [SerializeField]
    private float spinnerDuration = 2f;
    [SerializeField] private TextMeshProUGUI buyAdviserTMP;
    [SerializeField] private int priceBuyAdviser;
    
    [Header("Selected adviser info")]
    [SerializeField] private TextMeshProUGUI adviserName;
    [SerializeField] private TextMeshProUGUI adviserDescription;
    [SerializeField] private Image iconInfo;

    [Header("HUD adviser buttons")] [SerializeField]
    private Image iconButton1;
    [SerializeField] private Image iconButton2;
    [SerializeField] private Image iconButton3;


    private AdviserSlot _currentSlot;
    private bool _buyingRandom;
    private float _timeCheck;
    private List<AdviserCard> myCards = new List<AdviserCard>();


    private void OnEnable()
    {
        AdviserSlot.EventAdviserSlotClicked += OnAdviserSlotClicked;
        AdviserCard.EventCardSelected += OnAdviserCardSelected;
    }

    private void OnDisable()
    {
        AdviserSlot.EventAdviserSlotClicked -= OnAdviserSlotClicked;
        AdviserCard.EventCardSelected -= OnAdviserCardSelected;
    }

    private void OnAdviserSlotClicked(AdviserSlot selectedSlot)
    {
        _currentSlot = selectedSlot;

        if (_currentSlot.MyAdviser != null)
        {
            ShowAdviserDescription();
        }
    }

    private void OnAdviserCardSelected(AdviserCard selectedCard)
    {
        if (_currentSlot != null)
        {
            _currentSlot.SetAdviser(selectedCard);
            _currentSlot.ShowAdviserData();
            
            ShowAdviserPreviewButtons();
            ShowAdviserDescription();
            
            _currentSlot = null;
        }
    }

    private void Start()
    {
        LoadAdvisers();
        LoadSlots();
        buyAdviserTMP.text = $"${priceBuyAdviser.MoneyToText()}";
    }

    private void LoadAdvisers()
    {
        foreach (AdviserSO adviser in advisers)
        {
            AdviserCard card = Instantiate(adviserCardPrefab, cardsContainer);
            card.SetCardData(adviser);
            myCards.Add(card);
        }
    }

    private void LoadSlots()
    {
        foreach (AdviserSlot slot in slots)
        {
            foreach (AdviserSO adviser in advisers)
            {
                if (SaveGame.Exists(slot.KeyAdviserSaved))
                {
                    if (slot.AdviserSaved == adviser.adviserName)
                    {
                        slot.MyAdviser = adviser;
                        slot.ShowAdviserData();
                        ShowAdviserPreviewButtons();
                    }
                }
            }
        }
    }

    private void ShowAdviserDescription()
    {
        iconInfo.sprite = _currentSlot.MyAdviser.icon;
        adviserName.text = _currentSlot.MyAdviser.adviserName;
        adviserDescription.text = _currentSlot.MyAdviser.description;
    }
    
    public void ShowAdviserPreviewButtons()
    {
        if (slots[0].MyAdviser != null)
        {
            iconButton1.sprite = slots[0].MyAdviser.icon;
        }

        if (slots[1].MyAdviser != null)
        {
            iconButton2.sprite = slots[1].MyAdviser.icon;
        }

        if (slots[2].MyAdviser != null)
        {
            iconButton3.sprite = slots[2].MyAdviser.icon;
        }
    }

    public void BuyNewAdviser()
    {
        List<AdviserCard> availableAdvisers = GetAvailableAdvisers();

        if (availableAdvisers == null || availableAdvisers.Count == 0)
        {
            return;
        }

        if (MoneyManager.Instance.CurrentMoney >= priceBuyAdviser)
        {
            MoneyManager.Instance.RemoveMoney(priceBuyAdviser);
            _buyingRandom = true;
            StartCoroutine(IESelectRandomAdviserCard());
        }
    }

    private List<AdviserCard> GetAvailableAdvisers()
    {
        List<AdviserCard> availableAdvisers = new List<AdviserCard>();

        foreach (AdviserCard card in myCards)
        {
            AdviserSO adviserData = card.MyAdviser;

            if (!SaveGame.Exists(adviserData.KeyPurchased))
            {
                availableAdvisers.Add(card);
            }
        }

        return availableAdvisers;
    }

    public void OpenAdvisersPanel()
    {
        advisersPanel.transform.localPosition = Vector3.zero;
    }

    public void CloseAdvisersPanel()
    {
        advisersPanel.transform.localPosition = Vector3.right * 1500;
    }


    private IEnumerator IESelectRandomAdviserCard()
    {
        int index = 0;
        List<AdviserCard> availableAdvisers = GetAvailableAdvisers();

        if (availableAdvisers.Count > 1)
        {
            while (_buyingRandom)
            {
                _timeCheck += Time.deltaTime * 40f;
                availableAdvisers[index].ShowSelector();
                index++;

                if (index > availableAdvisers.Count - 1)
                {
                    index = 0;
                }


                if (_timeCheck >= spinnerDuration)
                {
                    availableAdvisers[index].UnlockCard();
                    _buyingRandom = false;
                    _timeCheck = 0;
                }

                yield return new WaitForSeconds(0.2f);
            }
        }
        else
        {
            availableAdvisers[0].UnlockCard();
            _buyingRandom = false;
        }
    }
}