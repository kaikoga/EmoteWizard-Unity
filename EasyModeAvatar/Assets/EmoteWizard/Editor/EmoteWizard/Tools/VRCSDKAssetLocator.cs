using UnityEditor;
using UnityEngine;

namespace EmoteWizard.Tools
{
    public static class VrcSdkAssetLocator
    {
        public static Texture2D PersonDance()
        {
            // const string path = "Assets/VRCSDK/Examples3/Expressions Menu/Icons/person_dance.png";
            var path = AssetDatabase.GUIDToAssetPath("9a20b3a6641e1af4e95e058f361790cb");
            return AssetDatabase.LoadAssetAtPath<Texture2D>(path);
        }

        public static Texture2D ItemFolder()
        {
            // const string path = "Assets/VRCSDK/Examples3/Expressions Menu/Icons/item_folder.png";
            var path = AssetDatabase.GUIDToAssetPath("a06282136d558c54aa15d533f163ff59");
            return AssetDatabase.LoadAssetAtPath<Texture2D>(path);
        }
    }
}