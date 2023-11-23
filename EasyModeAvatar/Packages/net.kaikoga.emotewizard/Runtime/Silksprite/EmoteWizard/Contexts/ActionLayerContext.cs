using Silksprite.EmoteWizard.Base;

namespace Silksprite.EmoteWizard.Contexts
{
    public class ActionLayerContext : AnimatorLayerContextBase, IActionLayerWizardContext
    {
        public ActionLayerContext(AnimatorLayerWizardBase wizard) : base(wizard) { }
    }

    public interface IActionLayerWizardContext : IAnimatorLayerWizardContext { }
}