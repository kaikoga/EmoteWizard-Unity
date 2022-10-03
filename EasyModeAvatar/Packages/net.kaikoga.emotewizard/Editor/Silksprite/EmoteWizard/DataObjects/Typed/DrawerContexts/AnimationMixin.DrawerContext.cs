using JetBrains.Annotations;
using Silksprite.EmoteWizard.Base.DrawerContexts;
using Silksprite.EmoteWizard.DataObjects.Typed.DrawerStates;

namespace Silksprite.EmoteWizard.DataObjects.Typed.DrawerContexts
{
    [UsedImplicitly]
    public class AnimationMixinDrawerContext : EmoteWizardDrawerContextBase<AnimationMixin, AnimationMixinDrawerContext>
    {
        public readonly string RelativePath;
        public readonly ParametersWizard ParametersWizard;
        public readonly AnimationMixinDrawerState State;

        public AnimationMixinDrawerContext() : base(null) { }
        public AnimationMixinDrawerContext(EmoteWizardRoot emoteWizardRoot, ParametersWizard parametersWizard, string relativePath, AnimationMixinDrawerState state) : base(emoteWizardRoot)
        {
            ParametersWizard = parametersWizard;
            RelativePath = relativePath;
            State = state;
        }

        public EmoteConditionDrawerContext EmoteConditionDrawerContext()
        {
            return new EmoteConditionDrawerContext(EmoteWizardRoot, ParametersWizard);
        }
        
        public EmoteControlDrawerContext EmoteControlDrawerContext()
        {
            return new EmoteControlDrawerContext(EmoteWizardRoot, ParametersWizard, false, State.EditControls);
        }
    }
}