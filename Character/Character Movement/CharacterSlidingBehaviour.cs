using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace GameProject
{
    public class CharacterSlidingBehaviour : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                transform.eulerAngles = new Vector3
                    (90, transform.eulerAngles.y, transform.eulerAngles.z);
            }
            else if (Input.GetKeyUp(KeyCode.LeftControl))
            {
                transform.rotation = Quaternion.identity;
            }
        }
    }
}
