using System;
using GameProject.Utilities;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameProject.Character
{
    [RequireComponent(typeof(CharacterController))]
    public class CharacterMovementHandler : MonoBehaviour
    {
        [FoldoutGroup("Debug")] [ReadOnly] [SerializeField] private float _verticalVelocity;
        [FoldoutGroup("Debug")] [ReadOnly] [SerializeField] private bool _isGrounded;
        [FoldoutGroup("Debug")] [ReadOnly] [SerializeField] private bool _isSprinting;
        
        [FoldoutGroup("Movement")] [SerializeField] private float _gravityForce = -9.81f;
        [FoldoutGroup("Movement")] [SerializeField] private float _groundedGravity = -2.0f;
        [FoldoutGroup("Movement")] [SerializeField] private float _terminalVelocity = -50.0f;
        [FoldoutGroup("Movement")] [SerializeField] private float _moveForce = 5.0f;
        [FoldoutGroup("Movement")] [SerializeField] private float _sprintMultiplier;
        
        [FoldoutGroup("Jumping")] [SerializeField] private float _jumpHeight = 8.0f;
        [FoldoutGroup("Jumping")] [SerializeField] private LayerMask _groundLayers;
        [FoldoutGroup("Jumping")] [SerializeField] private float _verticalOffset = -1.0f;
        [FoldoutGroup("Jumping")] [SerializeField] private float _groundedRadius = 0.25f;
        
        private CharacterController _characterController;
        private ICharacterMovementInput _movementInput;

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            _movementInput = GetComponent<ICharacterMovementInput>();
            
            if (_movementInput == null)
                LogUtilities.LogMissingComponent<ICharacterMovementInput>();
        }

        private void Update()
        {
            UpdateGrounded();
            ApplyGravity();
            Jump();
            Move();
        }

        public void Move()
        {
            Vector3 direction = (transform.forward * _movementInput.MoveInput.y 
                                 + transform.right * _movementInput.MoveInput.x).normalized;

            Vector3 moveDirection = _isSprinting ? 
                direction * (_moveForce * _sprintMultiplier) : direction * _moveForce;
            
            Vector3 moveDirectionWithGravity = moveDirection.Add(y: _verticalVelocity);
            
            _characterController.Move(moveDirectionWithGravity * Time.deltaTime);
        }

        public void Jump()
        {
            if (_isGrounded && _movementInput.IsJumping)
                _verticalVelocity = Mathf.Sqrt(_jumpHeight * -2f * _gravityForce);

            _movementInput.IsJumping = false;
        }
        
        private void UpdateGrounded()
        {
            Vector3 position = transform.position.Add(y: _verticalOffset);
            _isGrounded = Physics.CheckSphere
                (position, _groundedRadius, _groundLayers);
        }

        public void ApplyGravity()
        {
            if (_isGrounded && _verticalVelocity < 0.0f)
                _verticalVelocity = _groundedGravity;
            
            if (_verticalVelocity > _terminalVelocity )
                _verticalVelocity += _gravityForce * Time.deltaTime;
        }
        
        #if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = _isGrounded ? Color.green : Color.red;
            Vector3 position = transform.position.Add(y: _verticalOffset);
            Gizmos.DrawWireSphere(position, _groundedRadius);
        }
        #endif
    }
}

