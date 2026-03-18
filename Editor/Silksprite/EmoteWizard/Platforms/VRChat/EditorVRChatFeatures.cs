using System;
using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Platforms.Common;
using Silksprite.EmoteWizard.Platforms.Common.Extensions;
using UnityEditor.Animations;
using VRC.SDK3.Avatars.Components;
using VRC.SDKBase;

namespace Silksprite.EmoteWizard.Platforms.VRChat
{
    public class EditorVRChatFeatures : IEditorPlatformFeatures
    {
        public static readonly IEditorPlatformFeatures Instance = new EditorVRChatFeatures();

        void IEditorPlatformFeatures.PopulateTriggerDriver(AnimatorState state, IEnumerable<(TrackingTarget target, bool isOn)> settings)
        {
            var avatarParameterDriver = state.AddStateMachineBehaviour2<VRCAvatarParameterDriver>();
            avatarParameterDriver.localOnly = true;

            avatarParameterDriver.parameters = settings.Select(setting => new VRC_AvatarParameterDriver.Parameter
            {
                name = setting.target.ToAnimatorParameterName(setting.isOn),
                value = 0f,
                valueMin = 0f,
                valueMax = 0f,
                chance = 1f,
                type = VRC_AvatarParameterDriver.ChangeType.Set
            }).ToList();
        }

        void IEditorPlatformFeatures.PopulateTrackingControl(AnimatorState state, TrackingTarget target, float targetWeight)
        {
            var trackingControl = state.AddStateMachineBehaviour2<VRCAnimatorTrackingControl>();
            var value = targetWeight switch
            {
                0f => VRC_AnimatorTrackingControl.TrackingType.Animation,
                1f => VRC_AnimatorTrackingControl.TrackingType.Tracking,
                _ => throw new ArgumentOutOfRangeException(nameof(targetWeight), targetWeight, null)
            };
            switch (target)
            {
                case TrackingTarget.Head:
                    trackingControl.trackingHead = value; 
                    break;
                case TrackingTarget.LeftHand:
                    trackingControl.trackingLeftHand = value; 
                    break;
                case TrackingTarget.RightHand:
                    trackingControl.trackingRightHand = value; 
                    break;
                case TrackingTarget.Hip:
                    trackingControl.trackingHip = value; 
                    break;
                case TrackingTarget.LeftFoot:
                    trackingControl.trackingLeftFoot = value; 
                    break;
                case TrackingTarget.RightFoot:
                    trackingControl.trackingRightFoot = value; 
                    break;
                case TrackingTarget.LeftFingers:
                    trackingControl.trackingLeftFingers = value; 
                    break;
                case TrackingTarget.RightFingers:
                    trackingControl.trackingRightFingers = value; 
                    break;
                case TrackingTarget.Eyes:
                    trackingControl.trackingEyes = value; 
                    // TODO: Reset blink blend shape states (if any)
                    break;
                case TrackingTarget.Mouth:
                    trackingControl.trackingMouth = value; 
                    // TODO: Reset lip sync blend shape states (if any) (should we?)
                    break;
                default:
                    throw new ArgumentOutOfRangeException(target.ToString());
            }
        }

        void IEditorPlatformFeatures.PopulateLayerControl(AnimatorState state, float goalWeight, float duration)
        {
            var playableLayerControl = state.AddStateMachineBehaviour2<VRCPlayableLayerControl>();
            playableLayerControl.layer = VRC_PlayableLayerControl.BlendableLayer.Action;
            playableLayerControl.goalWeight = goalWeight;
            playableLayerControl.blendDuration = duration;
        }
    }
}
