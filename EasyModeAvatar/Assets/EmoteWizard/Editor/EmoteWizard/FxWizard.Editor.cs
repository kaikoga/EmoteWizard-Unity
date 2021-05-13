using EmoteWizard.Base;
using UnityEditor;
using UnityEditor.Animations;
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
                    BuildStaticStateMachine(resetLayer.stateMachine, "Reset", fxWizard.resetClip);

                    var allPartsLayer = PopulateLayer(animatorController, "AllParts"); 
                    BuildStaticStateMachine(allPartsLayer.stateMachine, "Global", fxWizard.globalClip);

                    var leftHandLayer = PopulateLayer(animatorController, "Left Hand"); 
                    BuildStateMachine(leftHandLayer.stateMachine, true);
            
                    var rightHandLayer = PopulateLayer(animatorController, "Right Hand"); 
                    BuildStateMachine(rightHandLayer.stateMachine, false);
                    
                });
            }
        }
    }
}