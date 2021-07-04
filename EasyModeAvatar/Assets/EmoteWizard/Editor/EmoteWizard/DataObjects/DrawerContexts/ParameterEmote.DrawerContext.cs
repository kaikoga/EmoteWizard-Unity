using EmoteWizard.Base.DrawerContexts;

namespace EmoteWizard.DataObjects.DrawerContexts
{
    public class ParameterEmoteDrawerContext : DrawerContextBase<ParameterEmoteDrawerContext>
    {
        public readonly string Layer;
        public readonly bool EditTargets;

        public ParameterEmoteDrawerContext() : base(null) { }
        public ParameterEmoteDrawerContext(EmoteWizardRoot emoteWizardRoot, string layer, bool editTargets) : base(emoteWizardRoot)
        {
            Layer = layer;
            EditTargets = editTargets;
        }
    }
}