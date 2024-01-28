using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Silksprite.EmoteWizard.Utils
{
    public static class VrcSdkAssetLocator
    {
        const string BaseFolderPath = "Packages/com.vrchat.avatars/Samples";

        static T DemoAsset<T>(string path)
            where T : Object
        {
#if UNITY_EDITOR
            return AssetDatabase.LoadAssetAtPath<T>($"{BaseFolderPath}/AV3 Demo Assets/{path}");
#else
            return null;
#endif
        }

        public static Texture2D PersonDance()
        {
            return DemoAsset<Texture2D>("Expressions Menu/Icons/person_dance.png");
        }

        public static Texture2D ItemFolder()
        {
            return DemoAsset<Texture2D>("Expressions Menu/Icons/item_folder.png");
        }

        public static Texture2D ItemWand()
        {
            return DemoAsset<Texture2D>("Expressions Menu/Icons/item_wand.png");
        }

        public static AvatarMask HandsOnly()
        {
            return DemoAsset<AvatarMask>("Animation/Masks/vrc_HandsOnly.mask");
        }

        public static AvatarMask HandLeft()
        {
            return DemoAsset<AvatarMask>("Animation/Masks/vrc_Hand Left.mask");
        }

        public static AvatarMask HandRight()
        {
            return DemoAsset<AvatarMask>("Animation/Masks/vrc_Hand Right.mask");
        }

        public static RuntimeAnimatorController HandsLayerController1()
        {
            return DemoAsset<RuntimeAnimatorController>("Animation/Controllers/vrc_AvatarV3HandsLayer.controller");
        }

        public static RuntimeAnimatorController HandsLayerController2()
        {
            return DemoAsset<RuntimeAnimatorController>("Animation/Controllers/vrc_AvatarV3HandsLayer2.controller");
        }

        public static RuntimeAnimatorController ActionLayerController()
        {
            return DemoAsset<RuntimeAnimatorController>("Animation/Controllers/vrc_AvatarV3ActionLayer.controller");
        }

        public static RuntimeAnimatorController SittingLayerController1()
        {
            return DemoAsset<RuntimeAnimatorController>("Animation/Controllers/vrc_AvatarV3SittingLayer.controller");
        }

        public static RuntimeAnimatorController SittingLayerController2()
        {
            return DemoAsset<RuntimeAnimatorController>("Animation/Controllers/vrc_AvatarV3SittingLayer2.controller");
        }

        public static Motion ProxyStandStill()
        {
            return DemoAsset<Motion>("Animation/ProxyAnim/proxy_stand_still.anim");
        }

        public static Motion ProxyStandStill2()
        {
            return DemoAsset<Motion>("Animation/ProxyAnim/proxy_stand_still2.anim");
        }

        public static Motion ProxyStandStill3()
        {
            return DemoAsset<Motion>("Animation/ProxyAnim/proxy_stand_still3.anim");
        }

        public static Motion ProxySit()
        {
            return DemoAsset<Motion>("Animation/ProxyAnim/proxy_sit.anim");
        }

        public static Motion ProxySit2()
        {
            return DemoAsset<Motion>("Animation/ProxyAnim/proxy_sit2.anim");
        }

        public static Motion ProxyHandsIdle()
        {
            return DemoAsset<Motion>("Animation/ProxyAnim/proxy_hands_idle.anim");
        }

        public static Motion ProxyHandsIdle2()
        {
            return DemoAsset<Motion>("Animation/ProxyAnim/proxy_hands_idle2.anim");
        }

        public static Motion ProxyHandsFist()
        {
            return DemoAsset<Motion>("Animation/ProxyAnim/proxy_hands_fist.anim");
        }

        public static Motion ProxyHandsOpen()
        {
            return DemoAsset<Motion>("Animation/ProxyAnim/proxy_hands_open.anim");
        }

        public static Motion ProxyHandsPoint()
        {
            return DemoAsset<Motion>("Animation/ProxyAnim/proxy_hands_point.anim");
        }

        public static Motion ProxyHandsPeace()
        {
            return DemoAsset<Motion>("Animation/ProxyAnim/proxy_hands_peace.anim");
        }

        public static Motion ProxyHandsRock()
        {
            return DemoAsset<Motion>("Animation/ProxyAnim/proxy_hands_rock.anim");
        }

        public static Motion ProxyHandsGun()
        {
            return DemoAsset<Motion>("Animation/ProxyAnim/proxy_hands_gun.anim");
        }

        public static Motion ProxyHandsThumbsUp()
        {
            return DemoAsset<Motion>("Animation/ProxyAnim/proxy_hands_thumbs_up.anim");
        }

        public static Motion ProxyAfk()
        {
            return DemoAsset<Motion>("Animation/ProxyAnim/proxy_afk.anim");
        }

        public static Motion ProxyStandWave()
        {
            return DemoAsset<Motion>("Animation/ProxyAnim/proxy_stand_wave.anim");
        }

        public static Motion ProxyStandClap()
        {
            return DemoAsset<Motion>("Animation/ProxyAnim/proxy_stand_clap.anim");
        }

        public static Motion ProxyStandPoint()
        {
            return DemoAsset<Motion>("Animation/ProxyAnim/proxy_stand_point.anim");
        }

        public static Motion ProxyStandCheer()
        {
            return DemoAsset<Motion>("Animation/ProxyAnim/proxy_stand_cheer.anim");
        }

        public static Motion ProxyDance()
        {
            return DemoAsset<Motion>("Animation/ProxyAnim/proxy_dance.anim");
        }

        public static Motion ProxyBackflip()
        {
            return DemoAsset<Motion>("Animation/ProxyAnim/proxy_backflip.anim");
        }

        public static Motion ProxyStandSadkick()
        {
            return DemoAsset<Motion>("Animation/ProxyAnim/proxy_stand_sadkick.anim");
        }

        public static Motion ProxyDie()
        {
            return DemoAsset<Motion>("Animation/ProxyAnim/proxy_die.anim");
        }

        public static Motion ProxySupineWakeup()
        {
            return DemoAsset<Motion>("Animation/ProxyAnim/proxy_supine_getup.anim");
        }
    }
}