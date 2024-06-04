using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using UnityEngine.UIElements;
using UnityEngine.UI;
// using PlayerControls; // this doesnt work

public class UIManager : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] Image handsRendererLeft;
    [SerializeField] Image handsRendererRight;

    [Header("SpriteLists")]
    [SerializeField] List<Sprite> baseHands; //#0 is Idle, #1 is point, #2 is hold;
    [SerializeField] float activeHeight; // if hand is active, draw at this height. otherwise lower it a bit
    float leftXPos, rightXPos;

    [SerializeField] Image itemRendererLeft;
    [SerializeField] Image itemRendererRight;

    [Header("SpriteLists")]
    [SerializeField] List<Sprite> baseItems; //#0 is None, #1 is apple


    //Other vars
    public enum State{Idle, Point, Hold, Grip_Loose, Basket};
    public enum Item{None, Apple, Ice_Cube, Mouse, Cat, Basket, Hot_Sauce, Start, Pie};

    State currStateLeft;
    State currStateRight;

    GameObject player;
    PlayerControls controller;

    //
    RectTransform leftHandTransform, leftItemTransform, rightHandTransform, rightItemTransform;

    void Awake(){
        currStateLeft = currStateRight = State.Idle; //First one!
        player = GameObject.FindWithTag("Player");
        controller = player.GetComponent<PlayerControls>();
        Debug.Log("player's name is " + player.name);

        leftHandTransform = handsRendererLeft.GetComponent<RectTransform>();
        rightHandTransform = handsRendererRight.GetComponent<RectTransform>();
        leftItemTransform = itemRendererLeft.GetComponent<RectTransform>();
        rightItemTransform = itemRendererRight.GetComponent<RectTransform>();

        leftXPos = leftHandTransform.anchoredPosition.x;
        rightXPos = rightHandTransform.anchoredPosition.x;
        Debug.Log("left: " + leftXPos + " right: " + rightXPos);
    }

    void Update() {
        // left hand being controlled
        if(controller.controllingContainer == PlayerControls.whichContainer.Left)
        {
            // RectTransform.anchoredPosition
            leftHandTransform.anchoredPosition = leftItemTransform.anchoredPosition = new Vector3(leftXPos, activeHeight, 0);
            rightHandTransform.anchoredPosition = rightItemTransform.anchoredPosition = new Vector3(rightXPos, activeHeight - 30, 0);
        }
        else // right hand being controlled
        {
            leftHandTransform.anchoredPosition = leftItemTransform.anchoredPosition = new Vector3(leftXPos, activeHeight - 30, 0);
            rightHandTransform.anchoredPosition = rightItemTransform.anchoredPosition = new Vector3(rightXPos, activeHeight, 0);
        }
    }

    //public methods:
    public void Point(PlayerControls.whichContainer h){
        ChangeHands(h, State.Point);
    }

    public void Idle(PlayerControls.whichContainer h){
        ChangeHands(h, State.Idle);
    }

    public void Hold(PlayerControls.whichContainer h){
        ChangeHands(h, State.Hold);
    }

    public void Grip_Loose(PlayerControls.whichContainer h){
        ChangeHands(h, State.Grip_Loose);
    }

    //Private:
    // nuh uh
    public void ChangeHands(PlayerControls.whichContainer h, State incoming){
        //In case we need other logic before changing hands
        bool isLeft;
        Image renderer = null;
        if(h == PlayerControls.whichContainer.Left){
            renderer = handsRendererLeft;
            currStateLeft = incoming;
        }
        else if(h == PlayerControls.whichContainer.Right){
            renderer = handsRendererRight;
            currStateRight = incoming;    
        }

        int stateId = (int)incoming;

        renderer.sprite = baseHands[stateId]; 

        //Special Case for basket:
        if(incoming == State.Basket){
            // Debug.Log("It is a basket :3");
            renderer.sprite = baseHands[(int)State.Hold];
            if(renderer == handsRendererLeft) itemRendererLeft.sprite = baseItems[(int) Item.Basket];
            if(renderer == handsRendererRight) itemRendererRight.sprite = baseItems[(int) Item.Basket];
        } else{
            renderer.sprite = baseHands[stateId]; 
        }
    }

    public void Drop(PlayerControls.whichContainer h){ HoldItem(h, Item.None); }
    public void HoldApple(PlayerControls.whichContainer h){ HoldItem(h, Item.Apple); }
    public void HoldIce_Cube(PlayerControls.whichContainer h){ HoldItem(h, Item.Ice_Cube); }
    public void HoldMouse(PlayerControls.whichContainer h){ HoldItem(h, Item.Mouse); }
    public void HoldCat(PlayerControls.whichContainer h){ HoldItem(h, Item.Cat); }

    public void HoldItem(PlayerControls.whichContainer h, Item item){
        Image renderer = null;
        Debug.Log(h);
        if(h == PlayerControls.whichContainer.Left) {
            Debug.Log("CONTAINER LEFT");
            renderer = itemRendererLeft;
            if(currStateLeft == State.Basket) return;
        }else if(h == PlayerControls.whichContainer.Right) {
            Debug.Log("CONTAINER right");
            renderer = itemRendererRight;
            if(currStateRight == State.Basket) return;
        }

        // Debug.Log(renderer);
        
        int itemId = (int)item;

        renderer.sprite = baseItems[itemId];

        // put in brackets so it's collapsible :p
        {
        // if(h == PlayerControls.whichContainer.Left)
        // {
        //     switch(item){
        //         case Item.None:
        //             itemRendererLeft.sprite = baseItems[0];
        //             break;
        //         case Item.Apple:
        //             itemRendererLeft.sprite = baseItems[1];
        //             break;
        //         case Item.Ice_Cube:
        //             itemRendererLeft.sprite = baseItems[2];
        //             break;
        //         case Item.Mouse:
        //             itemRendererLeft.sprite = baseItems[3];
        //             break;
        //         case Item.Cat:
        //             itemRendererLeft.sprite = baseItems[4];
        //             break;
        //         default:
        //             break;
        //     }
        // }
        // else if(h == PlayerControls.whichContainer.Right)
        // {
        //     switch(item){
        //         case Item.None:
        //             itemRendererRight.sprite = baseItems[0];
        //             break;
        //         case Item.Apple:
        //             itemRendererRight.sprite = baseItems[1];
        //             break;
        //         case Item.Ice_Cube:
        //             itemRendererRight.sprite = baseItems[2];
        //             break;
        //         case Item.Mouse:
        //             itemRendererRight.sprite = baseItems[3];
        //             break;
        //         case Item.Cat:
        //             itemRendererRight.sprite = baseItems[4];
        //             break;
        //         default:
        //             break;
        //     }
        // }
        }
    }
}
