using JetBrains.Annotations;
using Silksprite.EmoteWizard.Base.DrawerContexts;

namespace Silksprite.EmoteWizard.DataObjects.Typed.DrawerContexts
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

        public EmoteControlDrawerContext EmoteControlDrawerContext()
        {
            return new EmoteControlDrawerContext(EmoteWizardRoot, ParametersWizard, false, EditParameters);
        }
    }
}