using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizard.Extensions
{
    public static class UnityExtension
    {
        public static T EnsureComponent<T>(this GameObject gameObject)
            where T : Component =>
            gameObject.GetComponent<T>() ?? gameObject.AddComponent<T>();
        
        public static T EnsureComponent<T>(this Component component)
            where T : Component =>
            component.GetComponent<T>() ?? component.gameObject.AddComponent<T>();

        public static bool IsPersistedAsset(this Object unityObject)
        {
            return !string.IsNullOrEmpty(AssetDatabase.GetAssetPath(unityObject));
        }
    }
}