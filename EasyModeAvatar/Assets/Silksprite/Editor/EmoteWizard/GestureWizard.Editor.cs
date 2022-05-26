using System;
using System.Linq;
using Silksprite.EmoteWizard.Extensions;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.Sources;
using Silksprite.EmoteWizard.Sources.Extensions;
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

                if (gestureWizard.HasLegacyData)
                {
                    EditorGUILayout.HelpBox("レガシーデータを検出しました。以下のボタンを押してエクスポートします。", MessageType.Warning);
                    if (GUILayout.Button("Migrate to Data Source"))
                    {
                        MigrateToDataSource();
                    }
                }

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
        }

        void MigrateToDataSource()
        {
            var emoteSource = gestureWizard.AddChildComponent<GestureEmoteSource>();
            emoteSource.emotes = gestureWizard.legacyEmotes.ToList();
            gestureWizard.legacyEmotes.Clear();
            
            var parameterEmoteSource = gestureWizard.AddChildComponent<GestureParameterEmoteSource>();
            parameterEmoteSource.parameterEmotes = gestureWizard.legacyParameterEmotes.ToList();
            gestureWizard.legacyParameterEmotes.Clear();
            
            var mixinSource = gestureWizard.AddChildComponent<GestureAnimationMixinSource>();
            mixinSource.baseMixins = gestureWizard.legacyBaseMixins.ToList();
            mixinSource.mixins = gestureWizard.legacyMixins.ToList();
            gestureWizard.legacyBaseMixins.Clear();
            gestureWizard.legacyMixins.Clear();
        }
        
        static string Tutorial => 
            string.Join("\n",
                "Gesture Layerの設定を行い、AnimationControllerを生成します。",
                "Write Defaultsはオフになります。",
                "各アニメーションで使われているシェイプキーなどをリセットするアニメーションがResetレイヤーに自動的に設定されます。");
    }
}