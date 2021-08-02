using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Extensions;
using Silksprite.EmoteWizard.Utils;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using VRC.SDK3.Avatars.Components;
using VRC.SDKBase;

namespace Silksprite.EmoteWizard.Internal
{
    public class ActionControllerBuilder
    {
        public ActionWizard ActionWizard;
        public string DefaultRelativePath;

        AnimatorController _animatorController;
        AnimatorController AnimatorController
        {
            get
            {
                if (_animatorController) return _animatorController;
                return _animatorController = ActionWizard.ReplaceOrCreateOutputAsset(ref ActionWizard.outputAsset, DefaultRelativePath);
            }
        }

        AnimatorControllerLayer _actionLayer;
        AnimatorControllerLayer ActionLayer
        {
            get
            {
                if (_actionLayer != null) return _actionLayer;
                _actionLayer = new AnimatorControllerLayer
                {
                    name = "Action",
                    defaultWeight = 1.0f,
                    stateMachine = new AnimatorStateMachine
                    {
                        name = "Action",
                        hideFlags = HideFlags.HideInHierarchy,
                        anyStatePosition = Position(0, 0),
                        entryPosition = Position(0, 1),
                        exitPosition = Position(7, 0)
                    }
                };

                AssetDatabase.AddObjectToAsset(_actionLayer.stateMachine, AssetDatabase.GetAssetPath(AnimatorController));
                AnimatorController.AddLayer(_actionLayer);
                return _actionLayer;
            }
        }

        AnimatorStateMachine _stateMachine;
        AnimatorStateMachine StateMachine
        {
            get
            {
                if (_stateMachine != null) return _stateMachine;
                return _stateMachine = ActionLayer.stateMachine;
            }
        }

        Vector3 Position(int x, int y)
        {
            return new Vector3(x * 300, y * 60);
        }

        public void BuildActionLayer()
        {
            var standClip = VrcSdkAssetLocator.ProxyStandStill();
            var sitClip = VrcSdkAssetLocator.ProxySit();
            var afkClip = VrcSdkAssetLocator.ProxyAfk();

            var sitY = ActionWizard.actionEmotes.Count + 2;
            var afkY = sitY + 2;
            var stand = AddState("Stand", Position(1, 0), standClip);
            var sit = AddState("Sit", Position(1, sitY), sitClip);
            var afk = AddState("AFK Select", Position(2, afkY), afkClip);
            var standRelease = AddState("Restore Standing", Position(7, 0), standClip);
            var sitRelease = AddState("Restore Sitting", Position(7, sitY), sitClip);
            var afkRelease = AddState("Restore AFK", Position(6, afkY), afkClip);

            PopulateTrackingControl(standRelease, VRC_AnimatorTrackingControl.TrackingType.Tracking);
            PopulateTrackingControl(sitRelease, VRC_AnimatorTrackingControl.TrackingType.Tracking);
            PopulateTrackingControl(afkRelease, VRC_AnimatorTrackingControl.TrackingType.Tracking);

            PopulatePlayableLayerControl(standRelease, VRC_PlayableLayerControl.BlendableLayer.Action, 0f, 0.25f);
            PopulatePlayableLayerControl(sitRelease, VRC_PlayableLayerControl.BlendableLayer.Action, 0f, 0.25f);
            PopulatePlayableLayerControl(afkRelease, VRC_PlayableLayerControl.BlendableLayer.Action, 0f, 0.5f);

            StateMachine.defaultState = stand;
            var standToSit = stand.AddTransition(sit);
            standToSit.AddCondition(AnimatorConditionMode.If, 0, "Seated");
            var sitToStand = sit.AddTransition(stand);
            sitToStand.AddCondition(AnimatorConditionMode.IfNot, 0, "Seated");
            var standToAfk = stand.AddTransition(afk);
            standToAfk.AddCondition(AnimatorConditionMode.If, 0, "AFK");
            var sitToAfk = sit.AddTransition(afk);
            sitToAfk.AddCondition(AnimatorConditionMode.If, 0, "AFK");
            var completeStand = standRelease.AddTransition(stand);
            completeStand.AddCondition(AnimatorConditionMode.If, 0, "Seated");
            completeStand = standRelease.AddTransition(stand);
            completeStand.AddCondition(AnimatorConditionMode.IfNot, 0, "Seated");
            var completeSit = sitRelease.AddTransition(sit);
            completeSit.AddCondition(AnimatorConditionMode.If, 0, "Seated");
            completeSit = sitRelease.AddTransition(sit);
            completeSit.AddCondition(AnimatorConditionMode.IfNot, 0, "Seated");
            var completeAfkStand = afkRelease.AddTransition(standRelease);
            completeAfkStand.AddCondition(AnimatorConditionMode.IfNot, 0, "Seated");
            var completeAfkSit = afkRelease.AddTransition(sitRelease);
            completeAfkSit.AddCondition(AnimatorConditionMode.If, 0, "Seated");

            var y = 1;
            foreach (var actionEmote in ActionWizard.actionEmotes)
            {
                PopulateEmoteFlow(actionEmote, false, "VRCEmote", y, stand, standRelease);
                y++;
            }

            y = afkY + 1;
            foreach (var afkEmote in ActionWizard.afkEmotes)
            {
                PopulateEmoteFlow(afkEmote, true, "VRCEmote", y, afk, afkRelease);
                y++;
            }

        }

