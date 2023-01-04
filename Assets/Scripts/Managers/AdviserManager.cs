using System.Collections;
using System.Collections.Generic;
using BayatGames.SaveGameFree;
using TMPro;
using UnityEngine;

public class AdviserManager : Singleton<AdviserManager>
{
    [Header("Config")]
    [SerializeField] private GameObject advisersPanel;
    [SerializeField] private AdviserCard adviserCardPrefab;
    [SerializeField] private Transform cardsContainer;
    [SerializeField] private AdviserSO[] advisers;

    [SerializeField] private float spinnerDuration = 2f;
    [SerializeField] private TextMeshProUGUI buyAdviserTMP;
    [SerializeField] private int priceBuyAdviser;

    private bool _buyingRandom;
    private float _timeCheck;
    private List<AdviserCard> myCards = new List<AdviserCard>();

    private void Start()
    {
        LoadAdvisers();
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