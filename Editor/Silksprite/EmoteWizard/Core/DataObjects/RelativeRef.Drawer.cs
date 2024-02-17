using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizardSupport.Extensions;
using Silksprite.EmoteWizardSupport.Utils;
using UnityEditor;
using UnityEngine;
using static Silksprite.EmoteWizardSupport.L10n.LocalizationTool;

namespace Silksprite.EmoteWizard.DataObjects
{
    public abstract class RelativeRefDrawer<TTarget> : PropertyDrawer
    where TTarget : Component
    {
        public override void OnGUI(Rect position, SerializedProperty serializedProperty, GUIContent label)
        {
            var target = serializedProperty.FindPropertyRelative(nameof(RelativeRef<TTarget>.target));
            var relativePath = serializedProperty.FindPropertyRelative(nameof(RelativeRef<TTarget>.relativePath));

            if (!target.objectReferenceValue && !string.IsNullOrEmpty(relativePath.stringValue))
            {
                var env = ((EmoteWizardBehaviour)serializedProperty.serializedObject.targetObject).CreateEnv();
                target.objectReferenceValue = RelativeRef<TTarget>.ResolveTarget(env?.AvatarRoot, relativePath.stringValue);
            }

            EditorGUI.BeginProperty(position, label, serializedProperty);
            EditorGUI.BeginChangeCheck();
            target.objectReferenceValue = EditorGUI.ObjectField(position.UISliceV(0), label, target.objectReferenceValue, typeof(TTarget), true);
            if (EditorGUI.EndChangeCheck())
            {
                var env = ((EmoteWizardBehaviour)serializedProperty.serializedObject.targetObject).CreateEnv();
                relativePath.stringValue = RuntimeUtil.RelativePath(env?.AvatarRoot, ((TTarget)target.objectReferenceValue)?.transform);
            }
            EditorGUI.EndProperty();

            EditorGUI.BeginProperty(position, Loc("RelativeRef.relativePath").GUIContent, relativePath);
            EditorGUI.LabelField(position.UISliceV(1), Loc("RelativeRef.relativePath").GUIContent, new GUIContent(relativePath.stringValue));
            EditorGUI.EndProperty();
        }
    }

    [CustomPropertyDrawer(typeof(RelativeTransformRef))]
    public class RelativeTransformRefDrawer : RelativeRefDrawer<Transform>
    {
    }

    [CustomPropertyDrawer(typeof(RelativeSkinnedMeshRendererRef))]
    public class RelativeSkinnedMeshRendererRefDrawer : RelativeRefDrawer<SkinnedMeshRenderer>
    {
    }
}