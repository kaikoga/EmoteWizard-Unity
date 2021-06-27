using System.IO;
using UnityEngine;

namespace EmoteWizard
{
    [DisallowMultipleComponent]
    public class EmoteWizardRoot : MonoBehaviour
    {
        [SerializeField] [HideInInspector] public string generatedAssetRoot = "Assets/Generated/";
        [SerializeField] [HideInInspector] public string generatedAssetPrefix = "Generated";
        
        [SerializeField] public AnimationClip emptyClip;
        [SerializeField] public bool useReorderUI;

        public string GeneratedAssetPath(string relativePath) => Path.Combine(generatedAssetRoot, relativePath.Replace("@@@Generated@@@", generatedAssetPrefix));
    }
}