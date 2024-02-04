using System;
using System.Collections;
using System.Collections.Generic;
using GameProject.Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameProject.Character
{
    public class PlayerController : MonoBehaviour, ICharacterMovementInput, ICharacterCameraInput, 
        ICharacterInteractInput, ICharacterToolInput, ICharacterSlidingInput , ICharacterMovementData
    {
        [FoldoutGroup("Settings")] [SerializeField] private InputReader _inputHandler;
        
        public Vector2 MoveInput { get; private set; }
        public Vector2 CameraInput { get; private set; }
        public bool IsSprinting { get; set; }
        public bool IsJumping { get; set; }
        public bool IsInteract { get; set; }
        public bool IsSwitchingSlot { get; set; }
        public int SlotIndex { get; set; }
        public bool IsUse { get; set; }
        public bool IsDrop { get; set; }
        public bool IsGrounded { get; set; }
        public bool IsSliding { get; set; }

        private void Start()
        {
            _inputHandler.OnGameMove += UpdateMoveDirection;
            _inputHandler.OnGameJump += UpdateJump;
            _inputHandler.OnGameMouseMoveX += UpdateMouseInputX;
            _inputHandler.OnGameMouseMoveY += UpdateMouseInputY;
            _inputHandler.OnGameInteract += UpdateInteract;
            _inputHandler.OnGameToolSwitch += UpdateToolSwitch;
        }

        private void UpdateMoveDirection(Vector2 inputVector) => MoveInput = inputVector;
        private void UpdateJump() => IsJumping = true;
        private void UpdateSprinting() => IsSprinting = true;
        private void UpdateMouseInputX(float x) => CameraInput = new Vector2(x, CameraInput.y);
        private void UpdateMouseInputY(float y) => CameraInput = new Vector2(CameraInput.x, y);
        private void UpdateInteract() => IsInteract = true;

        private void UpdateToolSwitch(int slotIndex)
        {
            IsSwitchingSlot = true;
            SlotIndex = slotIndex;
        }
    }
}
