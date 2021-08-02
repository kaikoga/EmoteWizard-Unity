using System;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.Collections;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.DataObjects.DrawerContexts;
using Silksprite.EmoteWizard.DataObjects.DrawerStates;
using Silksprite.EmoteWizard.Extensions;
using Silksprite.EmoteWizard.Internal;
using Silksprite.EmoteWizard.UI;
using Silksprite.EmoteWizard.Utils;
using Silksprite.EmoteWizardSupport.Collections.Generic;
using Silksprite.EmoteWizardSupport.Scopes;
using Silksprite.EmoteWizardSupport.UI;
using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizard
{
    [CustomEditor(typeof(ActionWizard))]
    public class ActionWizardEditor : AnimationWizardBaseEditor
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
                        SetupWizardUtils.RepopulateDefaultActionEmotes(actionWizard);
                    }
                });

                TypedGUILayout.ToggleLeft("Fixed Transition Duration", ref actionWizard.fixedTransitionDuration);

                using (new ActionEmoteDrawerContext(emoteWizardRoot).StartContext())
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

                using (new ActionEmoteDrawerContext(emoteWizardRoot).StartContext())
                {
                    afkEmotesList.DrawAsProperty(actionWizard.afkEmotes, emoteWizardRoot.listDisplayMode);
                }

                EmoteWizardGUILayout.OutputUIArea(() =>
                {
                    if (GUILayout.Button("Generate Animation Controller"))
                    {
                        var builder = new ActionControllerBuilder
                        {
                            ActionWizard = actionWizard,
                            DefaultRelativePath = "Action/@@@Generated@@@Action.controller"
                        };

                        builder.BuildActionLayer();
                        builder.BuildParameters();
                    }

                    TypedGUILayout.AssetField("Output Asset", ref actionWizard.outputAsset);
                });

                EmoteWizardGUILayout.Tutorial(emoteWizardRoot, $"Action Layerの設定を行い、AnimationControllerを生成します。\n{Tutorial}");
            }
        }
    }
}