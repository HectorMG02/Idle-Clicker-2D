using System;
using BayatGames.SaveGameFree;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestCard : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI questTitleTMP;
    [SerializeField] private TextMeshProUGUI questDescriptionTMP;
    [SerializeField] private TextMeshProUGUI questRewardTMP;
    [SerializeField] private TextMeshProUGUI questProgressTMP;

    [SerializeField] private Image progressBarImage;
    [SerializeField] private GameObject collectButton;

    public QuestSO MyQuest { get; set; }

    private void OnEnable()
    {
        QuestSO.EventQuestCompleted += QuestCompleted;
    }
    
    private void OnDisable()
    {
        QuestSO.EventQuestCompleted -= QuestCompleted;
    }

    private void QuestCompleted(QuestSO questCompleted)
    {
        if (MyQuest == questCompleted)
        {
            OnQuestCompleted();
        }
    }

    private void Start()
    {
        if (SaveGame.Exists(MyQuest.ProgressKey) == false)
        {
            MyQuest.CurrentProgress = MyQuest.questType == QuestType.Click ? 0 : 1;
        }
    }

    private void Update()
    {
        int currentProgress = MyQuest.CurrentProgress;
        int questGoal = MyQuest.questGoal;

        questProgressTMP.text = $"{currentProgress} / {questGoal}";
        progressBarImage.fillAmount = (float) currentProgress / questGoal;
    }

    public void SetCardData(QuestSO quest)
    {
        MyQuest = quest;
        questTitleTMP.text = quest.questName;
        questDescriptionTMP.text = quest.questDescription;
        questRewardTMP.text = quest.reward.MoneyToText();
        
        
        if (SaveGame.Exists(MyQuest.CompletedKey))
        {
            OnQuestCompleted();
        }
    }

    private void OnQuestCompleted()
    {
        questDescriptionTMP.gameObject.SetActive(false);
        collectButton.SetActive(true);
    }

    public void CollectReward()
    {
        MoneyManager.Instance.AddMoney(MyQuest.reward);
        MyQuest.QuestCollected = true;
        gameObject.SetActive(false);
    }
}