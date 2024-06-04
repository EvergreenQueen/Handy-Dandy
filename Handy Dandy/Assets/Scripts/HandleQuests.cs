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
    public static GameObject Pie;
    public static int currQuest = 0;
    public static int wrongOption = 0;
    public static List<int> completedQuests = new List<int>();
    public static List<string> introsPlayed = new List<string>();
    public static bool questUIToggle = false;

    //public static AudioSource audioSource;

    [YarnCommand("switch_project")]
    public static void SwitchProject(){
        Debug.Log("Changed project to: "+charaDialogue); //this works
        dialogueRunner.SetProject(charaDialogue);
        dialogueRunner.Stop();
    }

    public static void GetRidOfItemsByName(string whatItem, int howMany, GameObject[] leftHand, GameObject[] rightHand, bool delete){
        // ok so right now, we have arrays lefthand and righthand
        // convert to list so that we can actually get rid of items in a way that's good
        // and then convert it back into an array so we can convert it into a stack
        // so that we can replace both rightHandInventory and leftHandInventory
        // ezpz
        List<GameObject> leftList = new List<GameObject>();
        leftList.AddRange(leftHand);
        List<GameObject> rightList = new List<GameObject>();
        rightList.AddRange(rightHand);

        for(int i=0; i<leftList.Count; i++){
            if((leftList[i].GetComponent<ItemIdentification>().name == whatItem) && (howMany > 0)){
                GameObject temp = leftList[i];
                leftList.RemoveAt(i);
                i--;
                howMany -= 1;
                player.leftHandInventory = new Stack(leftList);
                Debug.Log(player.leftHandInventory);
                player.DropUpdateUI(true, temp);
                if (delete) temp.SetActive(false);
            }
        }

        Debug.Log("Right List Count is: " + rightList.Count);

        foreach(GameObject a in rightList){
            Debug.Log("This is in rightList: "+a.name);
        }

        for(int i=0; i<rightList.Count; i++){
            Debug.Log("Current item: "+rightList[i].name);
            if((rightList[i].GetComponent<ItemIdentification>().name == whatItem) && (howMany > 0)){
                GameObject temp = rightList[i];
                rightList.RemoveAt(i);
                i--;
                howMany -= 1;
                Debug.Log("How Many? " + howMany);
                // Debug.Log("Am I dum");
                player.rightHandInventory = new Stack(rightList);
                Debug.Log(player.rightHandInventory);
                player.DropUpdateUI(false, temp);
                if (delete) temp.SetActive(false);
            }
        }
    }

    public static void GetRidOfItemsByTag(ItemIdentification.ListOfPossibleTags whatItem, int howMany, GameObject[] leftHand, GameObject[] rightHand, bool delete){
        // ok so right now, we have arrays lefthand and righthand
        // convert to list so that we can actually get rid of items in a way that's good
        // and then convert it back into an array so we can convert it into a stack
        // so that we can replace both rightHandInventory and leftHandInventory
        // ezpz
        List<GameObject> leftList = new List<GameObject>(leftHand);
        List<GameObject> rightList = new List<GameObject>(rightHand);

        for(int i=0; i<leftList.Count; i++){
            if((leftList[i].GetComponent<ItemIdentification>().containsTag(whatItem)) && (howMany > 0)){
                GameObject temp = leftList[i];
                leftList.RemoveAt(i);
                i--;
                howMany -= 1;
                player.leftHandInventory = new Stack(leftList);
                Debug.Log(player.leftHandInventory);
                player.DropUpdateUI(true, temp);
                if (delete) temp.SetActive(false);
            }
        }
        for(int i=0; i<rightList.Count; i++){
            if((rightList[i].GetComponent<ItemIdentification>().containsTag(whatItem)) && (howMany > 0)){
                GameObject temp = rightList[i];
                rightList.RemoveAt(i);
                i--;
                howMany -= 1;
                Debug.Log("How Many? " + howMany);
                // Debug.Log("Am I dum");
                player.rightHandInventory = new Stack(rightList);
                Debug.Log(player.rightHandInventory);
                player.DropUpdateUI(false, temp);
                if (delete) temp.SetActive(false);
            }
        }
    }

    public static bool CheckIfQuestComplete(int whatQuest){
        // loop through leftHandInventory and rightHandInventory to grab em all in one big stack
        GameObject[] leftHand = new GameObject[player.leftHandInventory.Count];
        GameObject[] rightHand = new GameObject[player.rightHandInventory.Count];
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
                    GetRidOfItemsByName("Cat", 1, leftHand, rightHand, false);
                    GetRidOfItemsByName("Mouse", 1, leftHand, rightHand, false);
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
                    // Get rid of 3 apples
                    Debug.Log("Gets to here");
                    GetRidOfItemsByName("Apple", 3, leftHand, rightHand, true);
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
                    // Get rid of hot object
                    GetRidOfItemsByTag(ItemIdentification.ListOfPossibleTags.Hot, 1, leftHand, rightHand, true);
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

    [YarnCommand("turn_on_questui")]
    public static void TurnOnQuestUI(){
        questUIToggle = !questUIToggle;
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
                break;
            case 4:
                Oven.SetActive(true);
                // Oven.GetComponent<AudioSource>().Play();
                break;
            case 5:
                Pie.SetActive(true);
                Oven.SetActive(false);
                break;
        }
    }
}

