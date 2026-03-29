using System;
using System.Collections.Generic;
using System.Linq;
using Silksprite.AdLib.ChilloutVR.Access;
using Silksprite.AdLib.ChilloutVR.Extensions;
using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.DataObjects.Internal;
using Silksprite.EmoteWizard.Platforms.Common.Context.Extensions;
using Silksprite.EmoteWizard.Platforms.Extensions;
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
            
            CustomizeAnimationLayers(cvrAvatar, null, null);
        }

        public static void BuildCvrAvatar(this EmoteWizardEnvironment environment, IUndoable undoable, bool manualBuild)
        {
            if (!environment.AvatarRoot.TryGetCVRAvatarAccess(out var cvrAvatar)) return;

            var avatarAnimator = undoable.EnsureComponent<Animator>(cvrAvatar.BaseObject);
            avatarAnimator.runtimeAnimatorController = null;

            using (new ManualBundleGeneratedAssetsScope(environment, manualBuild))
            {
                var parameters = environment.GetContext<ParametersContext>().Snapshot(); 

                var baseController = environment.GetContext<MergedLayerContext>().BuildOutputAsset(parameters);
                var overrideController = environment.GetContext<OverrideControllerContext>().BuildOutputAsset(baseController);

                CustomizeAnimationLayers(cvrAvatar, baseController, overrideController);
                if (cvrAvatar.avatarSettings is { } avatarSettings)
                {
                    avatarSettings.settings = parameters.ExtractAdvancedSettingsEntries(environment).ToList();
                }

                if (manualBuild)
                {
                    environment.GetContext<EditorLayerContext>().BuildOutputAsset(parameters);
                }
            }
        }

        static IEnumerable<CVRAdvancedSettingsEntryAccess?> ExtractAdvancedSettingsEntries(this ParametersSnapshot snapshot, EmoteWizardEnvironment environment)
        {
            var platformFeatures = environment.GetPlatformFeatures();

            foreach (var parameter in snapshot.AllParameters)
            {
                var valueKind = parameter.ValueKind;
                var writeSourceKind = parameter.WriteSourceKind;

                var usedType = new CVRAdvancesAvatarSettingBase_ParameterTypeAccess(valueKind switch
                {
                    ParameterValueKind.Bool => CVRAdvancesAvatarSettingBase_ParameterTypeAccess.EnumValues.Bool,
                    ParameterValueKind.Int => CVRAdvancesAvatarSettingBase_ParameterTypeAccess.EnumValues.Int,
                    ParameterValueKind.Float => CVRAdvancesAvatarSettingBase_ParameterTypeAccess.EnumValues.Float,
                    ParameterValueKind.HandSign => CVRAdvancesAvatarSettingBase_ParameterTypeAccess.EnumValues.Int,
                    _ => throw new ArgumentOutOfRangeException()
                });
                
                if (!(writeSourceKind switch
                {
                    ParameterWriteSourceKind.NoUI => (CVRAdvancedSettingsEntry_SettingsTypeAccess.EnumValues?)null,
                    ParameterWriteSourceKind.Button => CVRAdvancedSettingsEntry_SettingsTypeAccess.EnumValues.Toggle,
                    ParameterWriteSourceKind.Toggle => CVRAdvancedSettingsEntry_SettingsTypeAccess.EnumValues.Toggle,
                    ParameterWriteSourceKind.SubMenu => null,
                    ParameterWriteSourceKind.TwoAxisPuppet => CVRAdvancedSettingsEntry_SettingsTypeAccess.EnumValues.Joystick2D,
                    ParameterWriteSourceKind.FourAxisPuppet => null,
                    ParameterWriteSourceKind.RadialPuppet => CVRAdvancedSettingsEntry_SettingsTypeAccess.EnumValues.Slider,
                    _ => throw new ArgumentOutOfRangeException()
                } is { } maybeSettingsType))
                {
                    continue;
                }
                CVRAdvancesAvatarSettingBaseAccess setting;
                CVRAdvancedSettingsEntry_SettingsTypeAccess.EnumValues settingsType;
                switch (valueKind, maybeSettingsType)
                {
                    case (ParameterValueKind.Bool, CVRAdvancedSettingsEntry_SettingsTypeAccess.EnumValues.Toggle):
                        setting = new CVRAdvancesAvatarSettingGameObjectToggleAccess
                        {
                            usedType = usedType,
                            defaultValue = parameter.defaultValue != 0
                        };
                        settingsType = CVRAdvancedSettingsEntry_SettingsTypeAccess.EnumValues.Toggle;
                        break;
                    case (ParameterValueKind.Float, CVRAdvancedSettingsEntry_SettingsTypeAccess.EnumValues.Joystick2D):
                        setting = new CVRAdvancesAvatarSettingJoystick2DAccess
                        {
                            usedType = usedType,
                            defaultValue = Vector2.zero,
                            rangeMin = new Vector2(-1f, -1f),
                            rangeMax = new Vector2(1f, 1f)
                        };
                        settingsType = CVRAdvancedSettingsEntry_SettingsTypeAccess.EnumValues.Joystick2D;
                        break;
                    case (_, _):
                        setting = new CVRAdvancesAvatarSettingSliderAccess
                        {
                            usedType = usedType,
                            defaultValue = parameter.defaultValue,
                            materialPropertyTargets = new List<CVRAdvancedSettingsTargetEntryMaterialPropertyAccess?>
                            {
                                new CVRAdvancedSettingsTargetEntryMaterialPropertyAccess
                                {
                                    minValue = parameter.readUsages.Select(usage => usage.Value.AsFloat(platformFeatures)).Min(),
                                    maxValue = parameter.readUsages.Select(usage => usage.Value.AsFloat(platformFeatures)).Max()
                                }
                            }
                        };
                        settingsType = CVRAdvancedSettingsEntry_SettingsTypeAccess.EnumValues.Slider;
                        break;
                }
                yield return new CVRAdvancedSettingsEntryAccess
                {
                    type = new CVRAdvancedSettingsEntry_SettingsTypeAccess(settingsType),
                    setting = setting,
                    name = parameter.name,
                    machineName = parameter.name,
                };

            }
        }

        static void CustomizeAnimationLayers(CVRAvatarAccess cvrAvatar, RuntimeAnimatorController? baseController, AnimatorOverrideController? overrideController)
        {
            cvrAvatar.overrides = overrideController;
            cvrAvatar.avatarSettings = new CVRAdvancedAvatarSettingsAccess
            {
                settings = new List<CVRAdvancedSettingsEntryAccess?>(),
                baseController = baseController,
                baseOverrideController = overrideController,
                initialized = true
            };
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
            if (cvrAvatar.overrides != null)
            {
                yield return cvrAvatar.overrides;
            }
        }
    }
}