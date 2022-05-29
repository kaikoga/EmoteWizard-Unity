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

        public static Texture2D ItemWand()
        {
            return LoadAssetByGuidOrPath<Texture2D>("0ff9333af28b8224893850e22c95e496", 
                "Assets/VRCSDK/Examples3/Expressions Menu/Icons/item_wand.png");
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

        public static Motion ProxyStandStill()
        {
            return LoadAssetByGuidOrPath<Motion>("91e5518865a04934b82b8aba11398609", 
                "Assets/VRCSDK/Examples3/Animation/ProxyAnim/proxy_stand_still.anim");
        }

        public static Motion ProxyStandStill2()
        {
            return LoadAssetByGuidOrPath<Motion>("09f2544a21594ef44925887662e24be6", 
                "Assets/VRCSDK/Examples3/Animation/ProxyAnim/proxy_stand_still2.anim");
        }

        public static Motion ProxyStandStill3()
        {
            return LoadAssetByGuidOrPath<Motion>("c6a07915cc1a9a644af7a5a358a6d3f1", 
                "Assets/VRCSDK/Examples3/Animation/ProxyAnim/proxy_stand_still3.anim");
        }

        public static Motion ProxySit()
        {
            return LoadAssetByGuidOrPath<Motion>("970f39cfa8501c741b71ad9eefeeb83d", 
                "Assets/VRCSDK/Examples3/Animation/ProxyAnim/proxy_sit.anim");
        }

        public static Motion ProxySit2()
        {
            return LoadAssetByGuidOrPath<Motion>("c91ab643200feef4fae5d09a7fdd410c", 
                "Assets/VRCSDK/Examples3/Animation/ProxyAnim/proxy_sit2.anim");
        }

        public static Motion ProxyAfk()
        {
            return LoadAssetByGuidOrPath<Motion>("806c242c97b686d4bac4ad50defd1fdb", 
                "Assets/VRCSDK/Examples3/Animation/ProxyAnim/proxy_afk.anim");
        }

        public static Motion ProxyStandWave()
        {
            return LoadAssetByGuidOrPath<Motion>("60873c431a64a744d87a5ad1e20bf886", 
                "Assets/VRCSDK/Examples3/Animation/ProxyAnim/proxy_stand_wave.anim");
        }

        public static Motion ProxyStandClap()
        {
            return LoadAssetByGuidOrPath<Motion>("44ce16481749f4c4baf0549d1bf3b3f3", 
                "Assets/VRCSDK/Examples3/Animation/ProxyAnim/proxy_stand_clap.anim");
        }

        public static Motion ProxyStandPoint()
        {
            return LoadAssetByGuidOrPath<Motion>("498e9dfd6d870064184180c5e4a3fc59", 
                "Assets/VRCSDK/Examples3/Animation/ProxyAnim/proxy_stand_point.anim");
        }

        public static Motion ProxyStandCheer()
        {
            return LoadAssetByGuidOrPath<Motion>("7359fa5b13647ba4986416b105f0d6dd", 
                "Assets/VRCSDK/Examples3/Animation/ProxyAnim/proxy_stand_cheer.anim");
        }

        public static Motion ProxyDance()
        {
            return LoadAssetByGuidOrPath<Motion>("0d2e5f9cc00d88a48b7bbe6e2898a4b4", 
                "Assets/VRCSDK/Examples3/Animation/ProxyAnim/proxy_dance.anim");
        }

        public static Motion ProxyBackflip()
        {
            return LoadAssetByGuidOrPath<Motion>("2af7e07b1514ac14bafe50d6b79cd07e", 
                "Assets/VRCSDK/Examples3/Animation/ProxyAnim/proxy_backflip.anim");
        }

        public static Motion ProxyStandSadkick()
        {
            return LoadAssetByGuidOrPath<Motion>("762c2cb22a9e6cc45803bd200a00c634", 
                "Assets/VRCSDK/Examples3/Animation/ProxyAnim/proxy_stand_sadkick.anim");
        }

        public static Motion ProxyDie()
        {
            return LoadAssetByGuidOrPath<Motion>("4cf06429686164a45adaedb6a6e520a5", 
                "Assets/VRCSDK/Examples3/Animation/ProxyAnim/proxy_die.anim");
        }

        public static Motion ProxySupineWakeup()
        {
            return LoadAssetByGuidOrPath<Motion>("ef56f98d2522d6b4387a112b015c6478", 
                "Assets/VRCSDK/Examples3/Animation/ProxyAnim/proxy_supine_getup.anim");
        }
    }
}