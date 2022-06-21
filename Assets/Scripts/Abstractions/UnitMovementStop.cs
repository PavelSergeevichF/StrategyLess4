using System;
using UnityEngine;
using UnityEngine.AI;
using Utils;

namespace Core
{
    public class UnitMovementStop : MonoBehaviour, IAwaitable<AsyncExtensions.Void>
    {
        public class StopAwaiter : BaseIAwaiter<AsyncExtensions.Void>
        {
            private readonly UnitMovementStop _unitMovementStop;

            public StopAwaiter(UnitMovementStop unitMovementStop)
            {
                _unitMovementStop = unitMovementStop;
                _unitMovementStop.OnStop += ONStop;
            }

            private void ONStop()
            {
                _unitMovementStop.OnStop -= ONStop;
                _isCompleted = true;
                _continuation?.Invoke();
            }

            public override AsyncExtensions.Void GetResult() => new AsyncExtensions.Void();
        }

        public event Action OnStop;

        [SerializeField] private NavMeshAgent _agent;

        private void Update()
        {
            if (!_agent.pathPending)
            {
                if (_agent.remainingDistance <= _agent.stoppingDistance)
                {
                    if (!_agent.hasPath || _agent.velocity.sqrMagnitude == 0f)
                    {
                        OnStop?.Invoke();
                    }
                }
            }
        }

        public IAwaiter<AsyncExtensions.Void> GetAwaiter() => new StopAwaiter(this);
    }
}