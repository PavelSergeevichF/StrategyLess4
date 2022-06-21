using System;
using UnityEngine;
using Utils;

namespace UserControlSystem
{
    public abstract class ScriptableObjectValueBase<T> : ScriptableObject, IAwaitable<T>
    {
        public class NewValueNotifier<TAwaited> : BaseIAwaiter<TAwaited>
        {
            private readonly ScriptableObjectValueBase<TAwaited> _scriptableObjectValueBase;
            private TAwaited _result;
            

            public NewValueNotifier(ScriptableObjectValueBase<TAwaited> scriptableObjectValueBase)
            {
                _scriptableObjectValueBase = scriptableObjectValueBase;
                _scriptableObjectValueBase.OnNewValue += ONNewValue;
            }

            private void ONNewValue(TAwaited obj)
            {
                _scriptableObjectValueBase.OnNewValue -= ONNewValue;
                _result = obj;
                _isCompleted = true;
                _continuation?.Invoke();
            }

            public override TAwaited GetResult() => _result;
        }
        
        public T CurrentValue { get; private set; }
        public Action<T> OnNewValue;

        public void SetValue(T value)
        {
            CurrentValue = value;
            OnNewValue?.Invoke(value);
        }
        
        public IAwaiter<T> GetAwaiter()
        {
            return new NewValueNotifier<T>(this);
        }
    }
}