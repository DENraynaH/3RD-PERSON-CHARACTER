using UnityEngine;

namespace GameProject.Character
{
    public interface ICharacterMovementData
    {
        bool IsGrounded { get; set; }
        public Vector2 MoveInput { get; }
        public bool IsSprinting { get; set; }
        public bool IsJumping { get; set; }
    }
}