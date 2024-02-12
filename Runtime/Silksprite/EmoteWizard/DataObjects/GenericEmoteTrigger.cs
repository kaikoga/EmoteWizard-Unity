using System;
using JetBrains.Annotations;
using UnityEngine;

namespace Silksprite.EmoteWizard.DataObjects
{
    [Serializable]
    public struct GenericEmoteTrigger
    {
        [SerializeField] public string name;
        [SerializeField] public Platform platform;

        [SerializeField] public int value;

        [Pure]
        public bool TryGetHandSign(out HandSign handSign)
        {
            switch (platform)
            {
                case Platform.VRChat:
                    handSign = (HandSign)value;
                    return true;
                default:
                    handSign = default;
                    return false;
            }
        }

        [Pure]
        public bool TryGetVrm0BlendShape(out Vrm0BlendShapePreset vrm0BlendShape)
        {
            switch (platform)
            {
                case Platform.VRM0:
                    vrm0BlendShape = (Vrm0BlendShapePreset)value;
                    return true;
                default:
                    vrm0BlendShape = default;
                    return false;
            }
        }

        [Pure]
        public bool TryGetVrm1ExpressionPreset(out Vrm1ExpressionPreset vrm1Expression)
        {
            switch (platform)
            {
                case Platform.VRM1:
                    vrm1Expression = (Vrm1ExpressionPreset)value;
                    return true;
                default:
                    vrm1Expression = default;
                    return false;
            }
        }

        public static GenericEmoteTrigger FromHandSign(HandSign fromHandSign)
        {
            return new GenericEmoteTrigger
            {
                platform = Platform.VRChat,
                value = (int)fromHandSign
            };
        }

        public static GenericEmoteTrigger FromVrm0BlendShape(Vrm0BlendShapePreset vrm0BlendShape)
        {
            return new GenericEmoteTrigger
            {
                platform = Platform.VRM0,
                value = (int)vrm0BlendShape
            };
        }
    }
}