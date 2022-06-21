using UnityEngine;
using System;
using Utils;

public abstract class BaseIAwaiter<T>: IAwaiter<T>
{
    protected Action _continuation;
    protected bool _isCompleted;
    public void OnCompleted(Action continuation)
    {
        if (_isCompleted)
        {
            continuation?.Invoke();
        }
        else
        {
            _continuation = continuation;
        }
    }
    public bool IsCompleted => _isCompleted;

    public abstract T GetResult();
}
