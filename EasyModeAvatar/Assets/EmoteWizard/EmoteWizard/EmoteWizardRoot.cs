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

        [SerializeField] [HideInInspector] public string generatedAssetRoot = "Assets/Generated/";
        [SerializeField] [HideInInspector] public string generatedAssetPrefix = "Generated";
        
        [SerializeField] public AnimationClip emptyClip;
        [SerializeField] public bool useReorderUI;

        public string GeneratedAssetPath(string relativePath) => Path.Combine(generatedAssetRoot, relativePath.Replace("@Generated", generatedAssetPrefix));
    }
}