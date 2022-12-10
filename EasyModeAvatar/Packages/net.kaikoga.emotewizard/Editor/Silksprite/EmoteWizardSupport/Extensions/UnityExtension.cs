using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Silksprite.EmoteWizardSupport.Extensions
{
    public static class UnityExtension
    {
        public static bool IsPersistedAsset(this Object unityObject)
        {
            return !string.IsNullOrEmpty(AssetDatabase.GetAssetPath(unityObject));
        }

        public static T EnsureComponent<T>(this Component component, Action<T> initializer = null)
            where T : Component
        {
            var result = component.GetComponent<T>();
            if (result != null) return result;

            result = component.gameObject.AddComponent<T>();
            initializer?.Invoke(result);
            return result;
        }

        public static GameObject AddChildGameObject(this Component component, string path)
        {
            var paths = path.Split('/').ToArray();
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
            return gameObject;
        }

        public static T AddChildComponent<T>(this Component component, string path = null, Action<T> initializer = null)
            where T : Component
        {
            var t = AddChildGameObject(component, string.IsNullOrEmpty(path) ? typeof(T).Name : path).AddComponent<T>();
            initializer?.Invoke(t);
            return t;
        }

        public static T FindOrCreateChildComponent<T>(this Component component, string path = null, Action<T> initializer = null)
            where T : Component
        {
            var child = component.transform.Find(path);
            return child ? child.EnsureComponent<T>() : component.AddChildComponent(path, initializer);
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