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
            if (this.GetComponentInParent<EmoteWizardRoot>(true) is EmoteWizardRoot root)
            {
                return EmoteWizardEnvironment.FromRoot(root);
            }
            if (this.GetComponentInParent<VRCAvatarDescriptor>(true) is VRCAvatarDescriptor avatarDescriptor)
            {
                return EmoteWizardEnvironment.FromAvatar(avatarDescriptor);
            }
            return null;
        }
    }

#if !UNITY_2020_3_OR_NEWER
    internal static class ComponentExtensions
    {
        public static T GetComponentInParent<T>(this Component self, bool includeInactive)
            where T : Component
        {
            return self.GetComponentsInParent<T>(includeInactive).FirstOrDefault();
        }
    }
#endif

}