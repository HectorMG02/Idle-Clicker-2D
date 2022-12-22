using UnityEngine;

[CreateAssetMenu(fileName = "New Business", menuName = "Business")]
public class BusinessSO : ScriptableObject
{
    [Header("Sprites")] 
    public Sprite floor_1;
    public Sprite floor_2;
    public Sprite roof;

    [Header("Info")] 
    public string ID;
    public string businessName;
    public Sprite icon;

}
