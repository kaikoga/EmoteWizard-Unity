using System.Collections.Generic;
using Silksprite.AdLib.ChilloutVR.Access;
using Silksprite.AdLib.ChilloutVR.Extensions;
using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.Scopes;
using Silksprite.EmoteWizardSupport.Undoable;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Silksprite.EmoteWizard.Platforms.ChilloutVR.Contexts.Extensions
{
    public static class EmoteWizardEnvironmentExtension
    {
        public static void CleanupCvrAvatar(this EmoteWizardEnvironment environment)
        {
            if (!environment.AvatarRoot.TryGetCVRAvatarAccess(out var cvrAvatar)) return;

            if (environment.AvatarRoot.TryGetComponent<Animator>(out var avatarAnimator))
            {
                avatarAnimator.runtimeAnimatorController = null;
            }
            
            CustomizeAnimationLayers(cvrAvatar, null);
            if (cvrAvatar.avatarSettings is { } avatarSettings)
            {
                avatarSettings.settings = new List<CVRAdvancedSettingsEntryAccess>();
            }
        }

        public static void BuildCvrAvatar(this EmoteWizardEnvironment environment, IUndoable undoable, bool manualBuild)
        {
            if (!environment.AvatarRoot.TryGetCVRAvatarAccess(out var cvrAvatar)) return;

            var avatarAnimator = undoable.EnsureComponent<Animator>(cvrAvatar.BaseObject);
            avatarAnimator.runtimeAnimatorController = null;

            using (new ManualBundleGeneratedAssetsScope(environment, manualBuild))
            {
                var parameters = environment.GetContext<ParametersContext>().Snapshot(); 

                var baseController = environment.GetContext<BaseControllerContext>().BuildOutputAsset(parameters);
                var overrideController = environment.GetContext<OverrideControllerContext>().BuildOutputAsset(baseController);

                CustomizeAnimationLayers(cvrAvatar, overrideController);
                if (cvrAvatar.avatarSettings is { } avatarSettings)
                {
                    avatarSettings.settings = new List<CVRAdvancedSettingsEntryAccess>();
                }

                if (manualBuild)
                {
                    environment.GetContext<EditorLayerContext>().BuildOutputAsset(parameters);
                }
            }
        }

        static void CustomizeAnimationLayers(CVRAvatarAccess cvrAvatar, AnimatorOverrideController overrideController)
        {
            cvrAvatar.overrides = overrideController;
            cvrAvatar.avatarUsesAdvancedSettings = true;
        }
    }

    class ManualBundleGeneratedAssetsScope : ManualBundleGeneratedAssetsScopeBase
    {
        public ManualBundleGeneratedAssetsScope(EmoteWizardEnvironment environment, bool manualBuild) : base(environment, manualBuild)
        {
        }

        protected override IEnumerable<Object> CollectVolatileAssets(EmoteWizardEnvironment environment)
        {
            // manually try to persist volatile layers because layers are what Emote Wizard generates
            if (!environment.AvatarRoot.TryGetCVRAvatarAccess(out var cvrAvatar)) yield break;

            // FIXME: can we handle override controllers yet? 
            yield return cvrAvatar.overrides;
        }
    }
}