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
            using (new ObjectChangeScope(_actionEmoteSource))
            {
                var emoteWizardRoot = _actionEmoteSource.EmoteWizardRoot;
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
        }
    }
}