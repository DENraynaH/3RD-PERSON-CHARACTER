using System;
using System.Collections;
using System.Collections.Generic;
using GameProject.Character;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace GameProject
{
    public class PlayerStateController : MonoBehaviour
    {
        public PlayerController PlayerController => _playerController;
        
        public PlayerMovementState PlayerMovementState => _playerMovementState;
        public PlayerSlidingState PlayerSlidingState => _playerSlidingState;
        public PlayerHoldingState PlayerHoldingState => _playerHoldingState;
        
        [FoldoutGroup("Debug")] [SerializeField] [ReadOnly] private string _currentState;
        [FoldoutGroup("States")] [SerializeField] private PlayerMovementState _playerMovementState = new();
        [FoldoutGroup("States")] [SerializeField] private PlayerSlidingState _playerSlidingState = new();
        [FoldoutGroup("States")] [SerializeField] private PlayerHoldingState _playerHoldingState = new();
        
        private readonly StateMachine _stateMachine = new StateMachine();
        private PlayerController _playerController;

        private void Start()
        {
            _playerController = GetComponent<PlayerController>();
            
            _playerMovementState.InitialiseController(this);
            _playerSlidingState.InitialiseController(this);
            _playerHoldingState.InitialiseController(this);
            
            _stateMachine.Initialise(_playerMovementState);
        }

        private void Update()
        {
            _stateMachine.Update();
            _currentState = _stateMachine.CurrentState.ToString();
        }
    }
}
