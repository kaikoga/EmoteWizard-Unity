using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizard.Utils
{
    public static class VrcSdkAssetLocator
    {
        static T LoadAssetByGuidOrPath<T>(string guid, string path)
        where T : Object
        {
            var realPath = AssetDatabase.GUIDToAssetPath(guid);
            if (string.IsNullOrEmpty(realPath)) realPath = path;
            return AssetDatabase.LoadAssetAtPath<T>(realPath);
        }

        public static Texture2D PersonDance()
        {
            return LoadAssetByGuidOrPath<Texture2D>("9a20b3a6641e1af4e95e058f361790cb", 
                "Assets/VRCSDK/Examples3/Expressions Menu/Icons/person_dance.png");
        }

        public static Texture2D ItemFolder()
        {
            return LoadAssetByGuidOrPath<Texture2D>("a06282136d558c54aa15d533f163ff59", 
                "Assets/VRCSDK/Examples3/Expressions Menu/Icons/item_folder.png");
        }

        public static AvatarMask HandsOnly()
        {
            return LoadAssetByGuidOrPath<AvatarMask>("b2b8bad9583e56a46a3e21795e96ad92", 
                "Assets/VRCSDK/Examples3/Animation/Masks/vrc_HandsOnly.mask");
        }

        public static AvatarMask HandLeft()
        {
            return LoadAssetByGuidOrPath<AvatarMask>("7ff0199655202a04eb175de45a6e078a", 
                "Assets/VRCSDK/Examples3/Animation/Masks/vrc_Hand Left.mask");
        }

        public static AvatarMask HandRight()
        {
            return LoadAssetByGuidOrPath<AvatarMask>("903ce375d5f609d44b9f00b425d6eda9", 
                "Assets/VRCSDK/Examples3/Animation/Masks/vrc_Hand Right.mask");
        }

        public static RuntimeAnimatorController HandsLayerController1()
        {
            return LoadAssetByGuidOrPath<RuntimeAnimatorController>("404d228aeae421f4590305bc4cdaba16", 
                "Assets/VRCSDK/Examples3/Animation/Controllers/vrc_AvatarV3HandsLayer.controller");
        }

        public static RuntimeAnimatorController HandsLayerController2()
        {
            return LoadAssetByGuidOrPath<RuntimeAnimatorController>("5ecf8b95a27552840aef66909bdf588f", 
                "Assets/VRCSDK/Examples3/Animation/Controllers/vrc_AvatarV3HandsLayer2.controller");
        }

        public static RuntimeAnimatorController ActionLayerController()
        {
            return LoadAssetByGuidOrPath<RuntimeAnimatorController>("3e479eeb9db24704a828bffb15406520", 
                "Assets/VRCSDK/Examples3/Animation/Controllers/vrc_AvatarV3ActionLayer.controller");
        }

        public static RuntimeAnimatorController SittingLayerController1()
        {
            return LoadAssetByGuidOrPath<RuntimeAnimatorController>("1268460c14f873240981bf15aa88b21a", 
                "Assets/VRCSDK/Examples3/Animation/Controllers/vrc_AvatarV3SittingLayer.controller");
        }

        public static RuntimeAnimatorController SittingLayerController2()
        {
            return LoadAssetByGuidOrPath<RuntimeAnimatorController>("74c2e15937e5c95478edd251f20e126f", 
                "Assets/VRCSDK/Examples3/Animation/Controllers/vrc_AvatarV3SittingLayer2.controller");
        }
    }
}