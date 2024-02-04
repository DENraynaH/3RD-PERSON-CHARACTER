using System;
using GameProject.Core;
using GameProject.Utilities;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameProject.Character
{
    public class CharacterInteractHandler : MonoBehaviour
    {
        [FoldoutGroup("Settings")] [SerializeField] private Transform _interactOrigin;
        [FoldoutGroup("Settings")] [SerializeField] private LayerMask _detectMask;
        [FoldoutGroup("Settings")] [SerializeField] private float _interactRange;

        private IInteractable _currentInteractable;
        private ICharacterInteractInput _interactInput;

        private void Awake()
        {
            _interactInput = GetComponent<ICharacterInteractInput>();
            
            if (_interactInput == null)
                LogUtilities.LogMissingComponent<ICharacterInteractInput>();
        }

        private void Update()
        { 
            UpdateInteractable();
            if (_interactInput.IsInteract)
            {
                _currentInteractable?.Interact(this);
                _interactInput.IsInteract = false;
            }
        }

        private void UpdateInteractable()
        {
            Ray interactRay = new Ray(_interactOrigin.position, _interactOrigin.forward);
            
            bool raycastHit = Physics.Raycast
                (interactRay, out RaycastHit hitDetails, _interactRange, _detectMask);

            if (!raycastHit)
            {
                _currentInteractable = null;
                return;
            }

            bool isInteractable = hitDetails.collider.TryGetComponent
                (out IInteractable interactableObject);
            
            if (!isInteractable)
                return;
            
            _currentInteractable = interactableObject;
        }
    }
}
