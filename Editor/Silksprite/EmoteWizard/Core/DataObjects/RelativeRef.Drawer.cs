using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizardSupport.Extensions;
using Silksprite.EmoteWizardSupport.UI;
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
            var relativePath = serializedProperty.Lop(nameof(RelativeRef<TTarget>.relativePath), Loc("RelativeRef.relativePath"));

            if (!target.objectReferenceValue && !string.IsNullOrEmpty(relativePath.Property.stringValue))
            {
                var env = ((EmoteWizardBehaviour)serializedProperty.serializedObject.targetObject).CreateEnv();
                target.objectReferenceValue = RelativeRef<TTarget>.ResolveTarget(env?.AvatarRoot, relativePath.Property.stringValue);
            }

            EditorGUI.BeginProperty(position, label, serializedProperty);
            EditorGUI.BeginChangeCheck();
            target.objectReferenceValue = EditorGUI.ObjectField(position.UISliceV(0), label, target.objectReferenceValue, typeof(TTarget), true);
            if (EditorGUI.EndChangeCheck())
            {
                var env = ((EmoteWizardBehaviour)serializedProperty.serializedObject.targetObject).CreateEnv();
                relativePath.Property.stringValue = RuntimeUtil.RelativePath(env?.AvatarRoot, ((TTarget)target.objectReferenceValue)?.transform);
            }
            EditorGUI.EndProperty();

            EmoteWizardGUI.PropAsLabel(position.UISliceV(1), relativePath);
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