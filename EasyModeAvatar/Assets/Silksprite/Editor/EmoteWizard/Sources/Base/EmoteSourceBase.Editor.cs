using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.Collections;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.DataObjects.DrawerContexts;
using Silksprite.EmoteWizard.DataObjects.DrawerStates;
using Silksprite.EmoteWizard.Sources.Extensions;
using Silksprite.EmoteWizard.UI;
using Silksprite.EmoteWizardSupport.Collections.Generic;
using Silksprite.EmoteWizardSupport.Scopes;
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
            var parametersWizard = emoteWizardRoot.GetWizard<ParametersWizard>();
            var animationWizardBase = _emoteSource.LayerName == "FX" ? (AnimationWizardBase)emoteWizardRoot.GetWizard<FxWizard>() : emoteWizardRoot.GetWizard<GestureWizard>();
            using (new ObjectChangeScope(_emoteSource))
            {
                EmoteWizardGUILayout.SetupOnlyUI(animationWizardBase, () =>
                {
                    if (GUILayout.Button("Repopulate HandSigns: 7 items"))
                    {
                        _emoteSource.RepopulateDefaultEmotes();
                    }

                    if (_emoteSource.LayerName == "FX")
                    {
                        if (GUILayout.Button("Repopulate HandSigns: 14 items"))
                        {
                            _emoteSource.RepopulateDefaultEmotes14();
                        }
                    }
                });

                using (new EmoteDrawerContext(emoteWizardRoot, parametersWizard, animationWizardBase.LayerName, animationWizardBase.advancedAnimations, _emotesState).StartContext())
                {
                    _emoteList.DrawAsProperty(_emoteSource.emotes, emoteWizardRoot.listDisplayMode);
                }
            }
        }
    }
}