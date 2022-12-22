using UnityEngine;

public class Business : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private int index;

    public int Index => index;
    public bool Bought { get; set; }
    public bool CanBuy { get; set; }

    public void LoadBusinessData(BusinessData businessData)
    {
        Bought = businessData.bought;
        CanBuy = businessData.canBuy;
    }
}