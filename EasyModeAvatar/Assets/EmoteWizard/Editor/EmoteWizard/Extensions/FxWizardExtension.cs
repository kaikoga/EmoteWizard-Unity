using UnityEngine;

namespace EmoteWizard.Extensions
{
    public static class FxWizardExtension
    {
        public static AnimationClip ProvideResetClip(this FxWizard fxWizard)
        {
            return fxWizard.EmoteWizardRoot.EnsureAnimationClip("FX/@@@Generated@@@ResetFX.anim", ref fxWizard.resetClip);
        }
    }
}