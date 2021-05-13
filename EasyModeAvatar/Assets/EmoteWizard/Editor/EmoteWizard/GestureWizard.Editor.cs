using EmoteWizard.Base;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace EmoteWizard
{
    [CustomEditor(typeof(GestureWizard))]
    public class GestureWizardEditor : AnimationWizardBaseEditor
    {
        GestureWizard gestureWizard;

        void OnEnable()
        {
            gestureWizard = target as GestureWizard;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Repopulate Emotes"))
            {
                RepopulateDefaultGestureEmotes(gestureWizard);
            }
            if (GUILayout.Button("Generate Animation Controller"))
            {
                BuildAnimatorController("Gesture/GeneratedGesture.controller", animatorController =>
                {
                    var allPartsLayer = PopulateLayer(animatorController, "Reset"); 
                    BuildStaticStateMachine(allPartsLayer.stateMachine, "Global", null);

                    var leftHandLayer = PopulateLayer(animatorController, "Left Hand"); 
                    BuildStateMachine(leftHandLayer.stateMachine, true);
            
                    var rightHandLayer = PopulateLayer(animatorController, "Right Hand"); 
                    BuildStateMachine(rightHandLayer.stateMachine, false);
                });
            }
        }
    }
}