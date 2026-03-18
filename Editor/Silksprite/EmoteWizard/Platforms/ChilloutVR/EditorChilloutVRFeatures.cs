using System;
using System.Collections.Generic;
using System.Linq;
using Silksprite.AdLib.ChilloutVR;
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

        void IEditorPlatformFeatures.PopulateTriggerDriver(AnimatorState state, IEnumerable<(TrackingTarget target, TrackingMode mode)> settings)
        {
            var animatorDriver = state.AddStateMachineBehaviour2Access(CVRTypes.AnimatorDriver.Type, smb => new AnimatorDriverAccess(smb));
            animatorDriver.localOnly = true;

            animatorDriver.EnterTasks = settings.Select(setting => new AnimatorDriverTaskAccess
                {
                    targetType = AnimatorDriverTask_ParameterTypeAccess.EnumValues.Trigger.ToAccess(),
                    targetName = setting.target.ToAnimatorParameterName(setting.mode),

                    op = AnimatorDriverTask_OperatorAccess.EnumValues.Set.ToAccess(),

                    aType = AnimatorDriverTask_SourceTypeAccess.EnumValues.Static.ToAccess(),
                    aValue = 0f,
                    aMax = 0f,
                    aParamType = AnimatorDriverTask_ParameterTypeAccess.EnumValues.Trigger.ToAccess(),
                    aName = ""
                }).ToList();
        }

        void IEditorPlatformFeatures.PopulateTrackingControl(AnimatorState state, TrackingTarget target, TrackingMode mode)
        {
            var bodyControl = state.AddStateMachineBehaviour2Access(CVRTypes.BodyControl.Type, smb => new BodyControlAccess(smb));
            var targetWeight = mode switch
            {
                TrackingMode.Tracking => 1f,
                TrackingMode.Override => 0f,
                _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null)
            };
            void ConfigureEnterTask(BodyControlTask_BodyMaskAccess bodyMask)
            {
                bodyControl.EnterTasks = new List<BodyControlTaskAccess>
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
                    ConfigureEnterTask(BodyControlTask_BodyMaskAccess.EnumValues.Head.ToAccess()); 
                    break;
                case TrackingTarget.LeftHand:
                    ConfigureEnterTask(BodyControlTask_BodyMaskAccess.EnumValues.LeftArm.ToAccess()); 
                    break;
                case TrackingTarget.RightHand:
                    ConfigureEnterTask(BodyControlTask_BodyMaskAccess.EnumValues.RightArm.ToAccess()); 
                    break;
                case TrackingTarget.Hip:
                    ConfigureEnterTask(BodyControlTask_BodyMaskAccess.EnumValues.Pelvis.ToAccess()); 
                    break;
                case TrackingTarget.LeftFoot:
                    ConfigureEnterTask(BodyControlTask_BodyMaskAccess.EnumValues.LeftLeg.ToAccess()); 
                    break;
                case TrackingTarget.RightFoot:
                    ConfigureEnterTask(BodyControlTask_BodyMaskAccess.EnumValues.RightLeg.ToAccess()); 
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
