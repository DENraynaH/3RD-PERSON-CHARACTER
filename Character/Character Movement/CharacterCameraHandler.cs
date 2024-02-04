using System;
using System.Collections;
using System.Collections.Generic;
using GameProject.Utilities;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameProject.Character
{
    public class CharacterCameraHandler : MonoBehaviour
    {
        [FoldoutGroup("Settings")] [SerializeField] private GameObject _cameraTarget;
        [FoldoutGroup("Settings")] [SerializeField] private float _rotationSpeed = 10.0f;
        [FoldoutGroup("Settings")] [SerializeField] private float _upperCameraClamp = 85.0f;
        [FoldoutGroup("Settings")] [SerializeField] private float _lowerCameraClamp = -85.0f;

        private ICharacterCameraInput _cameraInput;
        
        private float _cameraPitch;
        private float _rotationVelocity;

        private void Awake()
        {
            bool hasCameraInput = TryGetComponent(out _cameraInput);
            if (!hasCameraInput)
                LogUtilities.LogMissingComponent<ICharacterCameraInput>();
            
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Update()
        {
            UpdateCameraRotation(_cameraInput.CameraInput);
        } 

        public void UpdateCameraRotation(Vector2 mouseInput)
        {
            _cameraPitch += -mouseInput.y * _rotationSpeed * Time.deltaTime;
            _rotationVelocity = mouseInput.x * _rotationSpeed * Time.deltaTime;
            
            _cameraPitch = ClampAngle(_cameraPitch, _lowerCameraClamp, _upperCameraClamp);         
            _cameraTarget.transform.localRotation = Quaternion.Euler(_cameraPitch, 0.0f, 0.0f);
            transform.Rotate(Vector3.up * _rotationVelocity);

            float ClampAngle(float lfAngle, float lfMin, float lfMax)
            {
                if (lfAngle < -360f) lfAngle += 360f;
                if (lfAngle > 360f) lfAngle -= 360f;
                return Mathf.Clamp(lfAngle, lfMin, lfMax);
            }
        }
    }
}
