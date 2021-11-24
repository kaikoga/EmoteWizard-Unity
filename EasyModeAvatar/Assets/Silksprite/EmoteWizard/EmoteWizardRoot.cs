using System.IO;
using Silksprite.EmoteWizard.Base;
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
        [SerializeField] public bool showCopyPasteJsonButtons;
        [SerializeField] public bool lowSpecUI = true;

        public T GetWizard<T>() where T : EmoteWizardBase => GetComponentInChildren<T>();

        public T EnsureWizard<T>() where T : EmoteWizardBase
        {
            var wizard = GetComponentInChildren<T>();
            if (!(wizard is null)) return wizard;

            if (!lowSpecUI) return gameObject.AddComponent<T>();

            var childObject = new GameObject(typeof(T).Name);
            childObject.transform.parent = gameObject.transform;
            return childObject.AddComponent<T>();
        }

        public string GeneratedAssetPath(string relativePath) => Path.Combine(generatedAssetRoot, relativePath.Replace("@@@Generated@@@", generatedAssetPrefix));
    }
}