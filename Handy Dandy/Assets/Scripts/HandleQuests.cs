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
    public static int wrongOption = 0;
    public static List<int> completedQuests = new List<int>();
    public static List<string> introsPlayed = new List<string>();
    public static bool alreadyCompleted = false;

    [YarnCommand("switch_project")]
    public static void SwitchProject(){
        Debug.Log("Changed project to: "+charaDialogue); //this works
        dialogueRunner.SetProject(charaDialogue);
        dialogueRunner.Stop();
    }

    public static bool CheckIfQuestComplete(int whatQuest){
        // loop through leftHandInventory and rightHandInventory to grab em all in one big stack
        GameObject[] leftHand = new GameObject[player.leftHandInventory.Count];
        player.leftHandInventory.CopyTo(leftHand, player.leftHandInventory.Count);
        GameObject[] rightHand = new GameObject[player.rightHandInventory.Count];
        player.rightHandInventory.CopyTo(rightHand, player.rightHandInventory.Count);
        int maxSize = player.leftHandInventory.Count + player.rightHandInventory.Count;
        GameObject[] BothHands = new GameObject[maxSize];
        for(int i=0; i<player.leftHandInventory.Count; ++i){
            BothHands[i] = leftHand[i];
        }
        for(int j=0; j<player.rightHandInventory.Count; ++j){
            BothHands[player.leftHandInventory.Count+j] = rightHand[j];
        }

        List<UIManager.Item> allItems = new List<UIManager.Item>();

        Debug.Log("maxSize is: "+maxSize);

        for(int i=0; i<maxSize; ++i){
            ItemIdentification itemDesc = BothHands[i].GetComponent<ItemIdentification>();
            int itemID = itemDesc.id;
            UIManager.Item item = (UIManager.Item) itemID;
            allItems.Add(item);
            Debug.Log("HELLO???");
        }

        foreach (UIManager.Item item in allItems){
            Debug.Log(item);
        }

        switch(whatQuest){
            case 1:
                bool foundCat = false;
                bool foundMouse = false;

                foreach (UIManager.Item item in allItems){
                    if(item == UIManager.Item.Cat){
                        foundCat = true;
                    }
                    if(item == UIManager.Item.Mouse){
                        foundMouse = true;
                    }
                }

                if(foundCat && foundMouse){
                    return true;
                }else if(foundCat){
                    wrongOption = 1;
                }else if(foundMouse){
                    wrongOption = 2;
                }else{
                    wrongOption = 3;
                }
                break;
            case 2:
                bool oneApple = false;
                bool twoApple = false;
                int howManyApple = 0;

                foreach (UIManager.Item item in allItems){
                    if(item == UIManager.Item.Apple){
                        howManyApple++;
                    }
                }

                if(howManyApple == 3){
                    return true;
                }else if(howManyApple == 1){
                    wrongOption = 1;
                }else if(howManyApple == 2){
                    wrongOption = 2;
                }else{
                    wrongOption = 3;
                }
                break;
            case 3:
                break;
            case 0:
                break;
        }
        return false;
    }

    [YarnFunction("check_quest_completion")]
    public static bool CheckQuestCompletion(){
        if(CheckIfQuestComplete(currQuest)){
            completedQuests.Add(currQuest);
            return true;
        }
        return false;
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

    [YarnCommand("set_intro_done")]
    public static void SetIntroDone(string whatIntro){
        introsPlayed.Add(whatIntro);
    }

    [YarnFunction("get_curr_quest")]
    public static int GetCurrQuest(){
        return currQuest;
    }

    [YarnFunction("get_wrong_option")]
    public static int GetWrongOption(){
        return wrongOption;
    }

    [YarnFunction("see_if_already_played_intro")]
    public static bool SeeIfAlreadyPlayedIntro(string whatIntro){
        for(int i=0; i<introsPlayed.Count; ++i){
            if(introsPlayed[i] == whatIntro){
                return true;
            }
        }
        return false;
    }
}

