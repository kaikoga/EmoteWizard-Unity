using Silksprite.EmoteWizard.Collections;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.DataObjects.DrawerContexts;
using Silksprite.EmoteWizard.DataObjects.DrawerStates;
using Silksprite.EmoteWizard.Sources.Impl;
using Silksprite.EmoteWizard.Sources.Impl.Multi;
using Silksprite.EmoteWizard.UI;
using Silksprite.EmoteWizardSupport.Collections.Generic;
using Silksprite.EmoteWizardSupport.Extensions;
using Silksprite.EmoteWizardSupport.Scopes;
using UnityEditor;

namespace Silksprite.EmoteWizard.Sources.Multi
{
    [CustomEditor(typeof(MultiAfkEmoteSource))]
    public class MultiAfkEmoteSourceEditor : Editor
    {
        MultiAfkEmoteSource _multiAfkEmoteSource;

        ActionEmoteDrawerState _afkEmotesState;

        ExpandableReorderableList<ActionEmote> _afkEmotesList;

        void OnEnable()
        {
            _multiAfkEmoteSource = (MultiAfkEmoteSource)target;
            
            _afkEmotesState = new ActionEmoteDrawerState();

            _afkEmotesList = new ExpandableReorderableList<ActionEmote>(new ActionEmoteListHeaderDrawer(), new ActionEmoteDrawer(), "AFK Emotes", ref _multiAfkEmoteSource.afkEmotes);
        }

        public override void OnInspectorGUI()
        {
            var emoteWizardRoot = _multiAfkEmoteSource.EmoteWizardRoot;
            if (emoteWizardRoot.showCopyPasteJsonButtons) this.CopyPasteJsonButtons();

            using (new ObjectChangeScope(_multiAfkEmoteSource))
            {
                var actionWizard = emoteWizardRoot.GetWizard<ActionWizard>();

                using (new ActionEmoteDrawerContext(emoteWizardRoot, _afkEmotesState, actionWizard.fixedTransitionDuration, false).StartContext())
                using (new EditorGUI.DisabledScope(!actionWizard.afkSelectEnabled))
                {
                    _afkEmotesList.DrawAsProperty(_multiAfkEmoteSource.afkEmotes, emoteWizardRoot.listDisplayMode);
                }
            }

            EmoteWizardGUILayout.Tutorial(emoteWizardRoot, Tutorial);
        }

        static string Tutorial =>
            string.Join("\n",
                "カスタムAFKモーションを登録します。",
                "",
                "Select Value: AFK Select Parameterの値",
                "Has Exit Time: オンの場合、一回再生のアニメーションとして適用されます。オフの場合、ループアニメーションとして適用されます");
    }
}