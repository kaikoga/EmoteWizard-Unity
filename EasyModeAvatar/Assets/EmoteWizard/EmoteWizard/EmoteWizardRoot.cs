using System.IO;
using UnityEngine;
using VRC.SDK3.Avatars.Components;

namespace EmoteWizard
{
    [DisallowMultipleComponent]
    public class EmoteWizardRoot : MonoBehaviour
    {
        [SerializeField] public VRCAvatarDescriptor avatarDescriptor;
        [SerializeField] public Animator proxyAnimator;

        [SerializeField] [HideInInspector] public string generatedAssetRoot;
        
        [SerializeField] public AnimationClip emptyClip;

        public string GeneratedAssetPath(string relativePath) => Path.Combine(generatedAssetRoot, relativePath);
    }
}