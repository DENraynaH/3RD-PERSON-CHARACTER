using System.Collections;
using System.Collections.Generic;
using GameProject.Utilities;
using Pelumi.Juicer;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameProject
{
    public class ObjectForceProvider : MonoBehaviour
    {
        public AxisConstraint AxisConstraint { get => _axisConstraint; set => _axisConstraint = value; }

        [FoldoutGroup("Debug")] [SerializeField] [ReadOnly] private float _objectDistanceMoved;
        [FoldoutGroup("Debug")] [SerializeField] [ReadOnly] private float _objectMovingSpeed;
        [FoldoutGroup("Debug")] [SerializeField] [ReadOnly] private float _objectMovingTimer;
        [FoldoutGroup("Settings")] [SerializeField] float _objectMovingTimerThreshold = 0.1f;
        [FoldoutGroup("Settings")] [SerializeField] private float _objectMovingSpeedThreshold = 0.35f;
        [FoldoutGroup("Settings")] [SerializeField] private AxisConstraint _axisConstraint;
        
        private readonly List<HorizontalForce> _objectForces = new List<HorizontalForce>();
        private Rigidbody _objectBody;
        private Vector3 _objectLastPosition;
        
        private void Update()
        {
            CalculateObjectMovingDistance();
            CalculateObjectMovingSpeed();
            ProcessForces();
            CalculateForce();
            ClearCompleteForces();
        }

        public void SetObjectInstantForce(Vector3 velocity, float duration = 0)
        {
            ClearAccumulatedForces();
            AppendObjectForce(velocity, duration);
        }
        
        public void SetObjectInstantForce(Vector3 fromVelocity, Vector3 toVelocity, float duration)
        {
            ClearAccumulatedForces();
            AppendObjectForce(fromVelocity, toVelocity, duration);
        }
        
        public void SetObjectInstantForce(Vector3 fromVelocity, Vector3 toVelocity, Ease ease, float duration)
        {
            ClearAccumulatedForces();
            AppendObjectForce(fromVelocity, toVelocity, ease, duration);
        }

        public void AppendObjectForce(Vector3 velocity, float duration = 0)
        {
            HorizontalForce horizontalForce = new HorizontalForce
                (velocity, velocity, null, duration);
            _objectForces.Add(horizontalForce);
        }
        
        public void AppendObjectForce(Vector3 fromVelocity, Vector3 toVelocity, float duration)
        {
            HorizontalForce horizontalForce = new HorizontalForce
                (fromVelocity, toVelocity, null, duration);
            _objectForces.Add(horizontalForce);
        }
        
        public void AppendObjectForce(Vector3 fromVelocity, Vector3 toVelocity, Ease ease, float duration)
        {
            HorizontalForce horizontalForce = new HorizontalForce
                (fromVelocity, toVelocity, ease, duration);
            _objectForces.Add(horizontalForce);
        }
        
        public void ClearAccumulatedForces()
        {
            _objectForces.Clear();
        }

        public void SetObjectRigidbody(Rigidbody objectBody)
        {
            _objectBody = objectBody;
        }
        
        private void ProcessForces()
        {
            for (int i = _objectForces.Count - 1; i >= 0; i--)
            {
                HorizontalForce horizontalForce = _objectForces[i];
                horizontalForce.ProcessForce(Time.deltaTime);
            }
        }
        
        private void ClearCompleteForces()
        {
            for (int i = _objectForces.Count - 1; i >= 0; i--)
            {
                HorizontalForce horizontalForce = _objectForces[i];
                if (horizontalForce.IsComplete)
                    _objectForces.RemoveAt(i);
            }
        }
        
        private void CalculateForce()
        {
            if(_objectBody.isKinematic)
                return;
            
            Vector3 totalForce = Vector3.zero;
            
            foreach (HorizontalForce horizontalForce in _objectForces)
                totalForce += horizontalForce.CurrentForce;

            _objectBody.velocity = new Vector3
                    (_axisConstraint.X ? _objectBody.velocity.x : totalForce.x,
                    _axisConstraint.Y ? _objectBody.velocity.y : totalForce.y,
                    _axisConstraint.Z ? _objectBody.velocity.z : totalForce.z);
        }

        private void CalculateObjectMovingDistance()
        {
            _objectDistanceMoved += Vector3.Distance(_objectLastPosition, transform.position);
            _objectLastPosition = transform.position;
        }

        private void CalculateObjectMovingSpeed()
        {
            _objectMovingTimer += Time.deltaTime;

            if (_objectMovingTimer < _objectMovingTimerThreshold)
                return;
            
            _objectMovingSpeed = _objectDistanceMoved * (1 / _objectMovingTimer);
            
            if (_objectMovingSpeed < _objectMovingSpeedThreshold)
                ClearAccumulatedForces();
            
            _objectMovingTimer = 0;
            _objectDistanceMoved = 0;
        }
    }
    
    public class HorizontalForce
    {
        public bool IsComplete => CurrentDuration >= Duration;
        
        public Vector3 FromForce { get; set; }
        public Vector3 ToForce { get; set; }
        public Vector3 CurrentForce { get; set; }
        public float Duration { get; set; }
        public float CurrentDuration { get; set; }
        public Ease? CurrentEase { get; set; }
        
        public HorizontalForce(Vector3 fromForce, Vector3 toForce, Ease? currentEase, float duration)
        {
            FromForce = fromForce;
            ToForce = toForce;
            Duration = duration;
            CurrentEase = currentEase;
        }

        public void ProcessForce(float deltaTime)
        {
            CurrentDuration += deltaTime;
            float lerpValue = CurrentDuration / Duration;

            if (CurrentEase != null)
            {
                Ease currentEase = CurrentEase.Value;
                EasingFunction.Function easingFunction = EasingFunction.GetEasingFunction(currentEase);
                CurrentForce = Vector3.Lerp(FromForce, ToForce, easingFunction(0, 1, lerpValue));
            }
            else
                CurrentForce = Vector3.Lerp(FromForce, ToForce, lerpValue);
        }
    }

    public static class Forces
    {
        public static ObjectForceProvider GetForceProvider(Rigidbody rigidbody)
        {
            ObjectForceProvider objectForceProvider = rigidbody.gameObject.GetOrAddComponent<ObjectForceProvider>();
            objectForceProvider.SetObjectRigidbody(rigidbody);
            return objectForceProvider;
        }
        
        public static ObjectForceProvider GetForceProvider(this GameObject gameObject, Rigidbody rigidbody)
        {
            ObjectForceProvider objectForceProvider = gameObject.GetOrAddComponent<ObjectForceProvider>();
            objectForceProvider.SetObjectRigidbody(rigidbody);
            return objectForceProvider;
        }
        
        public static ObjectForceProvider GetForceProvider(this Transform transform, Rigidbody rigidbody)
        {
            ObjectForceProvider objectForceProvider = transform.gameObject.GetOrAddComponent<ObjectForceProvider>();
            objectForceProvider.SetObjectRigidbody(rigidbody);
            return objectForceProvider;
        }
    }
}
