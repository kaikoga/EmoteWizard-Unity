using UnityEngine;

namespace EmoteWizard.Extensions
{
    public static class FxWizardExtension
    {
        public static AnimationClip ProvideResetClip(this FxWizard fxWizard)
        {
            return fxWizard.EmoteWizardRoot.EnsureAnimationClip("FX/GeneratedResetFX.anim", ref fxWizard.resetClip);
        }
    }
}