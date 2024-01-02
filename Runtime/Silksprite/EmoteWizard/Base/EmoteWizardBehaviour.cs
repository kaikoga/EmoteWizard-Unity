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
            if (GetComponentInParent<EmoteWizardRoot>(true) is EmoteWizardRoot root)
            {
                return EmoteWizardEnvironment.FromRoot(root);
            }
            if (GetComponentInParent<VRCAvatarDescriptor>(true) is VRCAvatarDescriptor avatarDescriptor)
            {
                return EmoteWizardEnvironment.FromAvatar(avatarDescriptor);
            }
            return null;
        }
    }
}