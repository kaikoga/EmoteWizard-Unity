using Silksprite.EmoteWizard.Base;

namespace Silksprite.EmoteWizard.Contexts
{
    public class FxLayerContext : AnimatorLayerContextBase, IFxLayerWizardContext
    {
        public FxLayerContext(AnimatorLayerWizardBase wizard) : base(wizard) { }
    }

    public interface IFxLayerWizardContext : IAnimatorLayerWizardContext { }
}