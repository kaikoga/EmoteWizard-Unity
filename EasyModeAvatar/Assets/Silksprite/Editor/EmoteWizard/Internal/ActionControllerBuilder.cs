using System.Linq;
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

            StateMachine.defaultState = stand;
            var standToSit = stand.AddTransition(sit);
            standToSit.AddCondition(AnimatorConditionMode.If, 0, "Seated");
            var sitToStand = sit.AddTransition(stand);
            sitToStand.AddCondition(AnimatorConditionMode.IfNot, 0, "Seated");
            var standToAfk = stand.AddTransition(afk);
            standToAfk.AddCondition(AnimatorConditionMode.If, 0, "AFK");
            var sitToAfk = sit.AddTransition(afk);
            sitToAfk.AddCondition(AnimatorConditionMode.If, 0, "AFK");

            var y = 1;
            foreach (var actionEmote in ActionWizard.actionEmotes)
            {
                PopulateEmoteFlow(actionEmote, false, ActionWizard.actionSelectParameter, y, stand);
                y++;
            }

            y = afkY + 1;
            if (ActionWizard.afkSelectEnabled)
            {
                foreach (var afkEmote in ActionWizard.afkEmotes)
                {
                    PopulateEmoteFlow(afkEmote, true,  ActionWizard.afkSelectParameter, y++, afk);
                }
            }
            PopulateEmoteFlow(ActionWizard.defaultAfkEmote, true, null, y, afk);

        }

        void PopulateEmoteFlow(ActionEmote actionEmote, bool isAfk, string parameter, int y, AnimatorState entry)
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
                PopulateTrackingControl(clipEntry, VRC_AnimatorTrackingControl.TrackingType.Animation);
                PopulatePlayableLayerControl(clipEntry, 1f, actionEmote.blendIn);
            }
            else
            {
                entryTransition = entry.AddTransition(clipMain);
                PopulateTrackingControl(clipMain, VRC_AnimatorTrackingControl.TrackingType.Animation);
                PopulatePlayableLayerControl(clipMain, 1f, actionEmote.blendIn);
            }

            if (parameter == null)
            {
                entryTransition.AddAlwaysTrueCondition();
            }
            else
            {
                entryTransition.AddCondition(AnimatorConditionMode.Equals, actionEmote.emoteIndex, parameter);
            }
            entryTransition.hasFixedDuration = ActionWizard.fixedTransitionDuration;
            entryTransition.duration = actionEmote.entryTransitionDuration;

            var release = AddState($"Release {actionEmote.name}", Position(6, y), null);
            var releaseTransition = release.AddExitTransition();
            PopulateTrackingControl(release, VRC_AnimatorTrackingControl.TrackingType.Tracking);
            PopulatePlayableLayerControl(release, 0f, actionEmote.blendOut);
            releaseTransition.AddAlwaysTrueCondition();

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
            else if (actionEmote.hasExitTime)
            {
                exitTransition.hasExitTime = true;
                exitTransition.exitTime = actionEmote.clipExitTime;
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
            // FIXME: customize?
            trackingControl.trackingHead = value; 
            trackingControl.trackingLeftHand = value; 
            trackingControl.trackingRightHand = value; 
            trackingControl.trackingHip = value; 
            trackingControl.trackingLeftFoot = value; 
            trackingControl.trackingRightFoot = value; 
            trackingControl.trackingLeftFingers = value; 
            trackingControl.trackingRightFingers = value; 
        }

        static void PopulatePlayableLayerControl(AnimatorState state, float goalWeight, float duration)
        {
            var playableLayerControl = state.AddStateMachineBehaviour<VRCPlayableLayerControl>();
            playableLayerControl.layer = VRC_PlayableLayerControl.BlendableLayer.Action;
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
            AnimatorController.AddParameter(ActionWizard.actionSelectParameter, AnimatorControllerParameterType.Int);
            AnimatorController.AddParameter("AFK", AnimatorControllerParameterType.Bool);
            AnimatorController.AddParameter("Seated", AnimatorControllerParameterType.Bool);

            AnimatorController.AddParameter("Viseme", AnimatorControllerParameterType.Int); // dummy for AlwaysTrueTransition
            if (ActionWizard.SelectableAfkEmotes)
            {
                AnimatorController.AddParameter(ActionWizard.afkSelectParameter, AnimatorControllerParameterType.Int);
            }
        }
    }
}