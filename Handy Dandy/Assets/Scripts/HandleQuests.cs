using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class HandleQuests : MonoBehaviour
{
    public InMemoryVariableStorage varStore;

    [YarnCommand("start_quest_1")]
    public static void StartQuest1(){
        Debug.Log("Starting Quest 1, remember to update the UI"); //this works
    }
}

