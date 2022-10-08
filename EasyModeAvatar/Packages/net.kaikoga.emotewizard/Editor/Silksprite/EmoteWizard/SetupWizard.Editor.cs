using System.Linq;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Extensions;
using Silksprite.EmoteWizard.Internal;
using Silksprite.EmoteWizard.Sources.Impl;
using Silksprite.EmoteWizard.Sources.Impl.Multi;
using Silksprite.EmoteWizard.Sources.Multi.Extensions;
using Silksprite.EmoteWizard.UI;
using Silksprite.EmoteWizardSupport.Extensions;
using Silksprite.EmoteWizardSupport.Scopes;
using Silksprite.EmoteWizardSupport.UI;
using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizard
{
    [CustomEditor(typeof(SetupWizard))]
    public class SetupWizardEditor : Editor
    {
        SetupWizard setupWizard;

        void OnEnable()
        {
            setupWizard = (SetupWizard) target;
        }

        public override void OnInspectorGUI()
        {
            var emoteWizardRoot = setupWizard.EmoteWizardRoot;

            using (new ObjectChangeScope(setupWizard))
            {
                TypedGUILayout.Toggle(new GUIContent("Enable Setup Only UI"), ref setupWizard.isSetupMode);
            }

            if (GUILayout.Button("Generate Wizards"))
            {
                emoteWizardRoot.EnsureWizard<AvatarWizard>();
                emoteWizardRoot.EnsureWizard<ExpressionWizard>();
                emoteWizardRoot.EnsureWizard<ParametersWizard>();
                emoteWizardRoot.EnsureWizard<FxWizard>();
                emoteWizardRoot.EnsureWizard<GestureWizard>();
                emoteWizardRoot.EnsureWizard<ActionWizard>();
            }

            EmoteWizardGUILayout.SetupOnlyUI(setupWizard, () =>
            {
                if (GUILayout.Button("Quick Setup 7 HandSigns"))
                {
                    QuickSetup(emoteWizardRoot);
                }

                if (GUILayout.Button("Quick Setup 14 HandSigns"))
                {
                    QuickSetup14(emoteWizardRoot);
                }

                if (GUILayout.Button("Quick Setup EmoteItems"))
                {
                    QuickSetupEmoteItems(emoteWizardRoot);
                }
            });

            if (GUILayout.Button("Complete setup and remove me"))
            {
                DestroySelf(emoteWizardRoot);
                return;
            }
            
            EmoteWizardGUILayout.Tutorial(emoteWizardRoot, Tutorial);
        }

        void DestroySelf(EmoteWizardRoot emoteWizardRoot)
        {
            if (setupWizard.gameObject != emoteWizardRoot.gameObject)
            {
                DestroyImmediate(setupWizard.gameObject, true);
            }
            else
            {
                DestroyImmediate(setupWizard, true);
            }
        }

        static void QuickSetup(EmoteWizardRoot emoteWizardRoot)
        {
            emoteWizardRoot.EnsureWizard<AvatarWizard>();
            var expressionWizard = emoteWizardRoot.EnsureWizard<ExpressionWizard>();
            var parametersWizard = emoteWizardRoot.EnsureWizard<ParametersWizard>();
            var fxWizard = emoteWizardRoot.EnsureWizard<FxWizard>();
            var gestureWizard = emoteWizardRoot.EnsureWizard<GestureWizard>();
            var actionWizard = emoteWizardRoot.EnsureWizard<ActionWizard>();
            expressionWizard.FindOrCreateChildComponent<MultiExpressionItemSource>("Expression Sources").RepopulateDefaultExpressionItems();
            expressionWizard.FindOrCreateChildComponent<MultiParameterSource>("Parameter Sources");
            parametersWizard.RefreshParameters();
            fxWizard.FindOrCreateChildComponent<MultiFxEmoteSource>("FX Sources").RepopulateDefaultEmotes();
            fxWizard.FindOrCreateChildComponent<MultiFxParameterEmoteSource>("FX Sources").RepopulateParameterEmotes(parametersWizard);
            gestureWizard.FindOrCreateChildComponent<MultiGestureEmoteSource>("Gesture Sources").RepopulateDefaultEmotes();
            gestureWizard.FindOrCreateChildComponent<MultiGestureParameterEmoteSource>("Gesture Sources").RepopulateParameterEmotes(parametersWizard);
            actionWizard.FindOrCreateChildComponent<MultiActionEmoteSource>("Action Sources").RepopulateDefaultActionEmotes();
            actionWizard.RepopulateDefaultAfkEmote();
        }

        static void QuickSetup14(EmoteWizardRoot emoteWizardRoot)
        {
            emoteWizardRoot.EnsureWizard<AvatarWizard>();
            var expressionWizard = emoteWizardRoot.EnsureWizard<ExpressionWizard>();
            var parametersWizard = emoteWizardRoot.EnsureWizard<ParametersWizard>();
            var fxWizard = emoteWizardRoot.EnsureWizard<FxWizard>();
            var gestureWizard = emoteWizardRoot.EnsureWizard<GestureWizard>();
            var actionWizard = emoteWizardRoot.EnsureWizard<ActionWizard>();
            expressionWizard.FindOrCreateChildComponent<MultiExpressionItemSource>("Expression Sources").RepopulateDefaultExpressionItems();
            expressionWizard.FindOrCreateChildComponent<MultiParameterSource>("Parameter Sources");
            parametersWizard.RefreshParameters();
            fxWizard.FindOrCreateChildComponent<MultiFxEmoteSource>("FX Sources").RepopulateDefaultEmotes14();
            fxWizard.FindOrCreateChildComponent<MultiFxParameterEmoteSource>("FX Sources").RepopulateParameterEmotes(parametersWizard);
            gestureWizard.FindOrCreateChildComponent<MultiGestureEmoteSource>("Gesture Sources").RepopulateDefaultEmotes();
            gestureWizard.FindOrCreateChildComponent<MultiGestureParameterEmoteSource>("Gesture Sources").RepopulateParameterEmotes(parametersWizard);
            actionWizard.FindOrCreateChildComponent<MultiActionEmoteSource>("Action Sources").RepopulateDefaultActionEmotes();
            actionWizard.RepopulateDefaultAfkEmote();
        }

        static void QuickSetupEmoteItems(EmoteWizardRoot emoteWizardRoot)
        {
            emoteWizardRoot.EnsureWizard<AvatarWizard>();
            var expressionWizard = emoteWizardRoot.EnsureWizard<ExpressionWizard>();
            var parametersWizard = emoteWizardRoot.EnsureWizard<ParametersWizard>();
            var fxLayerWizard = emoteWizardRoot.EnsureWizard<FxLayerWizard>();
            var gestureLayerWizard = emoteWizardRoot.EnsureWizard<GestureLayerWizard>();
            var actionLayerWizard = emoteWizardRoot.EnsureWizard<ActionLayerWizard>();

            var expressionSources = emoteWizardRoot.FindOrCreateChildComponent<EmoteWizardDataSourceFactory>("Expression Sources");
            foreach (var expressionItem in DefaultActionEmote.PopulateDefaultExpressionItems("Default/", Enumerable.Empty<ExpressionItem>()))
            {
                var expressionSource = expressionSources.FindOrCreateChildComponent<ExpressionItemSource>(expressionItem.path);
                expressionSource.expressionItem = expressionItem;

            }

            var parameterSources = emoteWizardRoot.FindOrCreateChildComponent<EmoteWizardDataSourceFactory>("Parameter Sources");
            parametersWizard.RefreshParameters();

            var fxSources = emoteWizardRoot.FindOrCreateChildComponent<EmoteWizardDataSourceFactory>("FX Sources");
            foreach (var fxItem in DefaultEmoteItems.EnumerateDefaultHandSigns("FX"))
            {
                var fxSource = fxSources.FindOrCreateChildComponent<EmoteItemSource>(fxItem.trigger.name);
                fxSource.trigger = fxItem.trigger;
                fxSource.gameObject.AddComponent<EmoteSequenceSource>().sequence = fxItem.sequence;
            }
            var gestureSources = emoteWizardRoot.FindOrCreateChildComponent<EmoteWizardDataSourceFactory>("Gesture Sources");
            foreach (var gestureItem in DefaultEmoteItems.EnumerateDefaultHandSigns("Gesture"))
            {
                var gestureSource = gestureSources.FindOrCreateChildComponent<EmoteItemSource>(gestureItem.trigger.name);
                gestureSource.trigger = gestureItem.trigger;
                gestureSource.gameObject.AddComponent<EmoteSequenceSource>().sequence = gestureItem.sequence;
            }
            var actionSources = emoteWizardRoot.FindOrCreateChildComponent<EmoteWizardDataSourceFactory>("Action Sources");
            foreach (var actionItem in DefaultActionEmote.EnumerateDefaultActionEmoteItems())
            {
                var actionSource = actionSources.FindOrCreateChildComponent<EmoteItemSource>(actionItem.trigger.name);
                actionSource.trigger = actionItem.trigger;
                actionSource.gameObject.AddComponent<EmoteSequenceSource>().sequence = actionItem.sequence;
            }
        }

        static string Tutorial =>
            string.Join("\n",
                "EmoteWizardの初期セットアップと、破壊的な各設定のリセットを行います。",
                "セットアップ中に表示される各ボタンは既存の設定を一括で消去して上書きするため、注意して扱ってください。");
    }
}