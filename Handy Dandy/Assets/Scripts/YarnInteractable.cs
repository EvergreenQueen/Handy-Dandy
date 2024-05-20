using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class YarnInteractable : MonoBehaviour {
    // internal properties exposed to editor
    [SerializeField] private string conversationStartNode;
    [SerializeField] public YarnProject yarnProject;
    public static YarnInteractable Instance {get; private set;}

    // internal properties not exposed to editor
    private DialogueRunner dialogueRunner;
    private Light lightIndicatorObject = null;
    private bool interactable = true;
    private bool isCurrentConversation = false;
    private float defaultIndicatorIntensity;

    public void Awake(){
        // If there is an instance, and it's not me, delete myself.
    
        if (Instance != null && Instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            Instance = this; 
        } 
    }

    public void Start() {
        dialogueRunner = FindObjectOfType<Yarn.Unity.DialogueRunner>();
        // dialogueRunner.onDialogueComplete.AddListener(EndConversation);
        // lightIndicatorObject = GetComponentInChildren<Light>();
        // // get starter intensity of light then
        // // if we're using it as an indicator => hide it 
        // if (lightIndicatorObject != null) {
        //     defaultIndicatorIntensity = lightIndicatorObject.intensity;
        //     lightIndicatorObject.intensity = 0;
        // }

        // now trying this function to pass in everything to HandleQuests
        // dialogueRunner.AddCommandHandler("switch_project", HandleQuests.SwitchProject);
        // Debug.Log("command registered");
        // dialogueRunner.AddCommandHandler("check_quest_completion", HandleQuests.CheckQuestCompletion);
        // dialogueRunner.AddCommandHandler<int>("start_quest", HandleQuests.StartQuest);
        // dialogueRunner.AddCommandHandler<int>("see_if_already_completed", HandleQuests.SeeIfAlreadyCompleted);
        // dialogueRunner.AddCommandHandler<string>("set_intro_done", HandleQuests.SetIntroDone);
        // dialogueRunner.AddCommandHandler("check_quest_completion", HandleQuests.CheckQuestCompletion);

        // dialogueRunner.AddFunction<int>("get_curr_quest", HandleQuests.GetCurrQuest);
        // dialogueRunner.AddFunction<string, bool>("see_if_already_played_intro", HandleQuests.SeeIfAlreadyPlayedIntro);

        HandleQuests.dialogueRunner = dialogueRunner;
        HandleQuests.varStore = dialogueRunner.GetComponent<InMemoryVariableStorage>();
        HandleQuests.charaDialogue = yarnProject;
    }

    public void StartNPCDialogue(string whoTalkingTo) {
        conversationStartNode = whoTalkingTo;
        if (interactable && !dialogueRunner.IsDialogueRunning) {
            StartConversation();
        }
    }

    private void StartConversation() {
        Debug.Log($"Started conversation with {name}.");
        isCurrentConversation = true;
        // if (lightIndicatorObject != null) {
        //     lightIndicatorObject.intensity = defaultIndicatorIntensity;
        // }
        dialogueRunner.StartDialogue(conversationStartNode);
    }

    private void EndConversation() {
        if (isCurrentConversation) {
            // if (lightIndicatorObject != null) {
            //     lightIndicatorObject.intensity = 0;
            // }
            isCurrentConversation = false;
            Debug.Log($"Started conversation with {name}.");
        }
    }

//    [YarnCommand("disable")]
    public void DisableConversation() {
        interactable = false;
    }
}