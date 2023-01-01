using UnityEngine;

public class VFXManager: Singleton<VFXManager>
{
    private Pooler _pooler;
 
    private void Start()
    {
        _pooler = GetComponent<Pooler>();
    }

    public void ShowText(Transform positionToShow, string text)
    {
        GameObject instance = _pooler.GetPoolerInstance();
        TextVFX textVFX = instance.GetComponent<TextVFX>();
         
        textVFX.ShowText(text, _pooler.PoolerParent);
        
        instance.transform.SetParent(positionToShow);
        instance.transform.position = positionToShow.position;
        instance.SetActive(true);
    }
}