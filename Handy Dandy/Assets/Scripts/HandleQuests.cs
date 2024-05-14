using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public static class HandleQuests
{
    public static InMemoryVariableStorage varStore;
    public static DialogueRunner dialogueRunner;
    public static YarnProject charaDialogue;
    public static PlayerControls player;
    public static int currQuest = 0;
    public static List<int> completedQuests = new List<int>();
    public static bool questCompleted = false;
    public static bool alreadyCompleted = false;

    [YarnCommand("switch_project")]
    public static void SwitchProject(){
        Debug.Log("Changed project to: "+charaDialogue); //this works
        dialogueRunner.SetProject(charaDialogue);
    }

    public static bool CheckIfQuestComplete(int whatQuest){
        // check left hand first

        // now check right hand
        return true;
    }

    [YarnCommand("check_quest_completion")]
    public static void CheckQuestCompletion(){
        if(CheckIfQuestComplete(currQuest)){
            questCompleted = true;
            completedQuests.Add(currQuest);
        }
    }

    [YarnCommand("start_quest")]
    public static void StartQuest(int whatQuest){
        currQuest = whatQuest;
    }

    [YarnCommand("see_if_already_completed")]
    public static void SeeIfAlreadyCompleted(int whatQuest){
        for(int i = 0; i < completedQuests.Count; ++i){
            if(completedQuests[i] == whatQuest){
                alreadyCompleted = true;
            }
        }
    }
}

