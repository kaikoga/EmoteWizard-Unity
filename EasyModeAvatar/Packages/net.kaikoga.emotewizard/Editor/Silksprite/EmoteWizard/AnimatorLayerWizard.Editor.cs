using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Extensions;
using Silksprite.EmoteWizard.UI;
using Silksprite.EmoteWizard.Utils;
using Silksprite.EmoteWizardSupport.Scopes;
using Silksprite.EmoteWizardSupport.UI;
using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizard
{
    [CustomEditor(typeof(AnimatorLayerWizardBase), true)]
    public class AnimatorLayerWizardBaseEditor : Editor
    {
        AnimatorLayerWizardBase _wizard;

        SerializedProperty _serializedDefaultAvatarMask;
        SerializedProperty _serializedOutputAsset;
        SerializedProperty _serializedHasResetClip;
        SerializedProperty _serializedResetClip;

        void OnEnable()
        {
            _wizard = (AnimatorLayerWizardBase)target;

            _serializedDefaultAvatarMask = serializedObject.FindProperty(nameof(AnimatorLayerWizardBase.defaultAvatarMask));
            _serializedOutputAsset = serializedObject.FindProperty(nameof(AnimatorLayerWizardBase.outputAsset));
            _serializedHasResetClip = serializedObject.FindProperty("hasResetClip");
            _serializedResetClip = serializedObject.FindProperty(nameof(AnimatorLayerWizardBase.resetClip));
        }

        public override void OnInspectorGUI()
        {
            var context = _wizard.Environment;

            using (new ObjectChangeScope(_wizard))
            {
                if (_wizard.LayerKind == LayerKind.Gesture)
                {
                    CustomEditorGUILayout.PropertyFieldWithGenerate(_serializedDefaultAvatarMask, () =>
                    {
                        var avatarMask = context.EnsureAsset<AvatarMask>("Gesture/@@@Generated@@@GestureDefaultMask.mask");
                        return AvatarMaskUtils.SetupAsGestureDefault(avatarMask);
                    });
                }
                else
                {
                    EditorGUILayout.PropertyField(_serializedDefaultAvatarMask);
                }

                EditorGUILayout.PropertyField(_serializedHasResetClip);

                EmoteWizardGUILayout.OutputUIArea(() =>
                {
                    EmoteWizardGUILayout.RequireAnotherContext<IAvatarWizardContext, AvatarWizard>(_wizard, () =>
                    {
                        if (GUILayout.Button("Generate Animation Controller"))
                        {
                            _wizard.GetContext().BuildOutputAsset(context.EnsureWizard<ParametersWizard>().GetContext().Snapshot());
                        }
                    });

                    EditorGUILayout.PropertyField(_serializedOutputAsset);
                    using (new EditorGUI.DisabledScope(!_wizard.HasResetClip))
                    {
                        EditorGUILayout.PropertyField(_serializedResetClip);
                    }
                });
            }
            serializedObject.ApplyModifiedProperties();

            EmoteWizardGUILayout.Tutorial(context, Tutorial);
            EmoteWizardGUILayout.Tutorial(context, Tutorial2);
        }

        string Tutorial => 
            string.Join("\n",
                $"{_wizard.LayerKind} Layerの設定を行い、Animation Controllerを生成します。");

        string Tutorial2 => 
            string.Join("\n",
                "Write Defaultsオフでセットアップされます。",
                $"Has Reset Clipがオンの場合、各アニメーションで使われているパラメータをリセットするアニメーションが{_wizard.LayerKind} Layerの一番上に追加されます。");
    }
}