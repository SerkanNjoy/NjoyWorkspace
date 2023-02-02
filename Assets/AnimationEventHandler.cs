using System;
using UnityEngine;

public class AnimationEventHandler : MonoBehaviour
{
    private Action _onInvokeAction;

    public void SetAnimationEvent(Action action)
    {
        _onInvokeAction = action;
    }

    public void InvokeAnimEvent()
    {
        _onInvokeAction.Invoke();
        _onInvokeAction = null;
    }
}