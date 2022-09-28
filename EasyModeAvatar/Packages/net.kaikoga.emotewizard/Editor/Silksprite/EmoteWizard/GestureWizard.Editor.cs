using Silksprite.EmoteWizard.Extensions;
using Silksprite.EmoteWizard.UI;
using Silksprite.EmoteWizard.Utils;
using Silksprite.EmoteWizardSupport.Extensions;
using Silksprite.EmoteWizardSupport.Scopes;
using Silksprite.EmoteWizardSupport.UI;
using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizard
{
    [CustomEditor(typeof(GestureWizard))]
    public class GestureWizardEditor : Editor
    {
        GestureWizard gestureWizard;

        void OnEnable()
        {
            gestureWizard = (GestureWizard) target;
        }

        public override void OnInspectorGUI()
        {
            var emoteWizardRoot = gestureWizard.EmoteWizardRoot;
            if (emoteWizardRoot.showCopyPasteJsonButtons) this.CopyPasteJsonButtons();

            using (new ObjectChangeScope(gestureWizard))
            {
                var parametersWizard = emoteWizardRoot.GetWizard<ParametersWizard>();

                TypedGUILayout.Toggle("HandSign Override", ref gestureWizard.handSignOverrideEnabled);
                if (gestureWizard.handSignOverrideEnabled)
                {
                    using (new InvalidValueScope(parametersWizard.IsInvalidParameter(gestureWizard.handSignOverrideParameter)))
                    {
                        TypedGUILayout.TextField("HandSign Override Parameter", ref gestureWizard.handSignOverrideParameter);
                    }
                }
                else
                {
                    using (new EditorGUI.DisabledScope(true))
                    {
                        TypedGUILayout.TextField("HandSign Override Parameter", ref gestureWizard.handSignOverrideParameter);
                    }
                }

                CustomTypedGUILayout.AssetFieldWithGenerate("Default Avatar Mask", ref gestureWizard.defaultAvatarMask, () =>
                {
                    var avatarMask = emoteWizardRoot.EnsureAsset<AvatarMask>("Gesture/@@@Generated@@@GestureDefaultMask.mask");
                    return AvatarMaskUtils.SetupAsGestureDefault(avatarMask);
                });

                EmoteWizardGUILayout.OutputUIArea(() =>
                {
                    EmoteWizardGUILayout.RequireAnotherWizard<AvatarWizard>(gestureWizard, () =>
                    {
                        if (GUILayout.Button("Generate Animation Controller"))
                        {
                            gestureWizard.BuildOutputAsset(parametersWizard);
                        }
                    });

                    TypedGUILayout.AssetField("Output Asset", ref gestureWizard.outputAsset);
                });
            }

            EmoteWizardGUILayout.Tutorial(emoteWizardRoot, Tutorial);
            EmoteWizardGUILayout.Tutorial(emoteWizardRoot, Tutorial2);
        }

        static string Tutorial => 
            string.Join("\n",
                "Gesture Layerの設定を行い、Animation Controllerを生成します。");

        static string Tutorial2 => 
            string.Join("\n",
                "Write Defaultsオフでセットアップされます。",
                "各アニメーションで使われているパラメータをリセットするアニメーションがGesture Layerの一番上に追加されます。");
    }
}