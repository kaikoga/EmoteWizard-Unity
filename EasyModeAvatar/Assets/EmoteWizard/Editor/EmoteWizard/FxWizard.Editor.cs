using System.Linq;
using EmoteWizard.Base;
using EmoteWizard.DataObjects;
using EmoteWizard.Extensions;
using UnityEditor;
using UnityEngine;

namespace EmoteWizard
{
    [CustomEditor(typeof(FxWizard))]
    public class FxWizardEditor : AnimationWizardBaseEditor
    {
        FxWizard fxWizard;

        void OnEnable()
        {
            fxWizard = target as FxWizard;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Repopulate Emotes"))
            {
                RepopulateDefaultFxEmotes(fxWizard);
            }
            if (GUILayout.Button("Generate Animation Controller"))
            {
                BuildAnimatorController("FX/GeneratedFX.controller", animatorController =>
                {
                    var resetLayer = PopulateLayer(animatorController, "Reset"); 
                    BuildStaticStateMachine(resetLayer.stateMachine, "Reset", fxWizard.EnsureResetClip());

                    var allPartsLayer = PopulateLayer(animatorController, "AllParts"); 
                    BuildStaticStateMachine(allPartsLayer.stateMachine, "Global", fxWizard.EnsureGlobalClip());

                    var leftHandLayer = PopulateLayer(animatorController, "Left Hand"); 
                    BuildStateMachine(leftHandLayer.stateMachine, true);
            
                    var rightHandLayer = PopulateLayer(animatorController, "Right Hand"); 
                    BuildStateMachine(rightHandLayer.stateMachine, false);
                    
                });
            }
        }

        static void RepopulateDefaultFxEmotes(AnimationWizardBase wizard)
        {
            var newEmotes = Enumerable.Empty<Emote>()
                .Concat(Emote.HandSigns
                    .Select(handSign => new Emote
                    {
                        gesture1 = EmoteGestureCondition.Populate(handSign, GestureParameter.Gesture),
                        gesture2 = EmoteGestureCondition.Populate(handSign, GestureParameter.GestureOther),
                    }))
                .Concat(Emote.HandSigns
                    .Select(handSign => new Emote
                    {
                        gesture1 = EmoteGestureCondition.Populate(handSign, GestureParameter.Gesture),
                        gesture2 = EmoteGestureCondition.Populate(handSign, GestureParameter.GestureOther, GestureConditionMode.NotEqual),
                    }))
                .ToList();
            wizard.emotes = newEmotes;
        }
    }
}