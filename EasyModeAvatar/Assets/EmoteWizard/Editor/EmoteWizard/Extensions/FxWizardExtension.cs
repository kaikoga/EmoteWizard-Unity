using UnityEngine;

using static EmoteWizard.Tools.EmoteWizardEditorTools;

namespace EmoteWizard.Extensions
{
    public static class FxWizardExtension
    {
        public static AnimationClip EnsureResetClip(this FxWizard fxWizard)
        {
            return EnsureAnimationClip(fxWizard.EmoteWizardRoot, "FX/GeneratedResetFX.anim", ref fxWizard.resetClip);
        }
    }
}