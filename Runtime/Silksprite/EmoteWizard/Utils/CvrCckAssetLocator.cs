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

        public static Motion HandLeftOpen()
        {
            return AvatarAsset<Motion>("Animations/Hands/HandLeftOpen.anim");
        }

        public static Motion HandLeftRelaxed()
        {
            return AvatarAsset<Motion>("Animations/Hands/HandLeftRelaxed.anim");
        }

        public static Motion HandLeftFist()
        {
            return AvatarAsset<Motion>("Animations/Hands/HandLeftFist.anim");
        }

        public static Motion HandLeftThumbsUp()
        {
            return AvatarAsset<Motion>("Animations/Hands/HandLeftThumbsUp.anim");
        }

        public static Motion HandLeftGun()
        {
            return AvatarAsset<Motion>("Animations/Hands/HandLeftGun.anim");
        }

        public static Motion HandLeftPoint()
        {
            return AvatarAsset<Motion>("Animations/Hands/HandLeftPoint.anim");
        }

        public static Motion HandLeftPeace()
        {
            return AvatarAsset<Motion>("Animations/Hands/HandLeftPeace.anim");
        }

        public static Motion HandLeftRocknroll()
        {
            return AvatarAsset<Motion>("Animations/Hands/HandLeftRocknroll.anim");
        }

        public static Motion HandRightOpen()
        {
            return AvatarAsset<Motion>("Animations/Hands/HandRightOpen.anim");
        }

        public static Motion HandRightRelaxed()
        {
            return AvatarAsset<Motion>("Animations/Hands/HandRightRelaxed.anim");
        }

        public static Motion HandRightFist()
        {
            return AvatarAsset<Motion>("Animations/Hands/HandRightFist.anim");
        }

        public static Motion HandRightThumbsUp()
        {
            return AvatarAsset<Motion>("Animations/Hands/HandRightThumbsUp.anim");
        }

        public static Motion HandRightGun()
        {
            return AvatarAsset<Motion>("Animations/Hands/HandRightGun.anim");
        }

        public static Motion HandRightPoint()
        {
            return AvatarAsset<Motion>("Animations/Hands/HandRightPoint.anim");
        }

        public static Motion HandRightPeace()
        {
            return AvatarAsset<Motion>("Animations/Hands/HandRightPeace.anim");
        }

        public static Motion HandRightRocknroll()
        {
            return AvatarAsset<Motion>("Animations/Hands/HandRightRocknroll.anim");
        }

        public static Motion EmotesEmote1()
        {
            return AvatarAsset<Motion>("Animations/Emotes/Emote1.anim");
        }

        public static Motion EmotesEmote2()
        {
            return AvatarAsset<Motion>("Animations/Emotes/Emote2.anim");
        }

        public static Motion EmotesEmote3()
        {
            return AvatarAsset<Motion>("Animations/Emotes/Emote3.anim");
        }

        public static Motion EmotesEmote4()
        {
            return AvatarAsset<Motion>("Animations/Emotes/Emote4.anim");
        }

        public static Motion EmotesEmote5()
        {
            return AvatarAsset<Motion>("Animations/Emotes/Emote5.anim");
        }

        public static Motion EmotesEmote6()
        {
            return AvatarAsset<Motion>("Animations/Emotes/Emote6.anim");
        }

        public static Motion EmotesEmote7()
        {
            return AvatarAsset<Motion>("Animations/Emotes/Emote7.anim");
        }

        public static Motion EmotesEmote8()
        {
            return AvatarAsset<Motion>("Animations/Emotes/Emote8.anim");
        }

        public static RuntimeAnimatorController AvatarAnimator()
        {
            return AvatarAsset<RuntimeAnimatorController>("Animations/AvatarAnimator.controller");
        }
    }
}
