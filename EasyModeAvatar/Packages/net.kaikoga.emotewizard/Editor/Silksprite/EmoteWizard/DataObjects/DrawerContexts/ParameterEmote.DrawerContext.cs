using JetBrains.Annotations;
using Silksprite.EmoteWizard.Base.DrawerContexts;
using Silksprite.EmoteWizard.DataObjects.DrawerStates;
using UnityEngine;

namespace Silksprite.EmoteWizard.DataObjects.DrawerContexts
{
    [UsedImplicitly]
    public class ParameterEmoteDrawerContext : EmoteWizardDrawerContextBase<ParameterEmote, ParameterEmoteDrawerContext>
    {
        public readonly Component Component;
        public readonly ParametersWizard ParametersWizard;
        public readonly string Layer;
        public readonly ParameterEmoteDrawerState State;

        public ParameterEmoteDrawerContext() : base(null) { }
        public ParameterEmoteDrawerContext(EmoteWizardRoot emoteWizardRoot, Component component, ParametersWizard parametersWizard, string layer, ParameterEmoteDrawerState state) : base(emoteWizardRoot)
        {
            ParametersWizard = parametersWizard;
            Component = component;
            Layer = layer;
            State = state;
        }

        public ParameterEmoteStateDrawerContext ParameterEmoteStateDrawerContext(string name, bool canEditTargets)
        {
            return new ParameterEmoteStateDrawerContext(EmoteWizardRoot, ParametersWizard, Layer, name, State.EditTargets && canEditTargets, State.EditControls);
        }
    }
}