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
    [SerializeField] List<Sprite> baseHands; //#0 is Idle, #1 is point;

    //Other vars
    enum State{Idle, Point};
    State curState;

    void Awake(){
        curState = State.Idle; //First one!
    }


    //public methods:
    public void Point(){
        ChangeHands(State.Point);
    }

    public void Idle(){
        ChangeHands(State.Idle);
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
            default:
                break;
        }
    }



}
