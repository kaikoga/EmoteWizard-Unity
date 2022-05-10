using System.Linq;
using Silksprite.EmoteWizard.Extensions;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.Sources;
using Silksprite.EmoteWizard.Sources.Extensions;
using Silksprite.EmoteWizard.UI;
using Silksprite.EmoteWizardSupport.Extensions;
using Silksprite.EmoteWizardSupport.Scopes;
using Silksprite.EmoteWizardSupport.UI;
using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizard
{
    [CustomEditor(typeof(FxWizard))]
    public class FxWizardEditor : AnimationWizardBaseEditor
    {
        FxWizard fxWizard;

        void OnEnable()
        {
            fxWizard = (FxWizard) target;
        }

        public override void OnInspectorGUI()
        {
            using (new ObjectChangeScope(fxWizard))
            {
                var emoteWizardRoot = fxWizard.EmoteWizardRoot;
                var parametersWizard = emoteWizardRoot.GetWizard<ParametersWizard>();

                if (emoteWizardRoot.showCopyPasteJsonButtons) this.CopyPasteJsonButtons();

                EmoteWizardGUILayout.SetupOnlyUI(fxWizard, () =>
                {
                    if (GUILayout.Button("Repopulate HandSigns: 7 items"))
                    {
                        fxWizard.AddChildComponent<FxEmoteSource>().RepopulateDefaultEmotes();
                    }

                    if (GUILayout.Button("Repopulate HandSigns: 14 items"))
                    {
                        fxWizard.AddChildComponent<FxEmoteSource>().RepopulateDefaultEmotes14();
                    }

                    if (parametersWizard != null)
                    {
                        if (GUILayout.Button("Repopulate Parameters"))
                        {
                            fxWizard.AddChildComponent<FxParameterEmoteSource>().RepopulateParameterEmotes(parametersWizard);
                        }
                    }
                });

                TypedGUILayout.Toggle("Advanced Animations", ref fxWizard.advancedAnimations);
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

                if (fxWizard.HasLegacyData)
                {
                    EditorGUILayout.HelpBox("レガシーデータを検出しました。以下のボタンを押してエクスポートします。", MessageType.Warning);
                    if (GUILayout.Button("Migrate to Data Source"))
                    {
                        MigrateToDataSource();
                    }
                }
                if (GUILayout.Button("Add Emote Source"))
                {
                    fxWizard.AddChildComponentAndSelect<FxEmoteSource>();
                }
                if (GUILayout.Button("Add Parameter Emote Source"))
                {
                    fxWizard.AddChildComponentAndSelect<FxParameterEmoteSource>();
                }
                if (GUILayout.Button("Add Animation Mixin Source"))
                {
                    fxWizard.AddChildComponentAndSelect<FxAnimationMixinSource>();
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

                EmoteWizardGUILayout.Tutorial(emoteWizardRoot, $"FX Layerの設定を行い、AnimationControllerを生成します。\n{Tutorial}");
            }
        }

        void MigrateToDataSource()
        {
            var emoteSource = fxWizard.AddChildComponent<FxEmoteSource>();
            emoteSource.emotes = fxWizard.legacyEmotes.ToList();
            fxWizard.legacyEmotes.Clear();
            
            var parameterEmoteSource = fxWizard.AddChildComponent<FxParameterEmoteSource>();
            parameterEmoteSource.parameterEmotes = fxWizard.legacyParameterEmotes.ToList();
            fxWizard.legacyParameterEmotes.Clear();
            
            var mixinSource = fxWizard.AddChildComponent<FxAnimationMixinSource>();
            mixinSource.baseMixins = fxWizard.legacyBaseMixins.ToList();
            mixinSource.mixins = fxWizard.legacyMixins.ToList();
            fxWizard.legacyBaseMixins.Clear();
            fxWizard.legacyMixins.Clear();
        }
    }
}