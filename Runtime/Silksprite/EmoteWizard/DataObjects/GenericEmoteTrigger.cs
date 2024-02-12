using System;
using UnityEngine;

namespace Silksprite.EmoteWizard.DataObjects
{
    [Serializable]
    public struct GenericEmoteTrigger
    {
        [SerializeField] public Platform platform;

        [SerializeField] public HandSign handSign;

        public static GenericEmoteTrigger FromHandSign(HandSign fromHandSign)
        {
            return new GenericEmoteTrigger
            {
                platform = Platform.VRChat,
                handSign = fromHandSign
            };
        }
    }

    public enum Platform
    {
        [InspectorName("VRChat")]
        VRChat = 0
    }
}