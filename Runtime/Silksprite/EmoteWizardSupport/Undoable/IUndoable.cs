using UnityEngine;

namespace Silksprite.EmoteWizardSupport.Undoable
{
    public interface IUndoable
    {
        GameObject CreateGameObject(string name);

        T AddComponent<T>(GameObject component) where T : Component;
        T AddComponent<T>(Component component) where T : Component;

        void DestroyObject(Object obj);
    }
}