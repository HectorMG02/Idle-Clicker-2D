using System;
using System.Collections;
using System.Collections.Generic;
using BayatGames.SaveGameFree;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class OfflineEarningsManager : MonoBehaviour
{
    [SerializeField] private int pricePerSecond = 2; 
    [SerializeField] private GameObject offlineEarningPanel;
    [SerializeField] private TextMeshProUGUI earningTMP;
    [SerializeField] private TextMeshProUGUI absenceTimeTMP;
    
    public int TotalSeconds { get; set; }
    public string AbsenceTime { get; set; }

    private int _offlineEarnings;
    private string KEY_EARNINGS = "EARNINGS";


    private void Start()
    {
        CalculateOfflineTime();
        CalculateOfflineEarnings();
        ShowOfflineEarnings();
    }

    private void CalculateOfflineEarnings()
    {
        if (TotalSeconds > 0)
        {
            _offlineEarnings = TotalSeconds * pricePerSecond;
        }
    }

    public void CollectOfflineEarnings()
    {
        offlineEarningPanel.SetActive(false);
        MoneyManager.Instance.AddMoney(_offlineEarnings);
        TotalSeconds = 0;
        _offlineEarnings = 0;
        SaveGame.Delete(KEY_EARNINGS);
    }

    private void ShowOfflineEarnings()
    {
        if (TotalSeconds > 0)
        {
            offlineEarningPanel.SetActive(true);
            earningTMP.text = $"$ {_offlineEarnings.MoneyToText()}";
            absenceTimeTMP.text = AbsenceTime;
        }
    }

    private void CalculateOfflineTime()
    {
        if (SaveGame.Exists(KEY_EARNINGS))
        {
            string time = SaveGame.Load<string>(KEY_EARNINGS);

            if (!string.IsNullOrEmpty(time))
            {
                DateTime timeSaved = DateTime.FromBinary(Convert.ToInt64(time));
                TimeSpan difference = DateTime.Now.Subtract(timeSaved);

                TotalSeconds = Mathf.CeilToInt( (float) difference.TotalSeconds);
                AbsenceTime = $"{difference.Hours:00}:{difference.Minutes:00}:{difference.Seconds:00}";
            }
        }
    }
    
    private void SaveGameTime()
    {
        SaveGame.Save(KEY_EARNINGS, DateTime.Now.ToBinary().ToString());
    }


    private void OnApplicationQuit()
    {
        SaveGameTime();
    }
}
