using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizard
{
    public static class EmoteWizardMenuItems
    {
        [MenuItem("GameObject/Emote Wizard", false, 20)]
        public static void Create()
        {
            var gameObject = new GameObject("Emote Wizard", typeof(EmoteWizardRoot));
            AssetDatabase.SaveAssets();
            EditorUtility.SetDirty(gameObject);
        }
    }
}