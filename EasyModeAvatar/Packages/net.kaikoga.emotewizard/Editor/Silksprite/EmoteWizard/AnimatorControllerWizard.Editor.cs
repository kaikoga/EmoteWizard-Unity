using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.Extensions;
using Silksprite.EmoteWizard.UI;
using Silksprite.EmoteWizardSupport.Extensions;
using Silksprite.EmoteWizardSupport.Scopes;
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
        SerializedProperty _serializedResetClip;

        void OnEnable()
        {
            _wizard = (AnimatorLayerWizardBase)target;

            _serializedDefaultAvatarMask = serializedObject.FindProperty(nameof(AnimatorLayerWizardBase.defaultAvatarMask));
            _serializedOutputAsset = serializedObject.FindProperty(nameof(AnimatorLayerWizardBase.outputAsset));
            _serializedResetClip = serializedObject.FindProperty(nameof(AnimatorLayerWizardBase.resetClip));
        }

        public override void OnInspectorGUI()
        {
            var emoteWizardRoot = _wizard.EmoteWizardRoot;

            using (new ObjectChangeScope(_wizard))
            {
                var parametersWizard = emoteWizardRoot.GetWizard<ParametersWizard>();

                EditorGUILayout.PropertyField(_serializedDefaultAvatarMask);
                
                EmoteWizardGUILayout.OutputUIArea(() =>
                {
                    EmoteWizardGUILayout.RequireAnotherWizard<AvatarWizard>(_wizard, () =>
                    {
                        if (GUILayout.Button("Generate Animation Controller"))
                        {
                            _wizard.BuildOutputAsset(parametersWizard);
                        }
                    });

                    EditorGUILayout.PropertyField(_serializedOutputAsset);
                    EditorGUILayout.PropertyField(_serializedResetClip);
                });
            }

            EmoteWizardGUILayout.Tutorial(emoteWizardRoot, Tutorial);
            EmoteWizardGUILayout.Tutorial(emoteWizardRoot, Tutorial2);
        }

        string Tutorial => 
            string.Join("\n",
                $"{_wizard.LayerKind} Layerの設定を行い、Animation Controllerを生成します。");

        string Tutorial2 => 
            string.Join("\n",
                "Write Defaultsオフでセットアップされます。",
                $"各アニメーションで使われているパラメータをリセットするアニメーションが{_wizard.LayerKind} Layerの一番上に追加されます。");
    }
}