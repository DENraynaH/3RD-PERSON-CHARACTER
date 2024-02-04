using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameProject.Character
{
    public interface IInteractable
    {
        void Interact(CharacterInteractHandler interactHandler);
    }
}
