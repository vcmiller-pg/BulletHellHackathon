using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SBR.Editor {
    [CustomPropertyDrawer(typeof(ConditionalAttribute))]
    public class ConditionalAttributeDrawer : PropertyDrawer {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            if (ShouldDraw(property)) {
                return EditorGUI.GetPropertyHeight(property, label, true);
            } else {
                return 0;
            }
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            if (ShouldDraw(property)) {
                EditorGUI.PropertyField(position, property, label, true);
            }
        }

        private bool ShouldDraw(SerializedProperty property) {
            ConditionalAttribute attr = attribute as ConditionalAttribute;
            var obj = property.serializedObject.targetObject;

            var func = obj.GetType().GetMethod(attr.condition);
            var prop = obj.GetType().GetProperty(attr.condition);
            var field = obj.GetType().GetField(attr.condition);

            bool? draw = (func?.Invoke(obj, new object[] { }) ?? prop?.GetValue(obj) ?? field?.GetValue(obj)) as bool?;

            if (draw == null) {
                Debug.LogError($"Could not find method, property, or field {attr.condition} on type {obj.GetType()}.");
            }

            return draw != false;
        }
    }
}
