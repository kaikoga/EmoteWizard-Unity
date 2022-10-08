using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.Collections;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.DataObjects.Typed;
using Silksprite.EmoteWizard.DataObjects.Typed.DrawerContexts;
using Silksprite.EmoteWizard.DataObjects.Typed.DrawerStates;
using Silksprite.EmoteWizard.Sources.Impl.Multi.Base;
using Silksprite.EmoteWizard.Sources.Multi.Extensions;
using Silksprite.EmoteWizard.UI;
using Silksprite.EmoteWizard.Utils;
using Silksprite.EmoteWizardSupport.Collections.Generic;
using Silksprite.EmoteWizardSupport.Extensions;
using Silksprite.EmoteWizardSupport.Scopes;
using Silksprite.EmoteWizardSupport.UI;
using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizard.Sources.Multi.Base
{
    [CustomEditor(typeof(MultiEmoteSourceBase), true)]
    public class MultiEmoteSourceEditor : Editor
    {
        MultiEmoteSourceBase _multiEmoteSource;

        EmoteDrawerState _emotesState;

        ExpandableReorderableList<Emote> _emoteList;

        void OnEnable()
        {
            _multiEmoteSource = (MultiEmoteSourceBase)target;

            _emotesState = new EmoteDrawerState();

            _emoteList = new ExpandableReorderableList<Emote>(new EmoteListHeaderDrawer(), new EmoteDrawer(), "HandSign Emotes", ref _multiEmoteSource.emotes);
        }

        public override void OnInspectorGUI()
        {
            var emoteWizardRoot = _multiEmoteSource.EmoteWizardRoot;
            if (emoteWizardRoot.showCopyPasteJsonButtons) this.CopyPasteJsonButtons();

            using (new ObjectChangeScope(_multiEmoteSource))
            {
                var parametersWizard = emoteWizardRoot.GetWizard<ParametersWizard>();
                var animationWizardBase = _multiEmoteSource.LayerName == EmoteWizardConstants.LayerNames.Fx ? (AnimationWizardBase)emoteWizardRoot.GetWizard<FxWizard>() : emoteWizardRoot.GetWizard<GestureWizard>();

                EmoteWizardGUILayout.SetupOnlyUI(animationWizardBase, () =>
                {
                    if (GUILayout.Button("Repopulate HandSigns: 7 items"))
                    {
                        _multiEmoteSource.RepopulateDefaultEmotes();
                    }

                    if (_multiEmoteSource.LayerName == EmoteWizardConstants.LayerNames.Fx)
                    {
                        if (GUILayout.Button("Repopulate HandSigns: 14 items"))
                        {
                            _multiEmoteSource.RepopulateDefaultEmotes14();
                        }
                    }
                });

                var advancedAnimations = _multiEmoteSource.AdvancedAnimations;
                using (new EditorGUI.DisabledScope(_multiEmoteSource.HasComplexAnimations))
                {
                    TypedGUILayout.Toggle("Advanced Animations", ref advancedAnimations);
                    if (advancedAnimations != _multiEmoteSource.AdvancedAnimations)
                    {
                        _multiEmoteSource.AdvancedAnimations = advancedAnimations;
                    }
                }

                using (new EmoteDrawerContext(emoteWizardRoot, parametersWizard, animationWizardBase.LayerName, _multiEmoteSource.AdvancedAnimations, _emotesState).StartContext())
                {
                    _emoteList.DrawAsProperty(_multiEmoteSource.emotes, emoteWizardRoot.listDisplayMode);
                }
            }
            
            if (GUILayout.Button("Explode"))
            {
                SourceExploder.Explode(_multiEmoteSource);
            }

            EmoteWizardGUILayout.Tutorial(emoteWizardRoot, Tutorial);
        }

        static string Tutorial => 
            string.Join("\n",
                "ハンドサインに基づくアニメーションの設定をします。",
                "");
    }
}