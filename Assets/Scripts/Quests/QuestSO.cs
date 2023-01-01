using UnityEngine;

public enum QuestType
{
   Click,
   Improvement
}

[CreateAssetMenu(fileName = "New Quest", menuName = "Quests")]
public class QuestSO : ScriptableObject
{
   [Header("Info")]
   public string questName;
   public string id;
   public int questObjective; // the number of clicks or the number of improvements to realize
   public QuestType questType;
   
   [Header("Description")]
   [TextArea] 
   public string questDescription;

   [Header("Reward")]
   public int reward;
}
