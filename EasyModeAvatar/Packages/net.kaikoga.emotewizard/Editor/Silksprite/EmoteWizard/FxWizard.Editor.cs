using Silksprite.EmoteWizard.Extensions;
using Silksprite.EmoteWizard.UI;
using Silksprite.EmoteWizardSupport.Extensions;
using Silksprite.EmoteWizardSupport.Scopes;
using Silksprite.EmoteWizardSupport.UI;
using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizard
{
    [CustomEditor(typeof(FxWizard))]
    public class FxWizardEditor : Editor
    {
        FxWizard fxWizard;

        void OnEnable()
        {
            fxWizard = (FxWizard) target;
        }

        public override void OnInspectorGUI()
        {
            var emoteWizardRoot = fxWizard.EmoteWizardRoot;

            using (new ObjectChangeScope(fxWizard))
            {
                var parametersWizard = emoteWizardRoot.GetWizard<ParametersWizard>();

                TypedGUILayout.Toggle("HandSign Override", ref fxWizard.handSignOverrideEnabled);
                if (fxWizard.handSignOverrideEnabled)
                {
                    using (new InvalidValueScope(parametersWizard.IsInvalidParameter(fxWizard.handSignOverrideParameter)))
                    {
                        TypedGUILayout.TextField("HandSign Override Parameter", ref fxWizard.handSignOverrideParameter);
                    }
                }
                else
                {
                    using (new EditorGUI.DisabledScope(true))
                    {
                        TypedGUILayout.TextField("HandSign Override Parameter", ref fxWizard.handSignOverrideParameter);
                    }
                }

                EmoteWizardGUILayout.OutputUIArea(() =>
                {
                    EmoteWizardGUILayout.RequireAnotherWizard<AvatarWizard>(fxWizard, () =>
                    {
                        if (GUILayout.Button("Generate Animation Controller"))
                        {
                            fxWizard.BuildOutputAsset(parametersWizard);
                        }
                    });

                    TypedGUILayout.AssetField("Output Asset", ref fxWizard.outputAsset);
                    TypedGUILayout.AssetField("Reset Clip", ref fxWizard.resetClip);
                });
            }

            EmoteWizardGUILayout.Tutorial(emoteWizardRoot, Tutorial);
            EmoteWizardGUILayout.Tutorial(emoteWizardRoot, Tutorial2);
        }

        static string Tutorial => 
            string.Join("\n",
                "FX Layerの設定を行い、Animation Controllerを生成します。");

        static string Tutorial2 => 
            string.Join("\n",
                "Write Defaultsオフでセットアップされます。",
                "各アニメーションで使われているパラメータをリセットするアニメーションがFX Layerの一番上に追加されます。");
    }
}