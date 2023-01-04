using System.Collections;
using BayatGames.SaveGameFree;
using UnityEngine;
using UnityEngine.UI;

public class AdviserCard : MonoBehaviour
{
   [Header("UI")] 
   [SerializeField] private Image icon;
   [SerializeField] private Image selectorImage;
   public AdviserSO MyAdviser { get; set; }

   private Button _button;

   private void Awake()
   {
      _button = GetComponent<Button>();
   }

   public void SetCardData(AdviserSO adviserData)
   {
      MyAdviser = adviserData;
      icon.sprite = MyAdviser.icon;

     _button.interactable = SaveGame.Exists(MyAdviser.KeyPurchased);
   }


   public void UnlockCard()
   {
      _button.interactable = true;
      MyAdviser.Purchased = true;
   }

   public void ShowSelector()
   {
      StartCoroutine(IESelector());
   }


   private IEnumerator IESelector()
   {
      selectorImage.enabled = true;
      yield return new WaitForSeconds(0.2f);
      selectorImage.enabled = false;
   }
   
}