        void PopulateEmoteFlow(ActionEmote actionEmote, bool isAfk, string parameter, int y, AnimatorState entry, AnimatorState release)
        {
            actionEmote.clip.SetLoopTimeRec(true);

            var clipMain = AddState(actionEmote.name, Position(4, y), actionEmote.clip);
            AnimatorStateTransition entryTransition;
            if (actionEmote.entryClip)
            {
                var clipEntry = AddState($"Entry {actionEmote.name}", Position(3, y), actionEmote.entryClip);
                entryTransition = entry.AddTransition(clipEntry);
                var exitEntryTransition = clipEntry.AddTransition(clipMain);
                exitEntryTransition.hasExitTime = true;
                exitEntryTransition.exitTime = actionEmote.entryClipExitTime;
                exitEntryTransition.hasFixedDuration = ActionWizard.fixedTransitionDuration;
                exitEntryTransition.duration = actionEmote.postEntryTransitionDuration;
                PopulatePlayableLayerControl(clipEntry, VRC_PlayableLayerControl.BlendableLayer.Action, 1f, 0.5f);
            }
            else
            {
                entryTransition = entry.AddTransition(clipMain);
                PopulatePlayableLayerControl(clipMain, VRC_PlayableLayerControl.BlendableLayer.Action, 1f, 0.5f);
            }
            entryTransition.AddCondition(AnimatorConditionMode.Equals, actionEmote.emoteIndex, parameter);
            entryTransition.hasFixedDuration = ActionWizard.fixedTransitionDuration;
            entryTransition.duration = actionEmote.entryTransitionDuration;

            AnimatorStateTransition exitTransition;
            if (actionEmote.exitClip)
            {
                var clipExit = AddState($"Exit {actionEmote.name}", Position(5, y), actionEmote.exitClip);
                exitTransition = clipMain.AddTransition(clipExit);
                var exitExitTransition = clipExit.AddTransition(release);
                exitExitTransition.hasExitTime = true;
                exitExitTransition.exitTime = actionEmote.exitClipExitTime;
                exitExitTransition.hasFixedDuration = ActionWizard.fixedTransitionDuration;
                exitExitTransition.duration = actionEmote.postExitTransitionDuration;
            }
            else
            {
                exitTransition = clipMain.AddTransition(release);
            }

            if (isAfk)
            {
                exitTransition.AddCondition(AnimatorConditionMode.IfNot, actionEmote.emoteIndex, "AFK");
            }
            else
            {
                exitTransition.AddCondition(AnimatorConditionMode.NotEqual, actionEmote.emoteIndex, parameter);
            }
            exitTransition.hasFixedDuration = ActionWizard.fixedTransitionDuration;
            exitTransition.duration = actionEmote.exitTransitionDuration;
        }
        
        static void PopulateTrackingControl(AnimatorState state, VRC_AnimatorTrackingControl.TrackingType value)
        {
            var trackingControl = state.AddStateMachineBehaviour<VRCAnimatorTrackingControl>();
            // TODO
            trackingControl.trackingHead = value; 
            trackingControl.trackingLeftHand = value; 
            trackingControl.trackingRightHand = value; 
            trackingControl.trackingHip = value; 
            trackingControl.trackingLeftFoot = value; 
            trackingControl.trackingRightFoot = value; 
            trackingControl.trackingLeftFingers = value; 
            trackingControl.trackingRightFingers = value; 
        }

        static void PopulatePlayableLayerControl(AnimatorState state, VRC_PlayableLayerControl.BlendableLayer layer, float goalWeight, float duration)
        {
            var playableLayerControl = state.AddStateMachineBehaviour<VRCPlayableLayerControl>();
            playableLayerControl.layer = layer;
            playableLayerControl.goalWeight = goalWeight;
            playableLayerControl.blendDuration = duration;
        }

        AnimatorState AddState(string stateName, Vector3 position, Motion motion)
        {
            var state = StateMachine.AddState(stateName, position);
            state.writeDefaultValues = false;
            state.motion = motion;
            return state;
        }

        public void BuildParameters()
        {
            AnimatorController.AddParameter("VRCEmote", AnimatorControllerParameterType.Int);
            AnimatorController.AddParameter("AFK", AnimatorControllerParameterType.Bool);
            AnimatorController.AddParameter("Seated", AnimatorControllerParameterType.Bool);
        }
    }
}