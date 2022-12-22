using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currentMoneyTMP;

    private void Update()
    {
        currentMoneyTMP.text = $"${MoneyManager.Instance.CurrentMoney}";
    }
}
