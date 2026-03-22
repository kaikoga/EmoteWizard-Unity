#if EW_ABLET_SUPPORT

using Ablet.API.V1;
using Ablet.API.V1.Building;
using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.Contexts.Extensions;
using UnityEngine;

#if EW_MODULAR_AVATAR
using System;
using System.Reflection;
using nadena.dev.modular_avatar.core;
#endif

namespace Silksprite.EmoteWizard.Preview
{
    public class InplaceAnimationPreview : AbletObservableProcedure
    {
        readonly AnimationClip? _clip;

        public InplaceAnimationPreview(AnimationClip? clip)
        {
            _clip = clip;
        }

        public override void Observe(IObserveContext observeContext)
        {
            observeContext.RootObject.Observe(Apply);
        }

        void Apply(GameObject avatarObject)
        {
            if (_clip == null)
            {
                return;
            }
            if (_clip.isHumanMotion)
            {
#if EW_MODULAR_AVATAR
                var setLockModeMethod = typeof(ModularAvatarMergeArmature).GetMethod("SetLockMode", BindingFlags.NonPublic | BindingFlags.Instance);
                foreach (var ma in avatarObject.GetComponentsInChildren<ModularAvatarMergeArmature>())
                {
                    if (ma.LockMode == ArmatureLockMode.NotLocked)
                    {
                        ma.LockMode = ArmatureLockMode.BaseToMerge;
                    }
                    setLockModeMethod?.Invoke(ma, Array.Empty<object>());
                }
#endif
            }
            else
            {
                EmoteWizardEnvironment.FromAvatar(avatarObject.transform).ProvideProxyAnimator().avatar = null;
            }
            _clip.SampleAnimation(avatarObject, 0f);
        }
    }
}

#endif