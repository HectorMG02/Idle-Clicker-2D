using System.Collections.Generic;
using BayatGames.SaveGameFree;
using UnityEngine;

public class QuestsManager : Singleton<QuestsManager>
{
    [Header("Quests")]
    [SerializeField] private QuestSO[] quests;
    
    [Header("Config")]
    [SerializeField] private GameObject questsPanel;
    [SerializeField] private Transform panelContent;
    [SerializeField] private QuestCard questCardPrefab;
    
    private List<QuestCard> myQuestCards = new List<QuestCard>();


    private void Start()
    {
        LoadQuests();
    }


    private void LoadQuests()
    {
        foreach (QuestSO quest in quests)
        {
            if (SaveGame.Exists(quest.CollectedKey))
            {
                if (quest.QuestCollected)
                {
                    continue;
                }
            }
            
            QuestCard newQuest = Instantiate(questCardPrefab, panelContent);
            newQuest.SetCardData(quest);
            myQuestCards.Add(newQuest);
        }
    }


    public void OpenQuestsPanel()
    {
        questsPanel.transform.localPosition = Vector3.zero;
    }
    
    public void CloseQuestsPanel()
    {
        questsPanel.transform.localPosition = Vector3.right * -1500;
    }


    public void AddProgress(string questId)
    {
        QuestSO quest = GetQuest(questId);
        
        if (quest != null)
        {
            if (!quest.CheckQuestIsCollected())
            {
                quest.UpdateQuest();
            }
        }
    }

    private QuestSO GetQuest(string questId)
    {
        foreach (QuestCard questCard in myQuestCards)
        {
            if (questCard.MyQuest.id == questId)
            {
                return questCard.MyQuest;
            }
        }

        return null;
    }
}
