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
    public static GameObject PieGuy;
    public static GameObject Oven;
    public static int currQuest = 0;
    public static int wrongOption = 0;
    public static List<int> completedQuests = new List<int>();
    public static List<string> introsPlayed = new List<string>();

    [YarnCommand("switch_project")]
    public static void SwitchProject(){
        Debug.Log("Changed project to: "+charaDialogue); //this works
        dialogueRunner.SetProject(charaDialogue);
        dialogueRunner.Stop();
    }

    // public static void GetRidOfItemsByName(string whatItem, int howMany, GameObject[] leftHand, GameObject[] rightHand){
    //     for(int i=0; i<leftHand.length; ++i){
    //         if(leftHand[i].GetComponent<ItemIdentification>().name == whatItem){
    //             leftHandInventory.Pop();
    //             if (amountOfItemsHeldLeft == 1)
    //             {
    //                 leftHand = null;
    //                 ui.Idle(controllingContainer);
    //             }
    //             else if (amountOfItemsHeldLeft > 1)
    //             {
    //                 leftHand = (GameObject)leftHandInventory.Peek();
    //             }
    //             amountOfItemsHeldLeft--;
    //             howMany -= 1;
    //         }
    //     }
    //     if(howMany != 0){
    //         foreach (GameObject item in rightHand){
    //             if(rightHand[i].GetComponent<ItemIdentification>().name == whatItem){
    //                 rightHandInventory.Pop();
    //                 if (amountOfItemsHeldRight == 1)
    //                 {
    //                     rightHand = null;
    //                     ui.Idle(controllingContainer);
    //                 }
    //                 else if (amountOfItemsHeldLeft > 1)
    //                 {
    //                     rightHand = (GameObject)rightHandInventory.Peek();
    //                 }
    //                 amountOfItemsHeldRight--;
    //                 howMany -= 1;
    //             }
    //         }
    //     }
    // }

    public static bool CheckIfQuestComplete(int whatQuest){
        // loop through leftHandInventory and rightHandInventory to grab em all in one big stack
        GameObject[] leftHand = null;
        GameObject[] rightHand = null;
        if(player.leftHandInventory.Count + player.rightHandInventory.Count == 0){
            wrongOption = 3;
            return false;
        }
        Debug.Log("Left hand object count: "+player.leftHandInventory.Count);
        if(player.leftHandInventory.Count != 0){
            leftHand = new GameObject[player.leftHandInventory.Count];
            player.leftHandInventory.CopyTo(leftHand, 0);
        }
        if(player.rightHandInventory.Count != 0){
            rightHand = new GameObject[player.rightHandInventory.Count];
            player.rightHandInventory.CopyTo(rightHand, 0);
        }
        int maxSize = player.leftHandInventory.Count + player.rightHandInventory.Count;
        GameObject[] BothHands = new GameObject[maxSize];
        for(int i=0; i<player.leftHandInventory.Count; ++i){
            BothHands[i] = leftHand[i];
        }
        for(int j=0; j<player.rightHandInventory.Count; ++j){
            BothHands[player.leftHandInventory.Count+j] = rightHand[j];
        }

        List<ItemIdentification> allItems = new List<ItemIdentification>();

        Debug.Log("maxSize is: "+maxSize);

        for(int i=0; i<maxSize; ++i){
            ItemIdentification item = BothHands[i].GetComponent<ItemIdentification>();
            allItems.Add(item);
        }

        foreach (ItemIdentification item in allItems){
            Debug.Log(item);
        }

        switch(whatQuest){
            case 1:
                bool foundCat = false;
                bool foundMouse = false;

                foreach (ItemIdentification item in allItems){
                    if(item.name == "Cat"){
                        foundCat = true;
                    }
                    if(item.name == "Mouse"){
                        foundMouse = true;
                    }
                }

                if(foundCat && foundMouse){
                    // Get rid of cat and mouse
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
                int howManyApple = 0;

                foreach (ItemIdentification item in allItems){
                    if(item.name == "Apple"){
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
                bool coldObject = false;
                bool hotObject = false;

                foreach (ItemIdentification item in allItems){
                    if(item.containsTag(ItemIdentification.ListOfPossibleTags.Hot)){
                        hotObject = true;
                    }
                    if(item.containsTag(ItemIdentification.ListOfPossibleTags.Cold)){
                        coldObject = true;
                    }
                }

                if(hotObject){
                    return true;
                }else if(coldObject){
                    wrongOption = 1;
                }else{
                    wrongOption = 2;
                }
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

    [YarnFunction("see_if_already_completed")]
    public static bool SeeIfAlreadyCompleted(int whatQuest){
        for(int i = 0; i < completedQuests.Count; ++i){
            if(completedQuests[i] == whatQuest){
                return true;
            }
        }
        return false;
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

    [YarnCommand("set_certain_quest_people_and_things_active")]
    public static void SetCertainQuestPeopleAndThingsActive(int whatQuest){
        switch(whatQuest){
            case 0:
                break;
            case 1:
                break;
            case 2:
                break;
            case 3:
                PieGuy.SetActive(true);
                Oven.SetActive(true);
                break;
        }
    }
}

