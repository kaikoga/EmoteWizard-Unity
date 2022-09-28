using System.Linq;
using Silksprite.EmoteWizardSupport.UI;
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

        [MenuItem("Edit/Emote Wizard Debug/Drawers", false, 60000)]
        public static void CheckDrawers()
        {
            foreach (var kv in TypedDrawerRegistry.UntypedDrawers.Where(kv => !(kv.Value.Typed is IInvalidTypedDrawer)))
            {
                Debug.Log($"{kv.Key} => {kv.Value.Typed.GetType()}");
            }
        }

        [MenuItem("Edit/Emote Wizard Debug/All Drawers", false, 60000)]
        public static void CheckAllDrawers()
        {
            foreach (var kv in TypedDrawerRegistry.UntypedDrawers)
            {
                Debug.Log($"{kv.Key} => {kv.Value.Typed.GetType()}");
            }
        }
    }
}