using System;
using Silksprite.EmoteWizardSupport.L10n;
using Silksprite.EmoteWizardSupport.UI;
using UnityEditor;
using UnityEngine;

#if EW_ABLET_SUPPORT
using Ablet.Previewing;
#endif

namespace Silksprite.EmoteWizard.Preview
{
    public static class AnimationPreviewWrapper
    {
        public static readonly IAnimationPreviewWrapper Null = new NullApplicationPreviewWrapper();

        public static IAnimationPreviewWrapper Create(GameObject avatarRootObject)
        {
#if EW_ABLET_SUPPORT
            return new AbletAnimationPreviewWrapper(avatarRootObject);
#else
            return Null;
#endif
        }
    }

#if EW_ABLET_SUPPORT
    class AbletAnimationPreviewWrapper : IDisposable, IAnimationPreviewWrapper
    {

        readonly InplacePreviewRequest _previewRequest;

        public AbletAnimationPreviewWrapper(GameObject avatarRootObject)
        {
            _previewRequest = new InplacePreviewRequest(avatarRootObject);
        }

        public bool IsBlocked => _previewRequest.IsBlocked;

        public void RefreshPreview(AnimationClip clip)
        {
            _previewRequest.Posing = new InplaceAnimationPreview(clip);
        }

        public void OnInspectorGUI()
        {
            if (!_previewRequest.IsBlocked)
            {
                EmoteWizardGUILayout.HelpBox(LocalizationTool.Loc("AnimationPreview::Active."), MessageType.Info);
            }
            else
            {
                EmoteWizardGUILayout.HelpBox(LocalizationTool.Loc("AnimationPreview::Blocked."), MessageType.Warning);
            }
        }

        public void Dispose()
        {
            _previewRequest?.Dispose();
        }
    }
#endif

    class NullApplicationPreviewWrapper : IAnimationPreviewWrapper
    {
        public bool IsBlocked => true;
        public void RefreshPreview(AnimationClip clip) { }
        public void OnInspectorGUI() { }
        public void Dispose() { }
    }

}
