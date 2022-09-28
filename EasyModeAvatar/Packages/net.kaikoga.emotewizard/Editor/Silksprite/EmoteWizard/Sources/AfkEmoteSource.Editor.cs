using Silksprite.EmoteWizard.Collections;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.DataObjects.DrawerContexts;
using Silksprite.EmoteWizard.DataObjects.DrawerStates;
using Silksprite.EmoteWizard.UI;
using Silksprite.EmoteWizardSupport.Collections.Generic;
using Silksprite.EmoteWizardSupport.Extensions;
using Silksprite.EmoteWizardSupport.Scopes;
using UnityEditor;

namespace Silksprite.EmoteWizard.Sources
{
    [CustomEditor(typeof(AfkEmoteSource))]
    public class AfkEmoteSourceEditor : Editor
    {
        AfkEmoteSource _afkEmoteSource;

        ActionEmoteDrawerState _afkEmotesState;

        ExpandableReorderableList<ActionEmote> _afkEmotesList;

        void OnEnable()
        {
            _afkEmoteSource = (AfkEmoteSource)target;
            
            _afkEmotesState = new ActionEmoteDrawerState();

            _afkEmotesList = new ExpandableReorderableList<ActionEmote>(new ActionEmoteListHeaderDrawer(), new ActionEmoteDrawer(), "AFK Emotes", ref _afkEmoteSource.afkEmotes);
        }

        public override void OnInspectorGUI()
        {
            var emoteWizardRoot = _afkEmoteSource.EmoteWizardRoot;
            if (emoteWizardRoot.showCopyPasteJsonButtons) this.CopyPasteJsonButtons();

            using (new ObjectChangeScope(_afkEmoteSource))
            {
                var actionWizard = emoteWizardRoot.GetWizard<ActionWizard>();

                using (new ActionEmoteDrawerContext(emoteWizardRoot, _afkEmotesState, actionWizard.fixedTransitionDuration, false).StartContext())
                using (new EditorGUI.DisabledScope(!actionWizard.afkSelectEnabled))
                {
                    _afkEmotesList.DrawAsProperty(_afkEmoteSource.afkEmotes, emoteWizardRoot.listDisplayMode);
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