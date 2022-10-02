using System.Linq;
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

        public static T AddChildComponent<T>(this Component component, string path = null)
            where T : Component
        {
            var paths = string.IsNullOrEmpty(path) ? new[] { typeof(T).Name } : path.Split('/').ToArray();
            var gameObject = component.gameObject;
            foreach (var name in paths)
            {
                var childTransform = gameObject.transform.Find(name);
                if (childTransform)
                {
                    gameObject = childTransform.gameObject;
                }
                else
                {
                    var newChildObject = new GameObject(name);
                    newChildObject.transform.parent = gameObject.transform;
                    gameObject = newChildObject;
                }
            }
            return gameObject.AddComponent<T>();
        }

        public static T FindOrCreateChildComponent<T>(this Component component, string path = null)
            where T : Component
        {
            var child = component.transform.Find(path);
            return child ? child.EnsureComponent<T>() : component.AddChildComponent<T>(path);
        }

        public static T AddChildComponentAndSelect<T>(this Component component, string path = null)
        where T : Component
        {
            var result = component.AddChildComponent<T>(path);
            Selection.SetActiveObjectWithContext(result.gameObject, result);
            return result;
        }
    }
}