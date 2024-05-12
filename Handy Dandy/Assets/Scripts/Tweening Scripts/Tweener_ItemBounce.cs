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

    void OnCollisionEnter()
    {
        if(easeType == LeanTweenType.animationCurve)
        {
            LeanTween.moveLocalY(tweening_Object, distance, duration).setDelay(delay).setLoopPingPong().setEase(curve);
        }
        else
        {
            LeanTween.moveLocalY(tweening_Object, distance, duration).setDelay(delay).setLoopPingPong().setEase(easeType);
        }
    }

    void OnCollisionExit()
    {
        LeanTween.cancel(tweening_Object);
        tweening_Object.gameObject.transform.localPosition = Vector3.zero;
    }

    private void OnDisable()
    {
        LeanTween.cancel(tweening_Object);
        tweening_Object.gameObject.transform.localPosition = Vector3.zero;
    }
}

