using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using UnityEngine.UIElements;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] Image handsRenderer;

    [Header("SpriteLists")]
    [SerializeField] List<Sprite> baseHands; //#0 is Idle, #1 is point, #2 is hold;

    [Header("Objects")]
    [SerializeField] Image itemRenderer;

    [Header("SpriteLists")]
    [SerializeField] List<Sprite> baseItems; //#0 is None, #1 is apple

    //Other vars
    enum State{Idle, Point, Hold};
    enum Item{None, Apple};

    State curState;
    Item item;

    void Awake(){
        curState = State.Idle; //First one!
        item = Item.None;
    }


    //public methods:
    public void Point(){
        ChangeHands(State.Point);
    }

    public void Idle(){
        ChangeHands(State.Idle);
    }

    public void Hold(){
        ChangeHands(State.Hold);
    }

    //Private:
    void ChangeHands(State incoming){
        //In case we need other logic before changing hands
        switch(incoming){
            case State.Idle:
                handsRenderer.sprite = baseHands[0];
                break;
            case State.Point:
                handsRenderer.sprite = baseHands[1];
                break;
            case State.Hold:
                handsRenderer.sprite = baseHands[2];
                break;
            default:
                break;
        }
    }

    public void Drop(){ HoldItem(Item.None); }
    public void HoldApple(){ HoldItem(Item.Apple); }

    void HoldItem(Item item){
        switch(item){
            case Item.None:
                itemRenderer.sprite = baseItems[0];
                break;
            case Item.Apple:
                itemRenderer.sprite = baseItems[1];
                break;
            default:
                break;
        }
    }

}