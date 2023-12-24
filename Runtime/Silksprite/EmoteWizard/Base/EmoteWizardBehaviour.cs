using System.Linq;
using Silksprite.EmoteWizard.Contexts;
using UnityEngine;
using VRC.SDK3.Avatars.Components;
using VRC.SDKBase;

namespace Silksprite.EmoteWizard.Base
{
    public abstract class EmoteWizardBehaviour : MonoBehaviour, IEditorOnly
    {
        public EmoteWizardEnvironment CreateEnv()
        {
            return GetComponentsInParent<EmoteWizardRoot>(true).Select(EmoteWizardEnvironment.FromRoot)
                .Concat(GetComponentsInParent<VRCAvatarDescriptor>(true).Select(EmoteWizardEnvironment.FromAvatar))
                .FirstOrDefault();
        }
    }
}