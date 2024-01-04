using UnityEngine;

namespace Silksprite.EmoteWizardSupport.Undoable
{
    public interface IUndoable
    {
        GameObject CreateGameObject(string name);

        T AddComponent<T>(Component component) where T : Component;
    }
}