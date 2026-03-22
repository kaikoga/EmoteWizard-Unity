using System;
using UnityEngine;

namespace Silksprite.EmoteWizard.Preview
{
    public interface IAnimationPreviewWrapper : IDisposable
    {
        bool IsBlocked { get; }
        void RefreshPreview(AnimationClip? clip);
        void OnInspectorGUI();
    }
}
