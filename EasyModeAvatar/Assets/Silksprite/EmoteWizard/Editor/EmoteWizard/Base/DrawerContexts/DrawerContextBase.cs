using System;
using Object = UnityEngine.Object;

namespace Silksprite.EmoteWizard.Base.DrawerContexts
{
    public abstract class DrawerContextBase<T> : IDisposable where T : DrawerContextBase<T>, new()
    {
        readonly EmoteWizardRoot _emoteWizardRoot;

        protected DrawerContextBase(EmoteWizardRoot emoteWizardRoot)
        {
            _emoteWizardRoot = emoteWizardRoot;
        }

        public EmoteWizardRoot EmoteWizardRoot => _emoteWizardRoot ? _emoteWizardRoot : Object.FindObjectOfType<EmoteWizardRoot>();

        public void Dispose() => PropertyDrawerWithContext<T>.EndContext();
    }
}