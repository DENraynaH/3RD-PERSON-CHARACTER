using System;
using GameProject.Utilities;
using Pelumi.Juicer;
using Raynah.Utilities;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameProject.Character
{
    [RequireComponent(typeof(Rigidbody))]
    public class CharacterMovementBehaviour : MonoBehaviour
    {
        public Transform CameraTarget => _cameraTarget.transform;
        
        [FoldoutGroup("Debug")] [SerializeField] private Vector3 _currentVelocity;
        [FoldoutGroup("Debug")] [SerializeField] private float _currentDamping;
        [FoldoutGroup("Debug")] [SerializeField] private float _currentSpeed;
        
        [FoldoutGroup("Rotation")] [SerializeField] private GameObject _cameraTarget;
        [FoldoutGroup("Rotation")] [SerializeField] private float _rotationSpeed = 3.0f;
        
        [FoldoutGroup("Movement")] [SerializeField] private float _gravityForce = -9.81f;
        [FoldoutGroup("Movement")] [SerializeField] private float _moveForce = 5.0f;
        [FoldoutGroup("Movement")] [SerializeField] private float _sprintMultiplier;
        
        [FoldoutGroup("Jumping")] [SerializeField] private float _jumpHeight = 8.0f;
        [FoldoutGroup("Jumping")] [SerializeField] private LayerMask _groundLayers;
        [FoldoutGroup("Jumping")] [SerializeField] private float _verticalOffset = -1.0f;
        [FoldoutGroup("Jumping")] [SerializeField] private float _groundedRadius = 0.25f;
        
        private Rigidbody _characterBody;
        private ICharacterMovementInput _movementInput;
        private ICharacterMovementData _movementData;
        private ObjectForceProvider _objectForceProvider;
        private readonly float inputThreshold = 0.01f;

        private void Awake()
        {
            _characterBody = GetComponent<Rigidbody>();
            _characterBody.constraints = RigidbodyConstraints.FreezeRotation;
            _characterBody.useGravity = false;
            
            _movementInput = GetComponent<ICharacterMovementInput>();
            
            if (_movementInput == null)
                LogUtilities.LogMissingComponent<ICharacterMovementInput>();
            
            _movementData = GetComponent<ICharacterMovementData>();
            
            if (_movementData == null)
                LogUtilities.LogMissingComponent<ICharacterMovementData>();

            _objectForceProvider = Forces.GetForceProvider(_characterBody);
            _objectForceProvider.AxisConstraint = new AxisConstraint(false, true, false);
        }

        private void Update()
        {
            UpdateGrounded();
            Move();
            Jump();
            ApplyGravity();
        }

        public void Move()
        {
            Vector3 inputDirection = new Vector3(_movementInput.MoveInput.x, 0, _movementInput.MoveInput.y);

            if (inputDirection.sqrMagnitude > inputThreshold)
            {
                RotateTowards(inputDirection);
                
                Vector3 forwardDirection = transform.forward;
            
                Vector3 moveDirection = _movementInput.IsSprinting ?
                    forwardDirection * (_moveForce * _sprintMultiplier) : forwardDirection * _moveForce;

                _objectForceProvider.AppendObjectForce(moveDirection);
            }
        }
        
        public void RotateTowards(Vector3 inputDirection)
        {
            float targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + _cameraTarget.transform.eulerAngles.y;
            Vector3 targetDirection = Quaternion.Euler(0f, targetRotation, 0f) * Vector3.forward;
            Quaternion targetRotationQuaternion = Quaternion.LookRotation(targetDirection, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotationQuaternion, Time.deltaTime * _rotationSpeed);
        }

        public void Jump()
        {
            if (_movementData.IsGrounded && _movementInput.IsJumping)
                _characterBody.velocity = _characterBody.velocity.Only(y: _jumpHeight);
            
            _movementInput.IsJumping = false;
        }
        
        private void UpdateGrounded()
        {
            Vector3 position = transform.position.Add(y: _verticalOffset);
            _movementData.IsGrounded = Physics.CheckSphere
                (position, _groundedRadius, _groundLayers);
        }
        
        public void ApplyGravity()
        {
            Vector3 gravityForce = Vector3.up * _gravityForce;
            _characterBody.AddForce(gravityForce);
        }

        public void ForceRotateToFaceDirection(Vector3 direction, float duration, Action OnComplete)
        {
            Vector3 lookDirection = new Vector3(direction.x, 0, direction.z);
            Quaternion lookRotation = Quaternion.LookRotation(lookDirection);
            transform.JuicyRotateQuaternion(lookRotation, duration)
                .SetOnComplected(() => OnComplete?.Invoke())
                .Start();
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if (_movementData == null)
                return;
            
            Gizmos.color = _movementData.IsGrounded ? Color.green : Color.red;
            Vector3 position = transform.position.Add(y: _verticalOffset);
            Gizmos.DrawWireSphere(position, _groundedRadius);
        }
        #endif
    }
}

