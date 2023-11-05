using System.Linq;
using UnityEngine;

namespace Silksprite.EmoteWizard.Base
{
    public abstract class EmoteWizardBehaviour : MonoBehaviour
    {
        public IEmoteWizardContext Context => GetComponentsInParent<EmoteWizardRoot>(true).FirstOrDefault();
        protected T GetWizard<T>() where T : EmoteWizardBase => Context.GetWizard<T>();

        public bool IsSetupMode
        {
            get
            {
                var setupWizard = GetWizard<SetupWizard>();
                return setupWizard != null && setupWizard.isSetupMode;
            }
        }
    }
}