using System;
using JetBrains.Annotations;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Silksprite.EmoteWizardSupport.Undoable
{
    public class RuntimeUndoable : IUndoable
    {
        [PublicAPI]
        public static IUndoable Instance => new RuntimeUndoable();

        public void RecordObject(Object obj) { } // noop

        public GameObject CreateGameObject(string name) => new GameObject(name);

        public T AddComponent<T>(GameObject gameObject) where T : Component => gameObject.AddComponent<T>();
        public T AddComponent<T>(Component component) where T : Component => component.gameObject.AddComponent<T>();

        public void DestroyObject(Object obj) => Object.Destroy(obj);
        
        public void SetActiveObjectWithContext(Object obj, Object context) { } // noop

        public string GetUniqueNameForSibling(Transform parent, string name) => parent.Find(name) ? new Guid().ToString() : name;
    }
}