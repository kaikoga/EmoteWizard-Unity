using System;
using System.Collections.Generic;
using System.Linq;
using Silksprite.AdLib.ChilloutVR.Access;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Platforms.ChilloutVR.Extensions;
using Silksprite.EmoteWizard.Platforms.Common;
using Silksprite.EmoteWizard.Platforms.Common.Extensions;
using UnityEditor.Animations;

namespace Silksprite.EmoteWizard.Platforms.ChilloutVR
{
    public class EditorChilloutVRFeatures : IEditorPlatformFeatures
    {
        public static readonly IEditorPlatformFeatures Instance = new EditorChilloutVRFeatures();

        void IEditorPlatformFeatures.PopulateParameterDriver(AnimatorState state, string parameterName, float parameterValue)
        {
            var animatorDriver = state.AddStateMachineBehaviour2Access(AnimatorDriverAccess.ActualType, smb => new AnimatorDriverAccess(smb));
            animatorDriver.localOnly = true;

            animatorDriver.EnterTasks = new List<AnimatorDriverTaskAccess?>
            {
                new AnimatorDriverTaskAccess{
                    targetType = AnimatorDriverTaskClass.ParameterTypeAccess.EnumValues.Float,
                    targetName = parameterName,

                    op = AnimatorDriverTaskClass.OperatorAccess.EnumValues.Set,

                    aType = AnimatorDriverTaskClass.SourceTypeAccess.EnumValues.Static,
                    aValue = 0f,
                    aMax = 0f,
                    aParamType = AnimatorDriverTaskClass.ParameterTypeAccess.EnumValues.Trigger,
                    aName = ""
                }
            };
        }

        void IEditorPlatformFeatures.PopulateTriggerDriver(AnimatorState state, IEnumerable<(TrackingTarget target, TrackingMode mode)> settings)
        {
            var animatorDriver = state.AddStateMachineBehaviour2Access(AnimatorDriverAccess.ActualType, smb => new AnimatorDriverAccess(smb));
            animatorDriver.localOnly = true;

            animatorDriver.EnterTasks = settings.Select(setting => (AnimatorDriverTaskAccess?)new AnimatorDriverTaskAccess
                {
                    targetType = AnimatorDriverTaskClass.ParameterTypeAccess.EnumValues.Trigger,
                    targetName = setting.target.ToAnimatorParameterName(setting.mode),

                    op = AnimatorDriverTaskClass.OperatorAccess.EnumValues.Set,

                    aType = AnimatorDriverTaskClass.SourceTypeAccess.EnumValues.Static,
                    aValue = 0f,
                    aMax = 0f,
                    aParamType = AnimatorDriverTaskClass.ParameterTypeAccess.EnumValues.Trigger,
                    aName = ""
                }).ToList();
        }

        void IEditorPlatformFeatures.PopulateTrackingControl(AnimatorState state, TrackingTarget target, TrackingMode mode)
        {
            var bodyControl = state.AddStateMachineBehaviour2Access(BodyControlAccess.ActualType, smb => new BodyControlAccess(smb));
            var targetWeight = mode switch
            {
                TrackingMode.Tracking => 1f,
                TrackingMode.Override => 0f,
                _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null)
            };
            void ConfigureEnterTask(BodyControlTaskClass.BodyMaskAccess.EnumValues bodyMask)
            {
                bodyControl.EnterTasks = new List<BodyControlTaskAccess?>
                {
                    new BodyControlTaskAccess
                    {
                        target = bodyMask,
                        targetWeight = targetWeight
                    }
                };
            }
            switch (target)
            {
                case TrackingTarget.Head:
                    ConfigureEnterTask(BodyControlTaskClass.BodyMaskAccess.EnumValues.Head); 
                    break;
                case TrackingTarget.LeftHand:
                    ConfigureEnterTask(BodyControlTaskClass.BodyMaskAccess.EnumValues.LeftArm); 
                    break;
                case TrackingTarget.RightHand:
                    ConfigureEnterTask(BodyControlTaskClass.BodyMaskAccess.EnumValues.RightArm); 
                    break;
                case TrackingTarget.Hip:
                    ConfigureEnterTask(BodyControlTaskClass.BodyMaskAccess.EnumValues.Pelvis); 
                    break;
                case TrackingTarget.LeftFoot:
                    ConfigureEnterTask(BodyControlTaskClass.BodyMaskAccess.EnumValues.LeftLeg); 
                    break;
                case TrackingTarget.RightFoot:
                    ConfigureEnterTask(BodyControlTaskClass.BodyMaskAccess.EnumValues.RightLeg); 
                    break;
                case TrackingTarget.LeftFingers:
                case TrackingTarget.RightFingers:
                case TrackingTarget.Eyes:
                case TrackingTarget.Mouth:
                    // not available
                    break;
                default:
                    throw new ArgumentOutOfRangeException(target.ToString());
            }
        }

        void IEditorPlatformFeatures.PopulateLayerControl(AnimatorState state, float goalWeight, float duration)
        {
            // do nothing
        }
    }
}
