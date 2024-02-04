using System;
using GameProject.Utilities;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameProject.Character
{
    [RequireComponent(typeof(CharacterController))]
    public class CharacterPushHandler : MonoBehaviour
    {
        [FoldoutGroup("Settings")] [SerializeField] private LayerMask _pushableLayers;
        [FoldoutGroup("Settings")] [SerializeField] private float _pushForce = 0.10f;
        
        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            Rigidbody hitRigidbody = hit.collider.attachedRigidbody;
            
            if (hitRigidbody == null || hitRigidbody.isKinematic)
                return;

            bool canPush = _pushableLayers.ContainsLayer(hit.gameObject.layer);

            if (canPush)
            {
                Vector3 pushDirection = new Vector3(hit.moveDirection.x, 0.0f, hit.moveDirection.z);
                hitRigidbody.AddForce(pushDirection * _pushForce, ForceMode.Impulse);
            }
        }
    }
}
