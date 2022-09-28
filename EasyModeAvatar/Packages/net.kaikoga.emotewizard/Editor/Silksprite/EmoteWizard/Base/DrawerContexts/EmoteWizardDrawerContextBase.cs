using Silksprite.EmoteWizardSupport.Base;
using Object = UnityEngine.Object;

namespace Silksprite.EmoteWizard.Base.DrawerContexts
{
    public abstract class EmoteWizardDrawerContextBase<T, TContext> : DrawerContextBase<T, TContext>
        where TContext : DrawerContextBase<T, TContext>
    {
        readonly EmoteWizardRoot _emoteWizardRoot;

        protected EmoteWizardDrawerContextBase(EmoteWizardRoot emoteWizardRoot)
        {
            _emoteWizardRoot = emoteWizardRoot;
        }

        public EmoteWizardRoot EmoteWizardRoot => _emoteWizardRoot ? _emoteWizardRoot : Object.FindObjectOfType<EmoteWizardRoot>();
    }
}