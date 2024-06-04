using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestUI : MonoBehaviour

{
    public GameObject questUI;
    public TMP_Text questText;
    int currQuest = HandleQuests.currQuest;
    public List<string> currQuestInfo;
    bool questUIOn = HandleQuests.questUIToggle;

    void Start() {
        currQuestInfo.Add("Help Tie Guy find his Cat and Mouse!");
        currQuestInfo.Add("Bring three apples to Tie Guy. Don't forget about the basket!");
        currQuestInfo.Add("Bring Pie Guy something HOT");
    }

    void Update() {
        questBox();
    }

    public void questBox() {
        currQuest = HandleQuests.currQuest;
        questUIOn = HandleQuests.questUIToggle;
        
        if ((currQuest > 0) && (questUIOn)) {
            Debug.Log("currQuest should pop up");
            questUI.SetActive(true);
            //currquest is not 0 indexed!! booooo!!
            questText.text = currQuestInfo[currQuest - 1];
        }
        else {
            questUI.SetActive(false);
        }
    }
}
