using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdviserManager : Singleton<AdviserManager>
{
    [SerializeField] private GameObject advisersPanel;
    [SerializeField] private AdviserCard adviserCardPrefab;
    [SerializeField] private Transform cardsContainer;
    [SerializeField] private AdviserSO[] advisers;

    private List<AdviserCard> myCards = new List<AdviserCard>();

    private void Start()
    {
        LoadAdvisers();
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

    public void OpenAdvisersPanel()
    {
        advisersPanel.transform.localPosition = Vector3.zero;
    }
    
    public void CloseAdvisersPanel()
    {
        advisersPanel.transform.localPosition = Vector3.right * 1500;
    }
}
