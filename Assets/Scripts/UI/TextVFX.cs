using TMPro;
using TreeEditor;
using UnityEngine;

public class TextVFX: MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    private Transform _poolerParent;
    
    public void ShowText(string textToShow, Transform parent)
    {
        text.text = textToShow;
        _poolerParent = parent;
    }

    public void FinishAnimation()
    {
        gameObject.SetActive(false);
        transform.SetParent(_poolerParent);
    }
}