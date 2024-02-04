using System;
using GameProject.Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameProject.Character
{
    public class HealthBehaviour : MonoBehaviour, IDamageable
    {
        public bool Alive => _currentHealth > _minimumHealth;
        public bool OverMax => _currentHealth > _maximumHealth;
        public float Health => _currentHealth;
        
        [FoldoutGroup("Debug")] [SerializeField] private float _currentHealth;
        
        [FoldoutGroup("Health")][SerializeField] private float _maximumHealth = 100;
        [FoldoutGroup("Health")][SerializeField] private float _minimumHealth = 0;
        [FoldoutGroup("Health")][SerializeField] private float _spawnHealth = 100;

        public Action<float> OnChange;
        public Action OnReduce;
        public Action OnRestore;
        public Action OnDeath;
        public Action OnFull;
        private IRespawnable _respawnHandler;
    
        private void Awake() => Restore(_spawnHealth);
    
        public void Reduce(float reduceAmount)
        {
            _currentHealth -= reduceAmount;
            OnReduce?.Invoke();
            OnChange?.Invoke(_currentHealth / _maximumHealth);
    
            if (Alive) 
                return;
            
            OnDeath?.Invoke();
            HandleDeath();
        }
    
        public void Restore(float restoreAmount)
        {
            _currentHealth += restoreAmount;
            OnReduce?.Invoke();
            OnChange?.Invoke(_currentHealth);
            
            if (!OverMax)
                return;
    
            _currentHealth = _maximumHealth;
            OnFull?.Invoke();
        }
    
        private void HandleDeath()
        {
            _respawnHandler ??= GetComponent<IRespawnable>();
            _respawnHandler?.Respawn();
        }
    }   
}
