using System;
using BayatGames.SaveGameFree;
using UnityEngine;
using UnityEngine.Serialization;

public enum QuestType
{
   Click,
   Improvement
}

[CreateAssetMenu(fileName = "New Quest", menuName = "Quests")]
public class QuestSO : ScriptableObject
{
   public static event Action<QuestSO> EventQuestCompleted;
   
   [Header("Info")]
   public string questName;
   public string id; 
   public int questGoal; // the number of clicks or the number of improvements to realize
   public QuestType questType;
   
   [Header("Description")]
   [TextArea] 
   public string questDescription;

   [Header("Reward")]
   public int reward;

   
   
   public bool QuestCollected
   {
      get => SaveGame.Load<bool>(CollectedKey);
      set => SaveGame.Save(CollectedKey, value);
   }

   public int CurrentProgress
   {
      get => SaveGame.Load<int>(ProgressKey);
      set => SaveGame.Save(ProgressKey, value);
   }
   

   public string CompletedKey => QUEST_COMPLETED + id;
   public string CollectedKey => QUEST + id;
   public string ProgressKey => PROGRESS + id;
   
   private string QUEST_COMPLETED = "COMPLETED";
   private string QUEST = "QUEST";
   private string PROGRESS = "PROGRESS";

   public void UpdateQuest()
   {
      CurrentProgress++;
      CompleteQuest();
   }

   public void CompleteQuest()
   {
      if (SaveGame.Exists(ProgressKey))
      {
         if(CurrentProgress >= questGoal)
         {
            Debug.Log("completed");
            SaveGame.Save(CompletedKey, true);
            EventQuestCompleted?.Invoke(this);
            CurrentProgress = questGoal;
         }
      }
   }


   public bool CheckQuestIsCollected()
   {
      if (SaveGame.Exists(CollectedKey))
      {
         return QuestCollected;
      }

      return false;
   }
}
