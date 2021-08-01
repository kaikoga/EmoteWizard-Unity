using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.Collections;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.DataObjects.DrawerContexts;
using Silksprite.EmoteWizard.DataObjects.DrawerStates;
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

        ExpandableReorderableList<ActionEmote> actionEmotesList;

        void OnEnable()
        {
            actionWizard = (ActionWizard) target;
            
            actionEmotesState = new ActionEmoteDrawerState();

            actionEmotesList = new ExpandableReorderableList<ActionEmote>(new ActionEmoteListHeaderDrawer(), new ActionEmoteDrawer(), "Action Emotes", ref actionWizard.actionEmotes);

        }

        public override void OnInspectorGUI()
        {
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

                using (new ActionEmoteDrawerContext(emoteWizardRoot).StartContext())
                {
                    actionEmotesList.DrawAsProperty(actionWizard.actionEmotes, emoteWizardRoot.listDisplayMode);
                }

                EmoteWizardGUILayout.OutputUIArea(() =>
                {
                    if (GUILayout.Button("Generate Animation Controller"))
                    {
                        var builder = new AnimationControllerBuilder
                        {
                            AnimationWizardBase = null,
                            ParametersWizard = null,
                            DefaultRelativePath = "FX/@@@Generated@@@FX.controller"
                        };

                        // builder.BuildStaticLayer("Reset", resetClip, null);
                    }

                    TypedGUILayout.AssetField("Output Asset", ref actionWizard.outputAsset);
                });

                EmoteWizardGUILayout.Tutorial(emoteWizardRoot, $"Action Layerの設定を行い、AnimationControllerを生成します。\n{Tutorial}");
            }
        }
    }
}