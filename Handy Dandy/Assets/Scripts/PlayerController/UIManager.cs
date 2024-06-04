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


    // [Header("Objects")]
    [SerializeField] Image itemRendererLeft;
    [SerializeField] Image itemRendererRight;

    [Header("SpriteLists")]
    [SerializeField] List<Sprite> baseItems; //#0 is None, #1 is apple


    //Other vars
    public enum State{Idle, Point, Hold, Grip_Loose, Basket};
    public enum Item{None, Apple, Ice_Cube, Mouse, Cat, Hot_Sauce, Start};

    State currStateLeft;
    State currStateRight;
    // Item itemLeft;
    // Item itemRight;

    void Awake(){
        currStateLeft = currStateRight = State.Idle; //First one!
        // itemLeft = itemRight = Item.None;
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
        Image renderer = null;
        if(h == PlayerControls.whichContainer.Left) renderer = handsRendererLeft;
        else if(h == PlayerControls.whichContainer.Right) renderer = handsRendererRight;

        int stateId = (int)incoming;

        renderer.sprite = baseHands[stateId];

        // brackets for collapsability
        {
        // if(h == PlayerControls.whichContainer.Left)
        // {
        //     switch(incoming){
        //         case State.Idle:
        //             handsRendererLeft.sprite = baseHands[0];
        //             break;
        //         case State.Point:
        //             handsRendererLeft.sprite = baseHands[1];
        //             break;
        //         case State.Hold:
        //             handsRendererLeft.sprite = baseHands[2];
        //             break;
        //         case State.Grip_Loose:
        //             handsRendererLeft.sprite = baseHands[3];
        //             break;
        //         default:
        //             break;
        //     }
        // }
        // else if(h == PlayerControls.whichContainer.Right)
        // {
        //     switch(incoming){
        //         case State.Idle:
        //             handsRendererRight.sprite = baseHands[0];
        //             break;
        //         case State.Point:
        //             handsRendererRight.sprite = baseHands[1];
        //             break;
        //         case State.Hold:
        //             handsRendererRight.sprite = baseHands[2];
        //             break;
        //         case State.Grip_Loose:
        //             handsRendererRight.sprite = baseHands[3];
        //             break;
        //         default:
        //             break;
        //     }
        // }
        }
    }

    public void Drop(PlayerControls.whichContainer h){ HoldItem(h, Item.None); }
    public void HoldApple(PlayerControls.whichContainer h){ HoldItem(h, Item.Apple); }
    public void HoldIce_Cube(PlayerControls.whichContainer h){ HoldItem(h, Item.Ice_Cube); }
    public void HoldMouse(PlayerControls.whichContainer h){ HoldItem(h, Item.Mouse); }
    public void HoldCat(PlayerControls.whichContainer h){ HoldItem(h, Item.Cat); }

    public void HoldItem(PlayerControls.whichContainer h, Item item){
        Image renderer = null;
        if(h == PlayerControls.whichContainer.Left) renderer = itemRendererLeft;
        else if(h == PlayerControls.whichContainer.Right) renderer = itemRendererRight;
        
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
