using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Silksprite.AdLib.ChilloutVR.Access;
using Silksprite.AdLib.ChilloutVR.Extensions;
using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.Contexts.Builder;
using Silksprite.EmoteWizard.DataObjects.Internal;
using Silksprite.EmoteWizard.Platforms.ChilloutVR.Contexts.Extensions;
using Silksprite.EmoteWizard.Platforms.Common.Context.Extensions;
using Silksprite.EmoteWizard.Platforms.Extensions;
using Silksprite.EmoteWizard.Scopes;
using Silksprite.EmoteWizardSupport.Undoable;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Silksprite.EmoteWizard.Platforms.ChilloutVR.Contexts.Builder
{
    public class ChilloutVRAvatarBuilderContext : AvatarBuilderContextBase
    {
        [UsedImplicitly]
        public ChilloutVRAvatarBuilderContext(EmoteWizardEnvironment environment) : base(environment) { }

        public override void CleanupAvatar()
        {
            if (!Environment.AvatarRoot.TryGetCVRAvatarAccess(out var cvrAvatar)) return;

            if (Environment.AvatarRoot.TryGetComponent<Animator>(out var avatarAnimator))
            {
                avatarAnimator.runtimeAnimatorController = null;
            }
            
            CustomizeAnimationLayers(cvrAvatar, null, null);
        }

        public override void BuildAvatar(IUndoable undoable, bool manualBuild)
        {
            if (!Environment.AvatarRoot.TryGetCVRAvatarAccess(out var cvrAvatar)) return;

            var avatarAnimator = undoable.EnsureComponent<Animator>(cvrAvatar.BaseObject);
            avatarAnimator.runtimeAnimatorController = null;

            using (new ManualBundleGeneratedAssetsScope(Environment, manualBuild))
            {
                var parameters = Environment.GetContext<ParametersContext>().Snapshot(); 

                var baseController = Environment.GetContext<MergedLayerContext>().BuildOutputAsset(parameters);
                var overrideController = Environment.GetContext<OverrideControllerContext>().BuildOutputAsset(baseController);

                CustomizeAnimationLayers(cvrAvatar, baseController, overrideController);
                if (cvrAvatar.avatarSettings is { } avatarSettings)
                {
                    avatarSettings.settings = ExtractAdvancedSettingsEntries(parameters).ToList();
                }

                if (manualBuild)
                {
                    Environment.GetContext<EditorLayerContext>().BuildOutputAsset(parameters);
                }
            }
        }

        IEnumerable<CVRAdvancedSettingsEntryAccess?> ExtractAdvancedSettingsEntries(ParametersSnapshot snapshot)
        {
            var platformFeatures = Environment.GetPlatformFeatures();
            float[] zero = { 0f };

            foreach (var parameter in snapshot.AllParameters)
            {
                var valueKind = parameter.ValueKind;
                var writeSourceKind = parameter.WriteSourceKind;

                var usedType = valueKind switch
                {
                    ParameterValueKind.Bool => CVRAdvancesAvatarSettingBaseClass.ParameterTypeAccess.EnumValues.Bool,
                    ParameterValueKind.Int => CVRAdvancesAvatarSettingBaseClass.ParameterTypeAccess.EnumValues.Int,
                    ParameterValueKind.Float => CVRAdvancesAvatarSettingBaseClass.ParameterTypeAccess.EnumValues.Float,
                    ParameterValueKind.HandSign => CVRAdvancesAvatarSettingBaseClass.ParameterTypeAccess.EnumValues.Int,
                    _ => throw new ArgumentOutOfRangeException()
                };
                
                if (!(writeSourceKind switch
                {
                    ParameterWriteSourceKind.NoUI => (CVRAdvancedSettingsEntryClass.SettingsTypeAccess.EnumValues?)null,
                    ParameterWriteSourceKind.Button => CVRAdvancedSettingsEntryClass.SettingsTypeAccess.EnumValues.Toggle,
                    ParameterWriteSourceKind.Toggle => CVRAdvancedSettingsEntryClass.SettingsTypeAccess.EnumValues.Toggle,
                    ParameterWriteSourceKind.SubMenu => null,
                    ParameterWriteSourceKind.TwoAxisPuppet => CVRAdvancedSettingsEntryClass.SettingsTypeAccess.EnumValues.Joystick2D,
                    ParameterWriteSourceKind.FourAxisPuppet => null,
                    ParameterWriteSourceKind.RadialPuppet => CVRAdvancedSettingsEntryClass.SettingsTypeAccess.EnumValues.Slider,
                    _ => throw new ArgumentOutOfRangeException()
                } is { } maybeSettingsType))
                {
                    continue;
                }
                CVRAdvancesAvatarSettingBaseAccess setting;
                CVRAdvancedSettingsEntryClass.SettingsTypeAccess.EnumValues settingsType;
                switch (valueKind, maybeSettingsType)
                {
                    case (ParameterValueKind.Bool, CVRAdvancedSettingsEntryClass.SettingsTypeAccess.EnumValues.Toggle):
                        setting = new CVRAdvancesAvatarSettingGameObjectToggleAccess
                        {
                            usedType = usedType,
                            defaultValue = !parameter.DefaultValue.IsDefault
                        };
                        settingsType = CVRAdvancedSettingsEntryClass.SettingsTypeAccess.EnumValues.Toggle;
                        break;
                    case (ParameterValueKind.Float, CVRAdvancedSettingsEntryClass.SettingsTypeAccess.EnumValues.Joystick2D):
                        setting = new CVRAdvancesAvatarSettingJoystick2DAccess
                        {
                            usedType = usedType,
                            defaultValue = Vector2.zero,
                            rangeMin = new Vector2(-1f, -1f),
                            rangeMax = new Vector2(1f, 1f)
                        };
                        settingsType = CVRAdvancedSettingsEntryClass.SettingsTypeAccess.EnumValues.Joystick2D;
                        break;
                    case (_, _):
                        var readUsageValues = zero.Concat(parameter.ReadUsages.Select(usage => usage.Value.AsFloat(platformFeatures))).Distinct().ToArray();
                        setting = new CVRAdvancesAvatarSettingSliderAccess
                        {
                            usedType = usedType,
                            defaultValue = parameter.DefaultValue.AsFloat(platformFeatures),
                            materialPropertyTargets = new List<CVRAdvancedSettingsTargetEntryMaterialPropertyAccess?>
                            {
                                new CVRAdvancedSettingsTargetEntryMaterialPropertyAccess
                                {
                                    minValue = readUsageValues.Min(),
                                    maxValue = readUsageValues.Max()
                                }
                            }
                        };
                        settingsType = CVRAdvancedSettingsEntryClass.SettingsTypeAccess.EnumValues.Slider;
                        break;
                }
                yield return new CVRAdvancedSettingsEntryAccess
                {
                    type = settingsType,
                    setting = setting,
                    name = parameter.Name,
                    machineName = parameter.Name,
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
}
