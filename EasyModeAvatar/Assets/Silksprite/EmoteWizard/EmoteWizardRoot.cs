using System.IO;
using Silksprite.EmoteWizardSupport.UI;
using UnityEngine;

namespace Silksprite.EmoteWizard
{
    [DisallowMultipleComponent]
    public class EmoteWizardRoot : MonoBehaviour
    {
        [SerializeField] [HideInInspector] public string generatedAssetRoot = "Assets/Generated/";
        [SerializeField] [HideInInspector] public string generatedAssetPrefix = "Generated";
        
        [SerializeField] public AnimationClip emptyClip;
        [SerializeField] public ListDisplayMode listDisplayMode;
        [SerializeField] public bool showTutorial;

        public string GeneratedAssetPath(string relativePath) => Path.Combine(generatedAssetRoot, relativePath.Replace("@@@Generated@@@", generatedAssetPrefix));
    }
}