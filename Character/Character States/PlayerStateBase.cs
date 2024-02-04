using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameProject
{
    [Serializable]
    public abstract class PlayerStateBase : State
    {
        [FoldoutGroup("Component Toggle")] [SerializeField] private List<MonoBehaviour> _toggleComponentsOnEnter = new List<MonoBehaviour>();
        [FoldoutGroup("Component Toggle")] [SerializeField] private List<MonoBehaviour> _toggleComponentsOnExit = new List<MonoBehaviour>();
        protected PlayerStateController _playerStateController;
        
        public override void Enter()
        {
            foreach (MonoBehaviour component in _toggleComponentsOnEnter)
                component.enabled = !component.enabled;
        }
        
        public override void Exit()
        {
            foreach (MonoBehaviour component in _toggleComponentsOnExit)
                component.enabled = !component.enabled;
        }
    }
}
