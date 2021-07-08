using Silksprite.EmoteWizardSupport.Base;
using Object = UnityEngine.Object;

namespace Silksprite.EmoteWizard.Base.DrawerContexts
{
    public abstract class EmoteWizardDrawerContextBase<T> : DrawerContextBase<T>
        where T : DrawerContextBase<T>, new()
    {
        readonly EmoteWizardRoot _emoteWizardRoot;

        protected EmoteWizardDrawerContextBase(EmoteWizardRoot emoteWizardRoot)
        {
            _emoteWizardRoot = emoteWizardRoot;
        }

        public EmoteWizardRoot EmoteWizardRoot => _emoteWizardRoot ? _emoteWizardRoot : Object.FindObjectOfType<EmoteWizardRoot>();
    }
}