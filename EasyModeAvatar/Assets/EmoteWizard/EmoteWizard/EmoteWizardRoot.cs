using System.IO;
using UnityEngine;
using VRC.SDKBase;

namespace EmoteWizard
{
    [DisallowMultipleComponent]
    public class EmoteWizardRoot : MonoBehaviour
    {
        [SerializeField] VRC_AvatarDescriptor avatarDescriptor;

        [SerializeField] [HideInInspector] public string generatedAssetRoot;
        
        [SerializeField] public AnimationClip emptyClip;
        
        public string GeneratedAssetPath(string relativePath) => Path.Combine(generatedAssetRoot, relativePath);
    }
}