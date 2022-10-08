using UnityEngine;

namespace Silksprite.EmoteWizardSupport.Utils
{
    public static class SerializableUtils
    {
        public static T Clone<T>(T item) where T : class => JsonUtility.FromJson<T>(JsonUtility.ToJson(item));
    }
}