using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Extensions;
using Silksprite.EmoteWizard.Sources.Impl.Base;
using Silksprite.EmoteWizardSupport.Scopes;
using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizard.Sources.Base
{
    [CustomEditor(typeof(ParameterEmoteSourceBase), true)]
    public class ParameterEmoteSourceBaseEditor : Editor
    {
        SerializedProperty _serializedEnabled;
        SerializedProperty _serializedName;
        SerializedProperty _serializedParameter;
        SerializedProperty _serializedValueKind;
        SerializedProperty _serializedEmoteKind;
        SerializedProperty _serializedStates;

        void OnEnable()
        {
            var serializedItem = serializedObject.FindProperty(nameof(ParameterEmoteSourceBase.parameterEmote));

            _serializedEnabled = serializedItem.FindPropertyRelative(nameof(ParameterEmote.enabled));
            _serializedName = serializedItem.FindPropertyRelative(nameof(ParameterEmote.name));
            _serializedParameter = serializedItem.FindPropertyRelative(nameof(ParameterEmote.parameter));
            _serializedValueKind = serializedItem.FindPropertyRelative(nameof(ParameterEmote.valueKind));
            _serializedEmoteKind = serializedItem.FindPropertyRelative(nameof(ParameterEmote.emoteKind));
            _serializedStates = serializedItem.FindPropertyRelative(nameof(ParameterEmote.states));
        }

        public override void OnInspectorGUI()
        {
            var parameterEmoteSourceBase = (ParameterEmoteSourceBase)target;
            var layer = parameterEmoteSourceBase.LayerName;
            var parameterEmote = parameterEmoteSourceBase.parameterEmote;

            using (new EditorGUI.IndentLevelScope(-EditorGUI.indentLevel))
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    using (new HideLabelsScope())
                    {
                        EditorGUILayout.PropertyField(_serializedEnabled);
                    }
                    EditorGUI.BeginDisabledGroup(!parameterEmote.enabled);
                    EditorGUILayout.PropertyField(_serializedName);
                }
                
                using (new EditorGUILayout.HorizontalScope())
                {
                    using (new InvalidValueScope(parameterEmoteSourceBase.EmoteWizardRoot.EnsureWizard<ParametersWizard>().IsInvalidParameter(_serializedParameter.stringValue)))
                    {
                        EditorGUILayout.PropertyField(_serializedParameter);
                    }
                    using (new HideLabelsScope())
                    {
                        EditorGUILayout.PropertyField(_serializedValueKind);
                    }
                }

                EditorGUILayout.PropertyField(_serializedEmoteKind);
            }

            if (parameterEmote.emoteKind != ParameterEmoteKind.Unused)
            {
                // using (var sub = context.ParameterEmoteStateDrawerContext(property.name, property.emoteKind == ParameterEmoteKind.Transition).StartContext())
                {
                    EditorGUILayout.PropertyField(_serializedStates);
                    // if (sub.Context.EditTargets && IsExpandedTracker.GetIsExpanded(property.states))
                    {
                        if (GUILayout.Button("Generate clips from targets (TBD)"))
                        {
                            // parameterEmote.GenerateParameterEmoteClipsFromTargets(context.Component, context);
                        }
                    }
                }
            }
            EditorGUI.EndDisabledGroup();
            
            serializedObject.ApplyModifiedProperties();
        }
    }
}