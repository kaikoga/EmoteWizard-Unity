using System;
using System.Collections.Generic;
using System.Linq;
using Silksprite.AdLib.ChilloutVR;
using Silksprite.AdLib.ChilloutVR.Access;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Platforms.Common.Extensions;
using Silksprite.EmoteWizard.Platforms.Common.Internal.Extensions;
using UnityEditor.Animations;

namespace Silksprite.EmoteWizard.Platforms.Common
{
    public class EditorChilloutVRFeatures : IEditorPlatformFeatures
    {
        public static readonly EditorChilloutVRFeatures Instance = new EditorChilloutVRFeatures();

        public void PopulateParameterDriver(AnimatorState state, bool isEntry, bool isPersisted, TrackingTarget[] targets, TrackingTarget[] currentTrackingTargets)
        {
            var animatorDriver = state.AddStateMachineBehaviour2Access(CVRTypes.AnimatorDriver.Type, smb => new AnimatorDriverAccess(smb), isPersisted);
            animatorDriver.localOnly = true;

            animatorDriver.EnterTasks = targets.Select(target => new AnimatorDriverTaskAccess
            {
                targetType = AnimatorDriverTask_ParameterTypeAccess.EnumValues.Float.ToAccess(),
                targetName = target.ToAnimatorParameterName(isEntry && currentTrackingTargets.Contains(target)),

                op = AnimatorDriverTask_OperatorAccess.EnumValues.Set.ToAccess(),

                aType = AnimatorDriverTask_SourceTypeAccess.EnumValues.Static.ToAccess(),
                aValue = 0f,
                aMax = 0f,
                aParamType = AnimatorDriverTask_ParameterTypeAccess.EnumValues.Float.ToAccess(),
                aName = ""
            }).ToList();
        }
        public void PopulateBodyControl(AnimatorState state, TrackingTarget target, float targetWeight, bool isPersisted)
        {
            var bodyControl = state.AddStateMachineBehaviour2Access(CVRTypes.BodyControl.Type, smb => new BodyControlAccess(smb), isPersisted);
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
    }
}
