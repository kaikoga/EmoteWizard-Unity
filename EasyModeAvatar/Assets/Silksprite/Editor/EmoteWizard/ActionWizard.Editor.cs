using System.Linq;
using Silksprite.EmoteWizard.Collections;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.DataObjects.DrawerContexts;
using Silksprite.EmoteWizard.DataObjects.DrawerStates;
using Silksprite.EmoteWizard.Extensions;
using Silksprite.EmoteWizard.UI;
using Silksprite.EmoteWizardSupport.Collections.Generic;
using Silksprite.EmoteWizardSupport.Scopes;
using Silksprite.EmoteWizardSupport.UI;
using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizard
{
    [CustomEditor(typeof(ActionWizard))]
    public class ActionWizardEditor : Editor
    {
        ActionWizard actionWizard;

        ActionEmoteDrawerState actionEmotesState;
        ActionEmoteDrawerState afkEmotesState;

        ExpandableReorderableList<ActionEmote> actionEmotesList;
        ExpandableReorderableList<ActionEmote> afkEmotesList;

        void OnEnable()
        {
            actionWizard = (ActionWizard) target;
            
            actionEmotesState = new ActionEmoteDrawerState();
            afkEmotesState = new ActionEmoteDrawerState();

            actionEmotesList = new ExpandableReorderableList<ActionEmote>(new ActionEmoteListHeaderDrawer(), new ActionEmoteDrawer(), "Action Emotes", ref actionWizard.actionEmotes);
            afkEmotesList = new ExpandableReorderableList<ActionEmote>(new ActionEmoteListHeaderDrawer(), new ActionEmoteDrawer(), "AFK Emotes", ref actionWizard.afkEmotes);
        }

        public override void OnInspectorGUI()
        {
            var parametersWizard = actionWizard.EmoteWizardRoot.GetWizard<ParametersWizard>();
            using (new ObjectChangeScope(actionWizard))
            {
                var emoteWizardRoot = actionWizard.EmoteWizardRoot;

                EmoteWizardGUILayout.SetupOnlyUI(actionWizard, () =>
                {
                    if (GUILayout.Button("Repopulate Default Actions"))
                    {
                        actionWizard.RepopulateDefaultActionEmotes();
                    }
                });

                TypedGUILayout.ToggleLeft("Fixed Transition Duration", ref actionWizard.fixedTransitionDuration);

                using (new InvalidValueScope(parametersWizard.IsInvalidParameter(actionWizard.actionSelectParameter)))
                {
                    TypedGUILayout.TextField("Action Select Parameter", ref actionWizard.actionSelectParameter);
                }
                using (new ActionEmoteDrawerContext(emoteWizardRoot, actionEmotesState, actionWizard.fixedTransitionDuration, null).StartContext())
                {
                    actionEmotesList.DrawAsProperty(actionWizard.actionEmotes, emoteWizardRoot.listDisplayMode);
                }

                TypedGUILayout.Toggle("AFK Select Enabled", ref actionWizard.afkSelectEnabled);
                if (actionWizard.SelectableAfkEmotes)
                {
                    using (new InvalidValueScope(parametersWizard.IsInvalidParameter(actionWizard.afkSelectParameter)))
                    {
                        TypedGUILayout.TextField("AFK Select Parameter", ref actionWizard.afkSelectParameter);
                    }
                }
                else
                {
                    using (new EditorGUI.DisabledScope(true))
                    {
                        TypedGUILayout.TextField("AFK Select Parameter", ref actionWizard.afkSelectParameter);
                    }
                }

                using (new ActionEmoteDrawerContext(emoteWizardRoot, afkEmotesState, actionWizard.fixedTransitionDuration, actionWizard.afkEmotes.LastOrDefault()).StartContext())
                {
                    afkEmotesList.DrawAsProperty(actionWizard.afkEmotes, emoteWizardRoot.listDisplayMode);
                }

                EmoteWizardGUILayout.OutputUIArea(() =>
                {
                    if (GUILayout.Button("Generate Animation Controller"))
                    {
                        actionWizard.BuildOutputAsset();
                    }

                    TypedGUILayout.AssetField("Output Asset", ref actionWizard.outputAsset);
                });

                EmoteWizardGUILayout.Tutorial(emoteWizardRoot, $"Action Layerの設定を行い、AnimationControllerを生成します。\n{Tutorial}");
            }
        }

        static string Tutorial =>
            string.Join("\n",
                "Write Defaultsはオフになります。",
                "Select Value: モーションを再生するために必要なAction Select ParameterかAFK Select Parameterの値。1以上である必要があります",
                "Has Exit Time: オンの場合、一回再生のアニメーションとして適用されます。オフの場合、ループアニメーションとして適用されます",
                "",
                "Action Emotes: Action Select Parameterによって制御されるモーション",
                "AFK Emotes: AFK Select Parameterによって制御されるAFKモーション。Select Valueが未設定の場合は、一番下のモーションが再生されます");
    }
}