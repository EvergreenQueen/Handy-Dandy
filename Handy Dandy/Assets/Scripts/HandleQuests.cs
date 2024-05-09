using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class HandleQuests : MonoBehaviour
{
    public InMemoryVariableStorage varStore;

    [YarnCommand("switch_project")]
    public static void SwitchProject(){
        Debug.Log("Starting Quest 1, remember to update the UI"); //this works
        FindObjectOfType<Yarn.Unity.DialogueRunner>().SetProject(charaDialogue);
    }
}

