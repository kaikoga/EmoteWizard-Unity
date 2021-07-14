using System;

namespace Silksprite.EmoteWizardSupport.Base
{
    public abstract class DrawerContextBase<T, TContext> : IDisposable
        where TContext : DrawerContextBase<T, TContext>
    {
        public void Dispose() => PropertyDrawerWithContext<T, TContext>.EndContext();
    }
}