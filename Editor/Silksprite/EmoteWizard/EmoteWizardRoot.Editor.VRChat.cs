#if EW_VRCSDK3_AVATARS

using System;
using Silksprite.EmoteWizard.Configs;
using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.Contexts.Extensions;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Platforms.Utils;
using Silksprite.EmoteWizard.Platforms.VRChat.Contexts.Builder;
using Silksprite.EmoteWizard.Platforms.VRChat.Extensions;
using Silksprite.EmoteWizardSupport.UI;
using Silksprite.EmoteWizardSupport.Undoable;
using Silksprite.Loch.IMGUI;
using UnityEditor;
using UnityEngine;
using VRC.SDK3.Avatars.Components;
using static Silksprite.Loch.Tools.LochTool;

namespace Silksprite.EmoteWizard
{
    public partial class EmoteWizardRootEditor
    {
        [InitializeOnLoadMethod]
        static void InitializeOnLoadVRChat()
        {
            RegisterPlatformUIs += editor =>
            {
                editor.PlatformLayerOptionsUI += editor.LayerOptionsVRChat;
                editor.PlatformOutputOptionsUI += editor.AvatarOutputVRChat;
            };
        }
        
        void LayerOptionsVRChat(EmoteWizardEnvironment env)
        {
            if (!env.IsVRChatAvatar())
            {
                return;
            }

            HeadingOnce(Loc("EmoteWizardRoot::Options"));

            LEditorGUILayout.PropAsEnumPopup<LayerKind>(_generateTrackingControlLayer);

            var avatarDescriptor = env.AvatarRoot.GetComponent<VRCAvatarDescriptor>();

            LEditorGUILayout.PropAsEnumPopup<OverrideGeneratedControllerType2>(_overrideGesture);
            using (new EditorGUI.IndentLevelScope())
            {
                switch (env.OverrideGesture)
                {
                    case OverrideGeneratedControllerType2.Generate:
                        break;
                    case OverrideGeneratedControllerType2.Override:
                        LEditorGUILayout.Prop(_overrideGestureController);
                        break;
                    case OverrideGeneratedControllerType2.Default1:
                        DummyController(_overrideGestureController, VrcSdkAssetLocator.HandsLayerController1());
                        break;
                    case OverrideGeneratedControllerType2.Default2:
                        DummyController(_overrideGestureController, VrcSdkAssetLocator.HandsLayerController2());
                        break;
                    case OverrideGeneratedControllerType2.Inherit:
                        DummyController(_overrideGestureController, avatarDescriptor.FindAnimationLayer(VRCAvatarDescriptor.AnimLayerType.Gesture));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            LEditorGUILayout.PropAsEnumPopup<OverrideGeneratedControllerType1>(_overrideAction);
            using (new EditorGUI.IndentLevelScope())
            {
                switch (env.OverrideAction)
                {
                    case OverrideGeneratedControllerType1.Generate:
                        break;
                    case OverrideGeneratedControllerType1.Override:
                        LEditorGUILayout.Prop(_overrideActionController);
                        break;
                    case OverrideGeneratedControllerType1.Default:
                        DummyController(_overrideActionController, VrcSdkAssetLocator.ActionLayerController());
                        break;
                    case OverrideGeneratedControllerType1.Inherit:
                        DummyController(_overrideActionController, avatarDescriptor.FindAnimationLayer(VRCAvatarDescriptor.AnimLayerType.Action));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            LEditorGUILayout.PropAsEnumPopup<OverrideControllerType2>(_overrideSitting);
            using (new EditorGUI.IndentLevelScope())
            {
                switch (env.OverrideSitting)
                {
                    case OverrideControllerType2.Override:
                        LEditorGUILayout.Prop(_overrideSittingController);
                        break;
                    case OverrideControllerType2.Default1:
                        DummyController(_overrideSittingController, VrcSdkAssetLocator.SittingLayerController1());
                        break;
                    case OverrideControllerType2.Default2:
                        DummyController(_overrideSittingController, VrcSdkAssetLocator.SittingLayerController2());
                        break;
                    case OverrideControllerType2.Inherit:
                        DummyController(_overrideSittingController, avatarDescriptor.FindAnimationLayer(VRCAvatarDescriptor.AnimLayerType.Sitting));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        void AvatarOutputVRChat(EmoteWizardEnvironment env)
        {
            if (!env.IsVRChatAvatar())
            {
                return;
            }

            HeadingOnce(Loc("EmoteWizardRoot::Avatar Output"));

            LEditorGUILayout.Prop(_proxyAnimator);
            var avatarDescriptor = env.AvatarRoot.GetComponent<VRCAvatarDescriptor>();
            if (avatarDescriptor)
            {
                EmoteWizardSupportGUILayout.OutputUIArea(true, null, () =>
                {
                    void EditAnimator(RuntimeAnimatorController? animatorController)
                    {
                        var animator = CachedEnv().ProvideProxyAnimator();
                        animator.runtimeAnimatorController = animatorController;
                        if (!animatorController) return;
                        Selection.SetActiveObjectWithContext(animator.gameObject, animatorController);
                    }

                    var gestureController = avatarDescriptor.FindAnimationLayer(VRCAvatarDescriptor.AnimLayerType.Gesture);
                    var fxController = avatarDescriptor.FindAnimationLayer(VRCAvatarDescriptor.AnimLayerType.FX);
                    var actionController = avatarDescriptor.FindAnimationLayer(VRCAvatarDescriptor.AnimLayerType.Action);
                    var editorController = env.GetContext<EditorLayerContext>().OutputAsset;

                    var avatarAnimator = RuntimeUndoable.Instance.EnsureComponent<Animator>(avatarDescriptor);
                    if (LGUILayout.Button(Loc("EmoteWizardRoot::Disconnect Avatar Output Assets")))
                    {
                        CachedEnv().GetContext<VRChatAvatarBuilderContext>().CleanupAvatar();
                    }
                    EmoteWizardSupportGUILayout.Undoable(Loc("EmoteWizardRoot::Generate Everything and Update Avatar"),
                        "Generate Everything and Update Avatar",
                        undoable =>
                        {
                            undoable.EnsureComponent<EditorLayerConfig>(soleTarget);
                            CachedEnv().GetContext<VRChatAvatarBuilderContext>().BuildAvatar(undoable, true);
                        });

                    using (new GUILayout.HorizontalScope())
                    {
                        using (new EditorGUI.DisabledScope(editorController == null))
                        {
                            if (LGUILayout.Button(Loc("EmoteWizardRoot::Edit")))
                            {
                                EditAnimator(editorController);
                            }
                        }

                        using (new EditorGUI.DisabledScope(gestureController == null || env.OverrideGesture == OverrideGeneratedControllerType2.Default1 || env.OverrideGesture == OverrideGeneratedControllerType2.Default2))
                        {
                            if (LGUILayout.Button(Loc("EmoteWizardRoot::Edit Gesture")))
                            {
                                EditAnimator(gestureController);
                            }
                        }

                        using (new EditorGUI.DisabledScope(fxController == null))
                        {
                            if (LGUILayout.Button(Loc("EmoteWizardRoot::Edit FX")))
                            {
                                EditAnimator(fxController);
                            }
                        }

                        using (new EditorGUI.DisabledScope(actionController == null || env.OverrideAction == OverrideGeneratedControllerType1.Default))
                        {
                            if (LGUILayout.Button(Loc("EmoteWizardRoot::Edit Action")))
                            {
                                EditAnimator(actionController);
                            }
                        }
                    }

                    if (LGUILayout.Button(Loc("EmoteWizardRoot::Remove Animator Controller")))
                    {
                        EditAnimator(null);
                    }

                    DummyController(_proxyAnimator, avatarAnimator.runtimeAnimatorController);

                    if (avatarAnimator.runtimeAnimatorController == null)
                    {
                        // do nothing
                    }
                    else if (avatarAnimator.runtimeAnimatorController == editorController)
                    {
                        LEditorGUILayout.HelpBox(Loc("EmoteWizardRoot::runtimeAnimatorController::editor."), MessageType.Warning);
                    }
                    else if (avatarAnimator.runtimeAnimatorController == gestureController)
                    {
                        LEditorGUILayout.HelpBox(Loc("EmoteWizardRoot::runtimeAnimatorController::gesture."), MessageType.Warning);
                    }
                    else if (avatarAnimator.runtimeAnimatorController == fxController)
                    {
                        LEditorGUILayout.HelpBox(Loc("EmoteWizardRoot::runtimeAnimatorController::fx."), MessageType.Warning);
                    }
                    else if (avatarAnimator.runtimeAnimatorController == actionController)
                    {
                        LEditorGUILayout.HelpBox(Loc("EmoteWizardRoot::runtimeAnimatorController::action."), MessageType.Warning);
                    }
                    else
                    {
                        LEditorGUILayout.HelpBox(Loc("EmoteWizardRoot::runtimeAnimatorController::unknown."), MessageType.Warning);
                    }
                });
            }
        }
    }
}

#endif
