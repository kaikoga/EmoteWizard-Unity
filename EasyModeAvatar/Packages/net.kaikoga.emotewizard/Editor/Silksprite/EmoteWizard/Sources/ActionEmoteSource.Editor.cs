using Silksprite.EmoteWizard.Collections;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.DataObjects.DrawerContexts;
using Silksprite.EmoteWizard.DataObjects.DrawerStates;
using Silksprite.EmoteWizard.Sources.Extensions;
using Silksprite.EmoteWizard.Sources.Impl;
using Silksprite.EmoteWizard.UI;
using Silksprite.EmoteWizardSupport.Collections.Generic;
using Silksprite.EmoteWizardSupport.Extensions;
using Silksprite.EmoteWizardSupport.Scopes;
using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizard.Sources
{
    [CustomEditor(typeof(ActionEmoteSource))]
    public class ActionEmoteSourceEditor : Editor
    {
        ActionEmoteSource _actionEmoteSource;

        ActionEmoteDrawerState _actionEmotesState;

        ExpandableReorderableList<ActionEmote> _actionEmotesList;

        void OnEnable()
        {
            _actionEmoteSource = (ActionEmoteSource)target;
            
            _actionEmotesState = new ActionEmoteDrawerState();

            _actionEmotesList = new ExpandableReorderableList<ActionEmote>(new ActionEmoteListHeaderDrawer(), new ActionEmoteDrawer(), "Action Emotes", ref _actionEmoteSource.actionEmotes);
        }

        public override void OnInspectorGUI()
        {
            var emoteWizardRoot = _actionEmoteSource.EmoteWizardRoot;
            if (emoteWizardRoot.showCopyPasteJsonButtons) this.CopyPasteJsonButtons();

            using (new ObjectChangeScope(_actionEmoteSource))
            {
                var actionWizard = emoteWizardRoot.GetWizard<ActionWizard>();

                EmoteWizardGUILayout.SetupOnlyUI(_actionEmoteSource, () =>
                {
                    if (GUILayout.Button("Repopulate Default Actions"))
                    {
                        _actionEmoteSource.RepopulateDefaultActionEmotes();
                    }
                });
                using (new ActionEmoteDrawerContext(emoteWizardRoot, _actionEmotesState, actionWizard.fixedTransitionDuration, false).StartContext())
                {
                    _actionEmotesList.DrawAsProperty(_actionEmoteSource.actionEmotes, emoteWizardRoot.listDisplayMode);
                }
            }

            EmoteWizardGUILayout.Tutorial(emoteWizardRoot, Tutorial);
        }

        static string Tutorial =>
            string.Join("\n",
                "エモートモーションを登録します。",
                "",
                "Select Value: Action Select Parameterの値",
                "Has Exit Time: オンの場合、一回再生のアニメーションとして適用されます。オフの場合、ループアニメーションとして適用されます");
    }
}