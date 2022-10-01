using System.Linq;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.Collections;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.DataObjects.DrawerContexts;
using Silksprite.EmoteWizard.DataObjects.DrawerStates;
using Silksprite.EmoteWizard.Sources.Extensions;
using Silksprite.EmoteWizard.Sources.Impl.Base;
using Silksprite.EmoteWizard.UI;
using Silksprite.EmoteWizardSupport.Collections.Generic;
using Silksprite.EmoteWizardSupport.Extensions;
using Silksprite.EmoteWizardSupport.Scopes;
using Silksprite.EmoteWizardSupport.UI;
using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizard.Sources.Base
{
    [CustomEditor(typeof(EmoteSourceBase), true)]
    public class EmoteSourceEditor : Editor
    {
        EmoteSourceBase _emoteSource;

        EmoteDrawerState _emotesState;

        ExpandableReorderableList<Emote> _emoteList;

        void OnEnable()
        {
            _emoteSource = (EmoteSourceBase)target;

            _emotesState = new EmoteDrawerState();

            _emoteList = new ExpandableReorderableList<Emote>(new EmoteListHeaderDrawer(), new EmoteDrawer(), "HandSign Emotes", ref _emoteSource.emotes);
        }

        public override void OnInspectorGUI()
        {
            var emoteWizardRoot = _emoteSource.EmoteWizardRoot;
            if (emoteWizardRoot.showCopyPasteJsonButtons) this.CopyPasteJsonButtons();

            using (new ObjectChangeScope(_emoteSource))
            {
                var parametersWizard = emoteWizardRoot.GetWizard<ParametersWizard>();
                var animationWizardBase = _emoteSource.LayerName == EmoteWizardConstants.LayerNames.Fx ? (AnimationWizardBase)emoteWizardRoot.GetWizard<FxWizard>() : emoteWizardRoot.GetWizard<GestureWizard>();

                EmoteWizardGUILayout.SetupOnlyUI(animationWizardBase, () =>
                {
                    if (GUILayout.Button("Repopulate HandSigns: 7 items"))
                    {
                        _emoteSource.RepopulateDefaultEmotes();
                    }

                    if (_emoteSource.LayerName == EmoteWizardConstants.LayerNames.Fx)
                    {
                        if (GUILayout.Button("Repopulate HandSigns: 14 items"))
                        {
                            _emoteSource.RepopulateDefaultEmotes14();
                        }
                    }
                });

                var advancedAnimations = _emoteSource.AdvancedAnimations;
                using (new EditorGUI.DisabledScope(_emoteSource.HasComplexAnimations))
                {
                    TypedGUILayout.Toggle("Advanced Animations", ref advancedAnimations);
                    if (advancedAnimations != _emoteSource.AdvancedAnimations)
                    {
                        _emoteSource.AdvancedAnimations = advancedAnimations;
                    }
                }

                using (new EmoteDrawerContext(emoteWizardRoot, parametersWizard, animationWizardBase.LayerName, _emoteSource.AdvancedAnimations, _emotesState).StartContext())
                {
                    _emoteList.DrawAsProperty(_emoteSource.emotes, emoteWizardRoot.listDisplayMode);
                }
            }
            
            EmoteWizardGUILayout.Tutorial(emoteWizardRoot, Tutorial);
        }

        static string Tutorial => 
            string.Join("\n",
                "ハンドサインに基づくアニメーションの設定をします。",
                "");
    }
}