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
