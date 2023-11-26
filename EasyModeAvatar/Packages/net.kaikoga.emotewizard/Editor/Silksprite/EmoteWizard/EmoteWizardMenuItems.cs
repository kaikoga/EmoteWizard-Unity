using UnityEditor;
using UnityEngine;
using VRC.SDK3.Avatars.Components;

namespace Silksprite.EmoteWizard
{
    public static class EmoteWizardMenuItems
    {
        [MenuItem("GameObject/Emote Wizard", false, 20)]
        public static void Create(MenuCommand menuCommand)
        {
            var gameObject = new GameObject("Emote Wizard");
            var root = gameObject.AddComponent<EmoteWizardRoot>();
            if (menuCommand.context is GameObject avatar && avatar.TryGetComponent<VRCAvatarDescriptor>(out var avatarDescriptor))
            {
                root.transform.SetParent(avatar.transform);
                root.avatarDescriptor = avatarDescriptor;
            }
            Undo.RegisterCreatedObjectUndo(gameObject, "Create Emote Wizard");
        }
    }
}