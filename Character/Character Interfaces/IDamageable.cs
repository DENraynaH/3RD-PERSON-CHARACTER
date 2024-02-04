using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameProject.Core
{
    public interface IDamageable
    {
        public void Reduce(float reduceAmount);
        public void Restore(float restoreAmount);
    }   
}
