using Silksprite.EmoteWizard.Base;

namespace Silksprite.EmoteWizard.Contexts
{
    public class GestureLayerContext : AnimatorLayerContextBase, IGestureLayerWizardContext
    {
        public GestureLayerContext(AnimatorLayerWizardBase wizard) : base(wizard) { }
    }

    public interface IGestureLayerWizardContext : IAnimatorLayerWizardContext { }
}