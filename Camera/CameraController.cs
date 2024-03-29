using Cinemachine;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameProject
{
    public enum CameraDirection
    {
        Left,
        Right
    }

    public class CameraController : MonoBehaviour
    {
        public static CameraController Instance { get; private set; }

        [SerializeField] private CinemachineVirtualCamera virtualCamera;
        [SerializeField] [ReadOnly] private Transform _target;

        Coroutine fovCoroutine;

        private void Awake()
        {
            Instance = this;
        }

        public static void SetDistanceToTarget(float newValue)
        {
            CinemachineComponentBase componentBase = Instance.virtualCamera.GetCinemachineComponent(CinemachineCore.Stage.Body);
            if (componentBase is Cinemachine3rdPersonFollow cinemachine3RdPersonFollow)
            {
                cinemachine3RdPersonFollow.CameraDistance = newValue;
            }
        }

        public static void SetFollowAndLootAt(Transform target)
        {
            Instance._target = target;
            Instance.virtualCamera.Follow = Instance._target;
        }

        public static void SetFollow(Transform target)
        {
            Instance._target = target;
            Instance.virtualCamera.Follow = Instance._target;
        }

        public static void SetLookAt(Transform target)
        {
            Instance._target = target;
            Instance.virtualCamera.LookAt = Instance._target;
        }

        public static void ChangeCameraDirection(CameraDirection direction)
        {
            Change3rdPersonCameraSide(Instance.virtualCamera, direction == CameraDirection.Left ? 1 : 0);
        }

        private static void Change3rdPersonCameraSide(CinemachineVirtualCamera virtualCamera, float newSide)
        {
            Cinemachine3rdPersonFollow cinemachine3RdPersonFollow = virtualCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
            cinemachine3RdPersonFollow.CameraSide = newSide;
        }

        public void ChangeFollowFOV(float newFOV, float duration = 0.5f)
        {
            if (fovCoroutine != null) StopCoroutine(fovCoroutine);
            fovCoroutine = StartCoroutine(ChangeFOVSmoothlyCoroutine(virtualCamera, newFOV, duration));
        }

        private IEnumerator ChangeFOVSmoothlyCoroutine(CinemachineVirtualCamera virtualCamera, float newFOV, float duration = 0.5f)
        {
            float initialFOV = virtualCamera.m_Lens.FieldOfView;
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                float t = Mathf.SmoothStep(0, 1, elapsedTime / duration);
                float currentFOV = Mathf.Lerp(initialFOV, newFOV, t);
                virtualCamera.m_Lens.FieldOfView = currentFOV;

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // Ensure the final FOV is set exactly
            virtualCamera.m_Lens.FieldOfView = newFOV;
        }
    }
}
