using Silksprite.EmoteWizard.Collections;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.DataObjects.DrawerContexts;
using Silksprite.EmoteWizard.DataObjects.DrawerStates;
using Silksprite.EmoteWizard.Sources.Extensions;
using Silksprite.EmoteWizard.UI;
using Silksprite.EmoteWizardSupport.Collections.Generic;
using Silksprite.EmoteWizardSupport.Extensions;
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
            if (emoteWizardRoot.showCopyPasteJsonButtons) this.CopyPasteJsonButtons();

            var parametersWizard = emoteWizardRoot.EnsureWizard<ParametersWizard>();
            using (new ObjectChangeScope(_parameterEmoteSource))
            {
                EmoteWizardGUILayout.SetupOnlyUI(_parameterEmoteSource, () =>
                {
                    if (GUILayout.Button("Repopulate Parameters"))
                    {
                        _parameterEmoteSource.RepopulateParameterEmotes(parametersWizard);
                    }
                });

                using (new ParameterEmoteDrawerContext(emoteWizardRoot, _parameterEmoteSource, parametersWizard, _parameterEmoteSource.LayerName, _parametersState).StartContext())
                {
                    _parameterEmoteList.DrawAsProperty(_parameterEmoteSource.parameterEmotes, emoteWizardRoot.listDisplayMode);
                }

                if (IsExpandedTracker.GetIsExpanded(_parameterEmoteSource.parameterEmotes))
                {
                    if (GUILayout.Button("Generate Parameters"))
                    {
                        _parameterEmoteSource.GenerateParameters(parametersWizard);
                    }
                }
            }
        }
    }
}