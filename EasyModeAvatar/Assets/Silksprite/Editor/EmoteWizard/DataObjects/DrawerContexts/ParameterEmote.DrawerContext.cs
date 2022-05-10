using System;
using JetBrains.Annotations;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.Base.DrawerContexts;
using Silksprite.EmoteWizard.DataObjects.DrawerStates;

namespace Silksprite.EmoteWizard.DataObjects.DrawerContexts
{
    [UsedImplicitly]
    public class ParameterEmoteDrawerContext : EmoteWizardDrawerContextBase<ParameterEmote, ParameterEmoteDrawerContext>
    {
        [Obsolete]
        public readonly AnimationWizardBase AnimationWizardBase;
        public readonly ParametersWizard ParametersWizard;
        public readonly string Layer;
        public readonly ParameterEmoteDrawerState State;

        public ParameterEmoteDrawerContext() : base(null) { }
        public ParameterEmoteDrawerContext(EmoteWizardRoot emoteWizardRoot, AnimationWizardBase animationWizardBase, ParametersWizard parametersWizard, string layer, ParameterEmoteDrawerState state) : base(emoteWizardRoot)
        {
            ParametersWizard = parametersWizard;
            AnimationWizardBase = animationWizardBase;
            Layer = layer;
            State = state;
        }

        public ParameterEmoteStateDrawerContext ParameterEmoteStateDrawerContext(string name, bool canEditTargets)
        {
            return new ParameterEmoteStateDrawerContext(EmoteWizardRoot, ParametersWizard, Layer, name, State.EditTargets && canEditTargets, State.EditControls);
        }
    }
}