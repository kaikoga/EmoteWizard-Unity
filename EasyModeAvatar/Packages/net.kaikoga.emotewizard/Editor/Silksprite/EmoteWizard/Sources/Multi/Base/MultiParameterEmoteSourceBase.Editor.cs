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
    [CustomEditor(typeof(MultiParameterEmoteSourceBase), true)]
    public class MultiParameterEmoteSourceBaseEditor : Editor
    {
        MultiParameterEmoteSourceBase _multiParameterEmoteSource;

        ParameterEmoteDrawerState _parametersState;

        ExpandableReorderableList<ParameterEmote> _parameterEmoteList;

        void OnEnable()
        {
            _multiParameterEmoteSource = (MultiParameterEmoteSourceBase)target;

            _parametersState = new ParameterEmoteDrawerState();

            _parameterEmoteList = new ExpandableReorderableList<ParameterEmote>(new ParameterEmoteListHeaderDrawer(), new ParameterEmoteDrawer(), "Parameter Emotes", ref _multiParameterEmoteSource.parameterEmotes);
        }

        public override void OnInspectorGUI()
        {
            var emoteWizardRoot = _multiParameterEmoteSource.EmoteWizardRoot;
            if (emoteWizardRoot.showCopyPasteJsonButtons) this.CopyPasteJsonButtons();

            using (new ObjectChangeScope(_multiParameterEmoteSource))
            {
                var parametersWizard = emoteWizardRoot.EnsureWizard<ParametersWizard>();

                EmoteWizardGUILayout.SetupOnlyUI(_multiParameterEmoteSource, () =>
                {
                    if (GUILayout.Button("Repopulate Parameters"))
                    {
                        _multiParameterEmoteSource.RepopulateParameterEmotes(parametersWizard);
                    }
                });

                using (new ParameterEmoteDrawerContext(emoteWizardRoot, _multiParameterEmoteSource, parametersWizard, _multiParameterEmoteSource.LayerName, _parametersState).StartContext())
                {
                    _parameterEmoteList.DrawAsProperty(_multiParameterEmoteSource.parameterEmotes, emoteWizardRoot.listDisplayMode);
                }

                if (IsExpandedTracker.GetIsExpanded(_multiParameterEmoteSource.parameterEmotes))
                {
                    if (GUILayout.Button("Generate Parameters"))
                    {
                        _multiParameterEmoteSource.GenerateParameters(parametersWizard);
                    }
                }
            }
            
            if (GUILayout.Button("Explode"))
            {
                SourceExploder.Explode(_multiParameterEmoteSource);
            }

            EmoteWizardGUILayout.Tutorial(emoteWizardRoot, Tutorial);
        }

        static string Tutorial => 
            string.Join("\n",
                "パラメーターに基づくアニメーションの設定をします。",
                "");
    }
}