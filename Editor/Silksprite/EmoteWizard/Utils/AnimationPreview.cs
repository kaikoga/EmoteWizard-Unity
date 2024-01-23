using System;
using System.Collections.Generic;
using System.Reflection;
using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.Contexts.Extensions;
using Silksprite.EmoteWizardSupport.UI;
using UnityEditor;
using UnityEngine;
using VRC.SDK3.Avatars.Components;
using static Silksprite.EmoteWizardSupport.L10n.LocalizationTool;
using Object = UnityEngine.Object;

#if EW_MODULAR_AVATAR
using nadena.dev.modular_avatar.core;
#endif

namespace Silksprite.EmoteWizard.Utils
{
    public class AnimationPreview : IDisposable
    {
        readonly VRCAvatarDescriptor _originalAvatar;
        GameObject _avatar;
        EmoteWizardEnvironment _environment;

        public AnimationClip Clip;

        static HashSet<AnimationPreview> _activePreviews = new HashSet<AnimationPreview>();
        static AnimationPreview _currentPreview;

        bool _disposed = false;

        public AnimationPreview(VRCAvatarDescriptor originalAvatar)
        {
            _originalAvatar = originalAvatar;
            
            ObjectChangeEvents.changesPublished += OnChangesPublished;
            AssemblyReloadEvents.beforeAssemblyReload += Dispose;
            _activePreviews.Add(this);
        }

        public void OnInspectorGUI()
        {
            if (_currentPreview != null)
            {
                if (_currentPreview == this)
                {
                    EmoteWizardGUILayout.HelpBox(Loc("AnimationPreview::Active."), MessageType.Info);
                }
                else
                {
                    EmoteWizardGUILayout.HelpBox(Loc("AnimationPreview::Blocked."), MessageType.Warning);
                }
            }
        }

        public void Refresh()
        {
            if (_currentPreview != null && _currentPreview != this)
            {
                return;
            }
            Hide();
            if (!Clip || !_originalAvatar)
            {
                return;
            }
            Show();
        }

        void OnChangesPublished(ref ObjectChangeEventStream stream)
        {
            if (stream.length > 0) Refresh();
        }

        void Show()
        {
            _currentPreview = this;

            _avatar = Object.Instantiate(_originalAvatar.gameObject);
            SceneVisibilityManager.instance.Isolate(_avatar.gameObject, true);
            _avatar.hideFlags = HideFlags.HideAndDontSave;
            if (Clip.isHumanMotion)
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
            Clip.SampleAnimation(_avatar, 0f);
        }

        void Hide()
        {
            _currentPreview = null;

            SceneVisibilityManager.instance.ExitIsolation();
            if (_avatar) Object.DestroyImmediate(_avatar);
        }

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;
            if (_currentPreview == this) Hide();
            ObjectChangeEvents.changesPublished -= OnChangesPublished;
            AssemblyReloadEvents.beforeAssemblyReload -= Dispose;
            _activePreviews.Remove(this);
        }
    }
}
