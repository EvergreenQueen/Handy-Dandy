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
    enum State{Idle, Point, Hold};
    enum Item{None, Apple, Ice_Cube};

    State currStateLeft;
    State currStateRight;
    // Item itemLeft;
    // Item itemRight;

    void Awake(){
        currStateLeft = currStateRight = State.Idle; //First one!
        // itemLeft = itemRight = Item.None;
    }

    //public methods:
    public void Point(PlayerControls.hand h){
        ChangeHands(h, State.Point);
    }

    public void Idle(PlayerControls.hand h){
        ChangeHands(h, State.Idle);
    }

    public void Hold(PlayerControls.hand h){
        ChangeHands(h, State.Hold);
    }

    //Private:
    void ChangeHands(PlayerControls.hand h, State incoming){
        //In case we need other logic before changing hands

        if(h == PlayerControls.hand.Left)
        {
            switch(incoming){
                case State.Idle:
                    handsRendererLeft.sprite = baseHands[0];
                    break;
                case State.Point:
                    handsRendererLeft.sprite = baseHands[1];
                    break;
                case State.Hold:
                    handsRendererLeft.sprite = baseHands[2];
                    break;
                default:
                    break;
            }
        }
        else if(h == PlayerControls.hand.Right)
        {
            switch(incoming){
                case State.Idle:
                    handsRendererRight.sprite = baseHands[0];
                    break;
                case State.Point:
                    handsRendererRight.sprite = baseHands[1];
                    break;
                case State.Hold:
                    handsRendererRight.sprite = baseHands[2];
                    break;
                default:
                    break;
            }
        }
    }

    public void Drop(PlayerControls.hand h){ HoldItem(h, Item.None); }
    public void HoldApple(PlayerControls.hand h){ HoldItem(h, Item.Apple); }
    public void HoldIce_Cube(PlayerControls.hand h){ HoldItem(h, Item.Ice_Cube); }

    public void HoldItem(PlayerControls.hand h, GlobalVars.PickableItems itemType){
        // i thought enums were stand-ins for numbers? maybe we could do it without switch case?
        if(h == PlayerControls.hand.Left)
        {
            switch(itemType){
                case GlobalVars.PickableItems.None:
                    itemRendererLeft.sprite = baseItems[0];
                    break;
                case GlobalVars.PickableItems.Apple:
                    itemRendererLeft.sprite = baseItems[1];
                    break;
                case Item.Ice_Cube:
                    itemRendererLeft.sprite = baseItems[2];
                    break;
                default:
                    break;
            }
        }
        else if(h == PlayerControls.hand.Right)
        {
            switch(itemType){
                case GlobalVars.PickableItems.None:
                    itemRendererRight.sprite = baseItems[0];
                    break;
                case GlobalVars.PickableItems.Apple:
                    itemRendererRight.sprite = baseItems[1];
                    break;
                case Item.Ice_Cube:
                    itemRendererRight.sprite = baseItems[2];
                    break;
                default:
                    break;
            }
        }
    }
}
