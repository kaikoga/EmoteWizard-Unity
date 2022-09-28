using Silksprite.EmoteWizardSupport.Base;

namespace Silksprite.EmoteWizardSupport.Collections.Base
{
    public abstract class ListHeaderDrawerWithContext<T, TContext> : ListHeaderDrawerBase
        where TContext : DrawerContextBase<T, TContext>
    {
        protected TContext EnsureContext() => DrawerContextBase<T, TContext>.EnsureContext();
    }
}