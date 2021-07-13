using System;

namespace Silksprite.EmoteWizardSupport.Base
{
    public abstract class DrawerContextBase<T> : IDisposable
        where T : DrawerContextBase<T>
    {
        public void Dispose() => PropertyDrawerWithContext<T>.EndContext();
    }
}