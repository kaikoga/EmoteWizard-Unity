using UnityEditor;
using UnityEngine;

#if EW_VRCSDK3_AVATARS
using VRC.SDK3.Avatars.Components;
#endif

namespace Silksprite.EmoteWizard
{
    public static class EmoteWizardMenuItems
    {
        [MenuItem("GameObject/Emote Wizard/Emote Wizard Root", false, 20)]
        public static void CreateRoot(MenuCommand menuCommand)
        {
            var gameObject = new GameObject("Emote Wizard");
            var root = gameObject.AddComponent<EmoteWizardRoot>();
            if (menuCommand.context is GameObject avatar)
            {
                root.transform.SetParent(avatar.transform);
                var avatarRootIsConfigured = false;
#if EW_VRCSDK3_AVATARS
                if (avatar.TryGetComponent<VRCAvatarDescriptor>(out var avatarDescriptor))
                {
                    root.avatarDescriptor = avatarDescriptor;
                    avatarRootIsConfigured = true;
                }
#endif
                if (!avatarRootIsConfigured)
                {
                    root.avatarRootTransform = avatar.transform;
                }
            }
            Undo.RegisterCreatedObjectUndo(gameObject, "Create Emote Wizard");
        }
        
        [MenuItem("GameObject/Emote Wizard/Emote Wizard Data Source", false, 20)]
        public static void CreateDataSource(MenuCommand menuCommand)
        {
            var gameObject = new GameObject("New Source");
            var root = gameObject.AddComponent<EmoteWizardDataSourceFactory>();
            if (menuCommand.context is GameObject parent)
            {
                root.transform.SetParent(parent.transform);
            }
            Undo.RegisterCreatedObjectUndo(gameObject, "Create Emote Wizard Data Source");
        }
    }
}