using System;
using System.Reflection;
using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.Contexts.Extensions;
using UnityEditor;
using UnityEngine;
using VRC.SDK3.Avatars.Components;
using Object = UnityEngine.Object;

#if EW_MODULAR_AVATAR
using nadena.dev.modular_avatar.core;
#endif

namespace Silksprite.EmoteWizard.Utils
{
    public class AnimationPreview : IDisposable
    {
        VRCAvatarDescriptor _originalAvatar;
        GameObject _avatar;
        EmoteWizardEnvironment _environment;

        public void Refresh(VRCAvatarDescriptor originalAvatar, AnimationClip clip, float time)
        {
            Dispose();
            _originalAvatar = originalAvatar;
            _avatar = Object.Instantiate(originalAvatar.gameObject);
            SceneVisibilityManager.instance.Isolate(_avatar.gameObject, true);
            _avatar.hideFlags = HideFlags.HideAndDontSave;
            if (clip.isHumanMotion)
            {
#if EW_MODULAR_AVATAR
                var setLockModeMethod = typeof(ModularAvatarMergeArmature).GetMethod("SetLockMode", BindingFlags.NonPublic | BindingFlags.Instance);
                foreach (var ma in _avatar.GetComponentsInChildren<ModularAvatarMergeArmature>())
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
                _environment = EmoteWizardEnvironment.FromAvatar(_avatar.GetComponent<VRCAvatarDescriptor>());
                _environment.ProvideProxyAnimator().avatar = null;
            }
            clip.SampleAnimation(_avatar, time);
        }

        public void Dispose()
        {
            SceneVisibilityManager.instance.ExitIsolation();
            if (_avatar) Object.DestroyImmediate(_avatar);
        }
    }
}
