using UnityEngine;

namespace EmoteWizard.Extensions
{
    public static class FxWizardExtension
    {
        public static AnimationClip EnsureResetClip(this FxWizard fxWizard)
        {
            return fxWizard.EmoteWizardRoot.EnsureAnimationClip("FX/GeneratedResetFX.anim", ref fxWizard.resetClip);
        }
    }
}