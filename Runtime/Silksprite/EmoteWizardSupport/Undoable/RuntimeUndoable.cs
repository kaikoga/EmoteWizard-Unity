using JetBrains.Annotations;
using UnityEngine;

namespace Silksprite.EmoteWizardSupport.Undoable
{
    public class RuntimeUndoable : IUndoable
    {
        [PublicAPI]
        public static IUndoable Instance => new RuntimeUndoable();

        public GameObject CreateGameObject(string name) => new(name);

        public T AddComponent<T>(Component component) where T : Component => component.gameObject.AddComponent<T>();
    }
}