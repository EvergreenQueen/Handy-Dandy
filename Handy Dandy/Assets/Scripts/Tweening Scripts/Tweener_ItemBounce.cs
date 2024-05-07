using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] LeanTweenType easeType;
    [SerializeField] AnimationCurve curve;
    [SerializeField] GameObject tweening_Object;
    [SerializeField] float distance;
    [SerializeField] float duration;
    [SerializeField] float delay;

    void OnTriggerEnter()
    {
        if(easeType == LeanTweenType.animationCurve)
        {
            LeanTween.moveY(tweening_Object, distance, duration).setLoopPingPong().setEase(curve);
        }
        else
        {
            LeanTween.moveY(tweening_Object, distance, duration).setDelay(delay).setLoopPingPong().setEase(easeType);
        }
    }

    private void OnTriggerExit()
    {
        LeanTween.cancel(tweening_Object);
        tweening_Object.gameObject.transform.localPosition = Vector3.zero;
    }
}

