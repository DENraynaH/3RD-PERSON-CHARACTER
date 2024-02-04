using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameProject.Character
{
    public interface ICharacterMovementInput
    {
        public Vector2 MoveInput { get; }
        public bool IsSprinting { get; set; }
        public bool IsJumping { get; set; }
    }
}
