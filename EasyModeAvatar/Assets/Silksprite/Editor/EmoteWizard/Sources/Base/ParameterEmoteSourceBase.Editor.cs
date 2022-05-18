using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.Collections;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.DataObjects.DrawerContexts;
using Silksprite.EmoteWizard.DataObjects.DrawerStates;
using Silksprite.EmoteWizard.Sources.Extensions;
using Silksprite.EmoteWizard.UI;
using Silksprite.EmoteWizardSupport.Collections.Generic;
using Silksprite.EmoteWizardSupport.Scopes;
using Silksprite.EmoteWizardSupport.UI;
using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizard.Sources.Base
{
    [CustomEditor(typeof(ParameterEmoteSourceBase), true)]
    public class ParameterEmoteSourceBaseEditor : Editor
    {
        ParameterEmoteSourceBase _parameterEmoteSource;

        ParameterEmoteDrawerState _parametersState;

        ExpandableReorderableList<ParameterEmote> _parameterEmoteList;

        void OnEnable()
        {
            _parameterEmoteSource = (ParameterEmoteSourceBase)target;

            _parametersState = new ParameterEmoteDrawerState();

            _parameterEmoteList = new ExpandableReorderableList<ParameterEmote>(new ParameterEmoteListHeaderDrawer(), new ParameterEmoteDrawer(), "Parameter Emotes", ref _parameterEmoteSource.parameterEmotes);
        }

        public override void OnInspectorGUI()
        {
            var emoteWizardRoot = _parameterEmoteSource.EmoteWizardRoot;
            var parametersWizard = emoteWizardRoot.GetWizard<ParametersWizard>();
            var animationWizardBase = _parameterEmoteSource.LayerName == "FX" ? (AnimationWizardBase)emoteWizardRoot.GetWizard<FxWizard>() : emoteWizardRoot.GetWizard<GestureWizard>();
            using (new ObjectChangeScope(_parameterEmoteSource))
            {
                EmoteWizardGUILayout.SetupOnlyUI(_parameterEmoteSource, () =>
                {
                    if (parametersWizard != null)
                    {
                        if (GUILayout.Button("Repopulate Parameters"))
                        {
                            _parameterEmoteSource.RepopulateParameterEmotes(parametersWizard);
                        }
                    }
                });

                using (new ParameterEmoteDrawerContext(emoteWizardRoot, animationWizardBase, parametersWizard, _parameterEmoteSource.LayerName, _parametersState).StartContext())
                {
                    _parameterEmoteList.DrawAsProperty(_parameterEmoteSource.parameterEmotes, emoteWizardRoot.listDisplayMode);
                }

                if (IsExpandedTracker.GetIsExpanded(_parameterEmoteSource.parameterEmotes))
                {
                    EmoteWizardGUILayout.RequireAnotherWizard(animationWizardBase, parametersWizard, () =>
                    {
                        if (GUILayout.Button("Generate Parameters"))
                        {
                            parametersWizard.TryRefreshParameters();
                            _parameterEmoteSource.GenerateParameters(parametersWizard, animationWizardBase);
                        }
                    });
                }
            }
        }
    }
}