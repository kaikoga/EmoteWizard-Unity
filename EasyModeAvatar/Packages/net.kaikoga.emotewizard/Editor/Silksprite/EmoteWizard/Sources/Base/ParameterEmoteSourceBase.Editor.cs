using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Extensions;
using Silksprite.EmoteWizard.Sources.Impl.Base;
using Silksprite.EmoteWizardSupport.Scopes;
using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizard.Sources.Base
{
    [CustomEditor(typeof(EmoteSourceBase), true)]
    public class ParameterEmoteSourceBaseEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var parameterEmoteSourceBase = (ParameterEmoteSourceBase)target;
            var layer = parameterEmoteSourceBase.LayerName;
            var parameterEmote = parameterEmoteSourceBase.parameterEmote;

            var serializedObj = serializedObject.FindProperty(nameof(ParameterEmoteSourceBase.parameterEmote));

            using (new EditorGUI.IndentLevelScope(-EditorGUI.indentLevel))
            {
                using (new HideLabelsScope())
                {
                    EditorGUILayout.PropertyField(serializedObj.FindPropertyRelative(nameof(ParameterEmote.enabled)));
                }
                EditorGUI.BeginDisabledGroup(!parameterEmote.enabled);
                EditorGUILayout.PropertyField(serializedObj.FindPropertyRelative(nameof(ParameterEmote.name)));
                using (new InvalidValueScope(parameterEmoteSourceBase.EmoteWizardRoot.EnsureWizard<ParametersWizard>().IsInvalidParameter(parameterEmote.parameter)))
                {
                    EditorGUILayout.PropertyField(serializedObj.FindPropertyRelative(nameof(ParameterEmote.enabled)));
                }

                using (new HideLabelsScope())
                {
                    EditorGUILayout.PropertyField(serializedObj.FindPropertyRelative(nameof(ParameterEmote.valueKind)));
                }

                EditorGUILayout.PropertyField(serializedObj.FindPropertyRelative(nameof(ParameterEmote.emoteKind)));
            }

            if (parameterEmote.emoteKind != ParameterEmoteKind.Unused)
            {
                // using (var sub = context.ParameterEmoteStateDrawerContext(property.name, property.emoteKind == ParameterEmoteKind.Transition).StartContext())
                {
                    EditorGUILayout.PropertyField(serializedObj.FindPropertyRelative(nameof(ParameterEmote.states)));
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