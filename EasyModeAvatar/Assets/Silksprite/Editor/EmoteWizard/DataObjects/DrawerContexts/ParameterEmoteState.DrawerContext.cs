using JetBrains.Annotations;
using Silksprite.EmoteWizard.Base.DrawerContexts;

namespace Silksprite.EmoteWizard.DataObjects.DrawerContexts
{
    [UsedImplicitly]
    public class ParameterEmoteStateDrawerContext : EmoteWizardDrawerContextBase<ParameterEmoteState, ParameterEmoteStateDrawerContext>
    {
        public readonly ParametersWizard ParametersWizard;
        public readonly string Layer;
        public readonly string Name;
        public readonly bool EditTargets;
        public readonly bool EditParameters;

        public ParameterEmoteStateDrawerContext() : base(null) { }
        public ParameterEmoteStateDrawerContext(EmoteWizardRoot emoteWizardRoot, ParametersWizard parametersWizard, string layer, string name, bool editTargets, bool editParameters) : base(emoteWizardRoot)
        {
            ParametersWizard = parametersWizard;
            Layer = layer;
            Name = name;
            EditTargets = editTargets;
            EditParameters = editParameters;
        }

        public EmoteParameterDrawerContext EmoteParameterDrawerContext()
        {
            return new EmoteParameterDrawerContext(EmoteWizardRoot, ParametersWizard, EditParameters);
        }
    }
}