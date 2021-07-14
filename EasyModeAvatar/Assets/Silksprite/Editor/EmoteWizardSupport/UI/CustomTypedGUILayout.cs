using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Silksprite.EmoteWizardSupport.UI
{
    public static class CustomTypedGUILayout
    {
        public static void AssetFieldWithGenerate<T>(string label, ref T value, Func<T> generate)
        where T : Object
        {
            using (new GUILayout.HorizontalScope())
            {
                TypedGUILayout.AssetField(label, ref value);
                if (value == null && GUILayout.Button("Generate", GUILayout.Width(60f)))
                {
                    value = generate();
                }
            }
        }
    }
}