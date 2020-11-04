using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace CyberMonk.Utils.Attributes
{
    public class ReadOnlyDrawer : PropertyDrawer
    {
        /// <summary>
        /// Unity method for drawing GUI in Editor
        /// </summary>
        /// <param name="position">The position of the property..</param>
        /// <param name="property">The serialized property..</param>
        /// <param name="label">The GUI Content label.</param>
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            bool prevValue = GUI.enabled;
            GUI.enabled = false;
            EditorGUI.PropertyField(position, property, label, true);
            GUI.enabled = prevValue;
        }
    }
}