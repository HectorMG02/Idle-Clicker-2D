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
        CloseQuestsPanel();
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
        questsPanel.transform.position = new Vector3(0, 0, 0);
    }
    
    public void CloseQuestsPanel()
    {
        questsPanel.transform.position = new Vector3(-15, 0, 0); 
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
