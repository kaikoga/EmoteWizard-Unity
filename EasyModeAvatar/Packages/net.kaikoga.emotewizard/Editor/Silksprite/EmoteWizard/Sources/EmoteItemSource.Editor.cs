using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Sources.Impl;
using UnityEditor;

namespace Silksprite.EmoteWizard.Sources
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(EmoteItemSource))]
    public class EmoteItemSourceEditor : Editor
    {
        SerializedProperty _serializedName;
        SerializedProperty _serializedPriority;
        SerializedProperty _serializedConditions;

        SerializedProperty _serializedHasExpressionItem;
        SerializedProperty _serializedExpressionItemPath;
        SerializedProperty _serializedExpressionItemIcon;

        EmoteItemSource _emoteItemSource;

        void OnEnable()
        {
            var serializedTrigger = serializedObject.FindProperty(nameof(EmoteItemSource.trigger));

            _serializedName = serializedTrigger.FindPropertyRelative(nameof(EmoteTrigger.name));
            _serializedPriority = serializedTrigger.FindPropertyRelative(nameof(EmoteTrigger.priority));
            _serializedConditions = serializedTrigger.FindPropertyRelative(nameof(EmoteTrigger.conditions));

            _serializedHasExpressionItem = serializedObject.FindProperty(nameof(EmoteItemSource.hasExpressionItem));
            _serializedExpressionItemPath = serializedObject.FindProperty(nameof(EmoteItemSource.expressionItemPath));
            _serializedExpressionItemIcon = serializedObject.FindProperty(nameof(EmoteItemSource.expressionItemIcon));

            _emoteItemSource = (EmoteItemSource)target;
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(_serializedName);
            EditorGUILayout.PropertyField(_serializedPriority);
            EditorGUILayout.PropertyField(_serializedConditions);

            using (new EditorGUI.DisabledScope(!_emoteItemSource.CanAutoExpression))
            {
                EditorGUILayout.PropertyField(_serializedHasExpressionItem);
            }
            if (_emoteItemSource.IsAutoExpression)
            {
                using (new EditorGUI.IndentLevelScope())
                {
                    EditorGUILayout.PropertyField(_serializedExpressionItemPath);
                    EditorGUILayout.PropertyField(_serializedExpressionItemIcon);
                }
            }
            
            serializedObject.ApplyModifiedProperties();

            if (_emoteItemSource.IsMirrorItem)
            {
                EditorGUILayout.HelpBox(MirrorInfoText, MessageType.Info);
            }
        }
        
        static string MirrorInfoText =>
            string.Join("\n",
                "パラメータのミラーリングが有効になっています。",
                "Gesture: GestureLeft / GestureRight",
                "GestureOther: GestureRight / GestureLeft",
                "GestureWeight: GestureLeftWeight / GestureRightWeight",
                "GestureOtherWeight: GestureRightWeight / GestureLeftWeight");
    }
}
