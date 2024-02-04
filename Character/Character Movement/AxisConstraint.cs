using System;
using UnityEngine;

namespace GameProject
{
    [Serializable]
    public struct AxisConstraint
    {
        [field:SerializeField] public bool X { get; set; }
        [field:SerializeField] public bool Y { get; set; }
        [field:SerializeField] public bool Z { get; set; }
        
        public AxisConstraint(bool x, bool y, bool z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }
    
}