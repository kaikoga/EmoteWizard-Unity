using Silksprite.EmoteWizard.Collections;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.DataObjects.DrawerContexts;
using Silksprite.EmoteWizard.DataObjects.DrawerStates;
using Silksprite.EmoteWizard.Sources.Impl;
using Silksprite.EmoteWizard.Sources.Impl.Multi;
using Silksprite.EmoteWizard.Sources.Multi.Extensions;
using Silksprite.EmoteWizard.UI;
using Silksprite.EmoteWizard.Utils;
using Silksprite.EmoteWizardSupport.Collections.Generic;
using Silksprite.EmoteWizardSupport.Extensions;
using Silksprite.EmoteWizardSupport.Scopes;
using Silksprite.EmoteWizardSupport.Utils;
using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizard.Sources.Multi
{
    [CustomEditor(typeof(MultiActionEmoteSource))]
    public class MultiActionEmoteSourceEditor : Editor
    {
        MultiActionEmoteSource _multiActionEmoteSource;

        ActionEmoteDrawerState _actionEmotesState;

        ExpandableReorderableList<ActionEmote> _actionEmotesList;

        void OnEnable()
        {
            _multiActionEmoteSource = (MultiActionEmoteSource)target;
            
            _actionEmotesState = new ActionEmoteDrawerState();

            _actionEmotesList = new ExpandableReorderableList<ActionEmote>(new ActionEmoteListHeaderDrawer(), new ActionEmoteDrawer(), "Action Emotes", ref _multiActionEmoteSource.actionEmotes);
        }

        public override void OnInspectorGUI()
        {
            var emoteWizardRoot = _multiActionEmoteSource.EmoteWizardRoot;
            if (emoteWizardRoot.showCopyPasteJsonButtons) this.CopyPasteJsonButtons();

            using (new ObjectChangeScope(_multiActionEmoteSource))
            {
                var actionWizard = emoteWizardRoot.GetWizard<ActionWizard>();

                EmoteWizardGUILayout.SetupOnlyUI(_multiActionEmoteSource, () =>
                {
                    if (GUILayout.Button("Repopulate Default Actions"))
                    {
                        _multiActionEmoteSource.RepopulateDefaultActionEmotes();
                    }
                });
                using (new ActionEmoteDrawerContext(emoteWizardRoot, _actionEmotesState, actionWizard.fixedTransitionDuration, false).StartContext())
                {
                    _actionEmotesList.DrawAsProperty(_multiActionEmoteSource.actionEmotes, emoteWizardRoot.listDisplayMode);
                }
            }

            if (GUILayout.Button("Explode"))
            {
                SourceExploder.Explode(_multiActionEmoteSource);
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