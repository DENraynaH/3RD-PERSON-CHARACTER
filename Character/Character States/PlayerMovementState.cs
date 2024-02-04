using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GameProject.Character
{
    [Serializable]
    public class PlayerMovementState : PlayerStateBase, InitialisableState<PlayerStateController>
    {
        private StateMachine _playerMovementStateMachine = new StateMachine();
        [SerializeField] private PlayerJumpState _playerJumpState = new PlayerJumpState();
        [SerializeField] PlayerFallingState _playerFallingState = new PlayerFallingState();
        
        public void InitialiseController(PlayerStateController playerStateController)
        {
            _playerStateController = playerStateController;
            InitialiseState();
        }
        
        public void InitialiseState()
        {
            _playerMovementStateMachine.Initialise(_playerJumpState);
            _playerJumpState.InitialiseController(this);
            _playerFallingState.InitialiseController(this);
        }

        public override void Update()
        {
            base.Update();
            _playerMovementStateMachine.Update();
        }
    }
    
    [Serializable]
    public class PlayerJumpState : PlayerStateBase, InitialisableState<PlayerMovementState>
    {
        public void InitialiseController(PlayerMovementState playerStateController)
        {
            
        }

        public void InitialiseState()
        {
            
        }
    }
    
    [Serializable]
    public class PlayerFallingState : PlayerStateBase, InitialisableState<PlayerMovementState>
    {
        public void InitialiseController(PlayerMovementState playerStateController)
        {
            
        }

        public void InitialiseState()
        {
            
        }
    }
}
