using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizardSupport.Undoable
{
    public static class UndoableExtensions
    {
        public static T EnsureComponent<T>(this IUndoable undoable, Component component, Action<T> initializer = null)
            where T : Component
        {
            var result = component.GetComponent<T>();
            if (result != null) return result;

            result = undoable.AddComponent<T>(component);
            initializer?.Invoke(result);
            return result;
        }

        public static GameObject AddChildGameObject(this IUndoable undoable, Component parent, string path)
        {
            var paths = path.Split('/').ToArray();
            var gameObject = parent.gameObject;
            for (var i = 0; i < paths.Length; i++)
            {
                var name = paths[i];
                var isLeaf = i == paths.Length - 1;
                if (gameObject.transform.Find(name) is Transform childTransform)
                {
                    if (isLeaf)
                    {
                        name = GameObjectUtility.GetUniqueNameForSibling(gameObject.transform, name);
                    }
                    else
                    {
                        gameObject = childTransform.gameObject;
                        continue;
                    }
                }
                var newChildObject = undoable.CreateGameObject(name);
                newChildObject.transform.SetParent(gameObject.transform);
                gameObject = newChildObject;
            }

            return gameObject;
        }
        
        public static T AddChildComponent<T>(this IUndoable undoable, Component component, string path = null, Action<T> initializer = null)
            where T : Component
        {
            var t = undoable.AddComponent<T>(undoable.AddChildGameObject(component, string.IsNullOrEmpty(path) ? typeof(T).Name : path).transform);
            initializer?.Invoke(t);
            return t;
        }

        public static T FindOrCreateChildComponent<T>(this IUndoable undoable, Component component, string path = null, Action<T> initializer = null)
            where T : Component
        {
            var child = component.transform.Find(path);
            return child ? undoable.EnsureComponent<T>(child) : undoable.AddChildComponent(component, path, initializer);
        }
    }
}