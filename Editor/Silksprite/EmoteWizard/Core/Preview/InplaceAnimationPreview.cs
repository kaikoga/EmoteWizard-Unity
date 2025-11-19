using System;
using System.Reflection;
using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.Contexts.Extensions;
using Silksprite.EmoteWizard.Preview.Core;
using Silksprite.EmoteWizard.Preview.Core.Internal.Presentation;
using UnityEngine;

#if EW_MODULAR_AVATAR
using nadena.dev.modular_avatar.core;
#endif

namespace Silksprite.EmoteWizard.Preview
{
    public class InplaceAnimationPreview : IInplacePreviewPosing
    {
        readonly AnimationClip _clip;

        public InplaceAnimationPreview(AnimationClip clip)
        {
            _clip = clip;
        }

        void IInplacePreviewPosing.Apply(IInplacePreviewPresenter presenter)
        {
            if (_clip.isHumanMotion)
            {
#if EW_MODULAR_AVATAR
                var setLockModeMethod = typeof(ModularAvatarMergeArmature).GetMethod("SetLockMode", BindingFlags.NonPublic | BindingFlags.Instance);
                foreach (var ma in presenter.Avatar.GetComponentsInChildren<ModularAvatarMergeArmature>())
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
                EmoteWizardEnvironment.FromAvatar(presenter.Avatar.transform).ProvideProxyAnimator().avatar = null;
            }
            _clip.SampleAnimation(presenter.Avatar, 0f);
        }
    }
}
