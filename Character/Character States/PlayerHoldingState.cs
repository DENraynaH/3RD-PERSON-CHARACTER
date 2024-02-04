using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameProject.Character
{
    [Serializable]
    public class PlayerHoldingState : PlayerStateBase
    {
        public void InitialiseController(PlayerStateController playerStateController)
        {
            _playerStateController = playerStateController;
            InitialiseState();
        }
        
        public void InitialiseState()
        {
            
        }
    }
}
