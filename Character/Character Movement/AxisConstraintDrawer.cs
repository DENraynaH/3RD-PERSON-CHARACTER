#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;

namespace GameProject
{
    public class AxisConstraintDrawer : OdinValueDrawer<AxisConstraint>
    {
        protected override void DrawPropertyLayout(GUIContent label)
        {
            Rect rect = EditorGUILayout.GetControlRect();
            rect = EditorGUI.PrefixLabel(rect, label);
            AxisConstraint axisConstraint = ValueEntry.SmartValue;
            axisConstraint.X = GUI.Toggle(rect.AlignLeft(rect.width * 0.5f), axisConstraint.X, "X");
            axisConstraint.Y = GUI.Toggle(rect.AlignCenter(rect.width * 0.5f), axisConstraint.Y, "Y");
            axisConstraint.Z = GUI.Toggle(rect.AlignRight(rect.width * 0.5f), axisConstraint.Z, "Z");
            ValueEntry.SmartValue = axisConstraint;
        }
    }
}
#endif