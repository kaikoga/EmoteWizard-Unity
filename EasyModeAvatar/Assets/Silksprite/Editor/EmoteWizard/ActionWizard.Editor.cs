using Silksprite.EmoteWizard.Collections;
using Silksprite.EmoteWizard.DataObjects.DrawerContexts;
using Silksprite.EmoteWizard.DataObjects.DrawerStates;
using Silksprite.EmoteWizard.Extensions;
using Silksprite.EmoteWizard.UI;
using Silksprite.EmoteWizardSupport.Extensions;
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

        ActionEmoteDrawerState defaultAfkEmoteState;

        void OnEnable()
        {
            actionWizard = (ActionWizard) target;
            
            defaultAfkEmoteState = new ActionEmoteDrawerState();
        }

        public override void OnInspectorGUI()
        {
            var emoteWizardRoot = actionWizard.EmoteWizardRoot;
            if (emoteWizardRoot.showCopyPasteJsonButtons) this.CopyPasteJsonButtons();

            using (new ObjectChangeScope(actionWizard))
            {
                var parametersWizard = actionWizard.EmoteWizardRoot.GetWizard<ParametersWizard>();

                TypedGUILayout.Toggle("Fixed Transition Duration", ref actionWizard.fixedTransitionDuration);

                using (new InvalidValueScope(parametersWizard.IsInvalidParameter(actionWizard.actionSelectParameter)))
                {
                    TypedGUILayout.TextField("Action Select Parameter", ref actionWizard.actionSelectParameter);
                }

                TypedGUILayout.Toggle("AFK Select Enabled", ref actionWizard.afkSelectEnabled);
                if (actionWizard.afkSelectEnabled)
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

                GUILayout.Label("Default AFK Emote");
                using (new ActionEmoteDrawerContext(emoteWizardRoot, defaultAfkEmoteState, actionWizard.fixedTransitionDuration, true).StartContext())
                {
                    new ActionEmoteListHeaderDrawer().OnGUI(false);
                    using (new EditorGUI.IndentLevelScope())
                    {
                        TypedGUILayout.TypedField(ref actionWizard.defaultAfkEmote, "Default AFK Emote");
                    }
                }

                EmoteWizardGUILayout.OutputUIArea(() =>
                {
                    if (GUILayout.Button("Generate Animation Controller"))
                    {
                        actionWizard.BuildOutputAsset();
                    }

                    TypedGUILayout.AssetField("Output Asset", ref actionWizard.outputAsset);
                });
            }

            EmoteWizardGUILayout.Tutorial(emoteWizardRoot, Tutorial);
        }

        static string Tutorial =>
            string.Join("\n",
                "Action Layerの設定を行い、AnimationControllerを生成します。");
    }
}