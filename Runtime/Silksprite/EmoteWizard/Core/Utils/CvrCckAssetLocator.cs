using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Silksprite.EmoteWizard.Utils
{
    public static class CvrCckAssetLocator
    {
        const string BaseFolderPath = "Assets/CVR.CCK";

        static T AvatarAsset<T>(string path)
            where T : Object
        {
#if UNITY_EDITOR
            return AssetDatabase.LoadAssetAtPath<T>($"{BaseFolderPath}/Assets/Avatar/{path}");
#else
            return null;
#endif
        }

        public static AvatarMask GesturesLeft()
        {
            return AvatarAsset<AvatarMask>("Animations/Masks/GesturesLeft.mask");
        }

        public static AvatarMask GesturesRight()
        {
            return AvatarAsset<AvatarMask>("Animations/Masks/GesturesRight.mask");
        }

        public static RuntimeAnimatorController AvatarAnimator()
        {
            return AvatarAsset<RuntimeAnimatorController>("Animations/AvatarAnimator.controller");
        }
    }
}
