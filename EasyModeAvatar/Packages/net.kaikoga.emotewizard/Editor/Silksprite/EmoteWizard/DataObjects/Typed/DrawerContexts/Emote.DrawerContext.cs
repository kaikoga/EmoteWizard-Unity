using JetBrains.Annotations;
using Silksprite.EmoteWizard.Base.DrawerContexts;
using Silksprite.EmoteWizard.DataObjects.Typed.DrawerStates;

namespace Silksprite.EmoteWizard.DataObjects.Typed.DrawerContexts
{
    [UsedImplicitly]
    public class EmoteDrawerContext : EmoteWizardDrawerContextBase<Emote, EmoteDrawerContext>
    {
        public readonly ParametersWizard ParametersWizard;
        public readonly string Layer;
        public readonly bool AdvancedAnimations;
        public readonly EmoteDrawerState State;

        public EmoteDrawerContext() : base(null) { }
        public EmoteDrawerContext(EmoteWizardRoot emoteWizardRoot, ParametersWizard parametersWizard, string layer, bool advancedAnimations, EmoteDrawerState state) : base(emoteWizardRoot)
        {
            ParametersWizard = parametersWizard;
            AdvancedAnimations = advancedAnimations;
            Layer = layer;
            State = state;
        }

        public EmoteConditionDrawerContext EmoteConditionDrawerContext()
        {
            return new EmoteConditionDrawerContext(EmoteWizardRoot, ParametersWizard);
        }
        
        public EmoteControlDrawerContext EmoteParameterDrawerContext()
        {
            return new EmoteControlDrawerContext(EmoteWizardRoot, ParametersWizard, true, State.EditControls);
        }
    }
}