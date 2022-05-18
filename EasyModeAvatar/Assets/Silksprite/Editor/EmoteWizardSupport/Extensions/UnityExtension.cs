using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizardSupport.Extensions
{
    public static class UnityExtension
    {
        public static bool IsPersistedAsset(this Object unityObject)
        {
            return !string.IsNullOrEmpty(AssetDatabase.GetAssetPath(unityObject));
        }

        public static T AddChildComponent<T>(this Component component)
        where T : Component
        {
            var childObject = new GameObject(typeof(T).Name);
            childObject.transform.parent = component.transform;
            return childObject.AddComponent<T>();
        }

        public static T AddChildComponentAndSelect<T>(this Component component)
        where T : Component
        {
            var result = component.AddChildComponent<T>();
            Selection.SetActiveObjectWithContext(result.gameObject, result);
            return result;
        }
    }
}