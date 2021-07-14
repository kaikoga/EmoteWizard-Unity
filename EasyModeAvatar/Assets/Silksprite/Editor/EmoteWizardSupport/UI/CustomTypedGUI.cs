using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Silksprite.EmoteWizardSupport.UI
{
    public static class CustomTypedGUI
    {
        public static void AssetFieldWithGenerate<T>(Rect position, string label, ref T value, Func<T> generate)
        where T : Object
        {
            const float buttonWidth = 60;
            var fieldPosition = position;
            if (value == null) fieldPosition.width -= buttonWidth;

            TypedGUI.AssetField(fieldPosition, label, ref value);
            if (value != null) return;
            var buttonPosition = new Rect(position.xMax - buttonWidth, position.y, buttonWidth, position.height);
            if (GUI.Button(buttonPosition, "Generate"))
            {
                value = generate();
            }
        }
    }
}