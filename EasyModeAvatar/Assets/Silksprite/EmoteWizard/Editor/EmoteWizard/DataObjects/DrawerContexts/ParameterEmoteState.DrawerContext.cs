using Silksprite.EmoteWizard.Base.DrawerContexts;

namespace Silksprite.EmoteWizard.DataObjects.DrawerContexts
{
    public class ParameterEmoteStateDrawerContext : DrawerContextBase<ParameterEmoteStateDrawerContext>
    {
        public readonly string Layer;
        public readonly string Name;
        public readonly bool EditTargets;

        public ParameterEmoteStateDrawerContext() : base(null) { }
        public ParameterEmoteStateDrawerContext(EmoteWizardRoot emoteWizardRoot, string layer, string name, bool editTargets) : base(emoteWizardRoot)
        {
            Layer = layer;
            Name = name;
            EditTargets = editTargets;
        }
    }
}