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

        public static AvatarMask HandsOnly()
        {
            // const string path = "Assets/VRCSDK/Examples3/Animation/Masks/vrc_HandsOnly.mask";
            var path = AssetDatabase.GUIDToAssetPath("b2b8bad9583e56a46a3e21795e96ad92");
            return AssetDatabase.LoadAssetAtPath<AvatarMask>(path);
        }

        public static AvatarMask HandLeft()
        {
            // const string path = "Assets/VRCSDK/Examples3/Animation/Masks/vrc_Hand Left.mask";
            var path = AssetDatabase.GUIDToAssetPath("7ff0199655202a04eb175de45a6e078a");
            return AssetDatabase.LoadAssetAtPath<AvatarMask>(path);
        }

        public static AvatarMask HandRight()
        {
            // const string path = "Assets/VRCSDK/Examples3/Animation/Masks/vrc_Hand Right.mask";
            var path = AssetDatabase.GUIDToAssetPath("903ce375d5f609d44b9f00b425d6eda9");
            return AssetDatabase.LoadAssetAtPath<AvatarMask>(path);
        }
    }
}