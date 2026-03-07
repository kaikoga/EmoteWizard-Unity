using System;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.Configs;
using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.Contexts.Extensions;
using Silksprite.EmoteWizard.UI;
using Silksprite.EmoteWizard.Utils;
using Silksprite.EmoteWizardSupport.UI;
using Silksprite.EmoteWizardSupport.Undoable;
using Silksprite.Loch;
using Silksprite.Loch.IMGUI;
using UnityEditor;
using UnityEngine;
using static Silksprite.Loch.Tools.LochTool;
using static Silksprite.EmoteWizardSupport.Tools.EmoteWizardEditorTools;

#if EW_VRCSDK3_AVATARS
using Silksprite.EmoteWizard.Platforms.VRChat.Contexts.Extensions;
using Silksprite.EmoteWizard.Platforms.VRChat.Extensions;
using VRC.SDK3.Avatars.Components;
#endif

namespace Silksprite.EmoteWizard
{
    [CustomEditor(typeof(EmoteWizardRoot))]
    public class EmoteWizardRootEditor : EmoteWizardEditorBase<EmoteWizardRoot>
    {
        bool _isSetup;

        LocalizedProperty _avatarRootTransform;
        LocalizedProperty _proxyAnimator;
        LocalizedProperty _persistGeneratedAssets;
        LocalizedProperty _generatedAssetRoot;
        LocalizedProperty _generatedAssetPrefix;
        LocalizedProperty _emptyClip;
        LocalizedProperty _generateTrackingControlLayer;
        LocalizedProperty _overrideGesture;
        LocalizedProperty _overrideGestureController;
        LocalizedProperty _overrideAction;
        LocalizedProperty _overrideActionController;
        LocalizedProperty _overrideSitting;
        LocalizedProperty _overrideSittingController;
        LocalizedProperty _author;
        LocalizedProperty _showTutorial;
        LocalizedProperty _detectPlatform;

        void OnEnable()
        {
            _avatarRootTransform = Lop(nameof(EmoteWizardRoot.avatarRootTransform), Loc("EmoteWizardRoot::avatarRootTransform"));
            _proxyAnimator = Lop(nameof(EmoteWizardRoot.proxyAnimator), Loc("EmoteWizardRoot::proxyAnimator"));
            _persistGeneratedAssets = Lop(nameof(EmoteWizardRoot.persistGeneratedAssets), Loc("EmoteWizardRoot::persistGeneratedAssets"));
            _generatedAssetRoot = Lop(nameof(EmoteWizardRoot.generatedAssetRoot), Loc("EmoteWizardRoot::generatedAssetRoot"));
            _generatedAssetPrefix = Lop(nameof(EmoteWizardRoot.generatedAssetPrefix), Loc("EmoteWizardRoot::generatedAssetPrefix"));
            _emptyClip = Lop(nameof(EmoteWizardRoot.emptyClip), Loc("EmoteWizardRoot::emptyClip"));
            _generateTrackingControlLayer = Lop(nameof(EmoteWizardRoot.generateTrackingControlLayer), Loc("EmoteWizardRoot::generateTrackingControlLayer"));
            _overrideGesture = Lop(nameof(EmoteWizardRoot.overrideGesture), Loc("EmoteWizardRoot::overrideGesture"));
            _overrideGestureController = Lop(nameof(EmoteWizardRoot.overrideGestureController), Loc("EmoteWizardRoot::overrideGestureController"));
            _overrideAction = Lop(nameof(EmoteWizardRoot.overrideAction), Loc("EmoteWizardRoot::overrideAction"));
            _overrideActionController = Lop(nameof(EmoteWizardRoot.overrideActionController), Loc("EmoteWizardRoot::overrideActionController"));
            _overrideSitting = Lop(nameof(EmoteWizardRoot.overrideSitting), Loc("EmoteWizardRoot::overrideSitting"));
            _overrideSittingController = Lop(nameof(EmoteWizardRoot.overrideSittingController), Loc("EmoteWizardRoot::overrideSittingController"));
            _author = Lop(nameof(EmoteWizardRoot.author), Loc("EmoteWizardRoot::author"));
            _showTutorial = Lop(nameof(EmoteWizardRoot.showTutorial), Loc("EmoteWizardRoot::showTutorial"));
            _detectPlatform = Lop(nameof(EmoteWizardRoot.detectPlatform), Loc("EmoteWizardRoot::detectPlatform"));
        }

        protected override void OnInnerInspectorGUI()
        {
            var env = CreateEnv();
            EmoteWizardGUILayout.ConfigUIArea(() =>
            {
                LEditorGUILayout.LocaleSelector();
                LEditorGUILayout.Prop(_showTutorial);
                LEditorGUILayout.Prop(_detectPlatform);
            });

            EmoteWizardGUILayout.Undoable(Loc("EmoteWizardRoot::Add Empty Data Source"), undoable =>
            {
                undoable.AddChildComponentAndSelect<EmoteWizardDataSourceFactory>(soleTarget, "New Source");
            });

            LocalizedContent loc = Loc("EmoteWizardRoot::Setup");
            _isSetup = LEditorGUILayout.Foldout(_isSetup, loc);
            if (_isSetup)
            {
                if (SetupGUI.OnInspectorGUI(env)) return;
            }

            LGUILayout.Heading(Loc("EmoteWizardRoot::Avatar"));
            LEditorGUILayout.Prop(_avatarRootTransform);

            var avatarRoot = env.AvatarRoot;
            if (!avatarRoot)
            {
                LEditorGUILayout.HelpBox(Loc("EmoteWizardRoot::AvatarRoot::notFound."), MessageType.Error);
            }
            else if (env.IsDetectedAvatarRoot)
            {
                using (new EditorGUI.IndentLevelScope())
                using (new EditorGUI.DisabledScope(true))
                {
                    LEditorGUILayout.ObjectField(Loc("EmoteWizardRoot::Detected Avatar Root"), env.AvatarRoot, true);
                }
            }

            LGUILayout.Heading(Loc("EmoteWizardRoot::Assets Generation"));
            EditorGUI.BeginChangeCheck();
            Action content = () =>
            {
                using (new GUILayout.HorizontalScope())
                {
                    LEditorGUILayout.Prop(_generatedAssetRoot);
                    if (LGUILayout.Button(Loc("EmoteWizardRoot::Browse..."), new GUILayoutOption[0]))
                    {
                        SelectFolder(Loc("EmoteWizardRoot::Select Generated Assets Root"), _generatedAssetRoot.Property);
                    }
                }

                LEditorGUILayout.Prop(_generatedAssetPrefix);

                EmoteWizardGUILayout.OutputUIArea(env.PersistGeneratedAssets, () => { EmoteWizardGUILayout.PropWithGenerate(_emptyClip, () => CreateEnv().ProvideEmptyClip()); });
                if (LGUILayout.Button(Loc("EmoteWizardRoot::Disconnect Output Assets"), new GUILayoutOption[0]))
                {
                    CreateEnv().DisconnectAllOutputAssets();
                }
            };
            LEditorGUILayout.PropAsFoldout(_persistGeneratedAssets, content);
            if (EditorGUI.EndChangeCheck() && !_persistGeneratedAssets.Property.boolValue)
            {
                env.DisconnectAllOutputAssets();
            }

#if EW_VRCSDK3_AVATARS
            if (env.IsVRChatAvatar())
            {
                HeaderOnce(Loc("EmoteWizardRoot::Options"));
                LEditorGUILayout.Prop(_generateTrackingControlLayer);

                var avatarDescriptor = env.AvatarRoot.GetComponent<VRCAvatarDescriptor>();

                LEditorGUILayout.Prop(_overrideGesture);
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

                LEditorGUILayout.Prop(_overrideAction);
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

                LEditorGUILayout.Prop(_overrideSitting);
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
#endif

#if ATIV_DETECTED_VRM0 || ATIV_DETECTED_VRM1
            if (env.MaybeVRM())
            {
                HeaderOnce(Loc("EmoteWizardRoot::Options"));
                LEditorGUILayout.Prop(_author);
            }
#endif

#if EW_VRCSDK3_AVATARS
            if (env.IsVRChatAvatar())
            {
                LGUILayout.Heading(Loc("EmoteWizardRoot::Avatar Output"));
                AvatarOutputVrc(env);
            }
#endif
 
            serializedObject.ApplyModifiedProperties();
        }

        void DummyController(LocalizedProperty lop, RuntimeAnimatorController dummyController)
        {
            using (new EditorGUI.DisabledScope(true))
            {
                LEditorGUILayout.ObjectField(lop.Loc, dummyController, false);
            }
        }

#if EW_VRCSDK3_AVATARS
        void AvatarOutputVrc(EmoteWizardEnvironment env)
        {
            LEditorGUILayout.Prop(_proxyAnimator);
            var avatarDescriptor = env.AvatarRoot.GetComponent<VRCAvatarDescriptor>();
            if (avatarDescriptor)
            {
                EmoteWizardGUILayout.OutputUIArea(true, default, () =>
                {
                    void EditAnimator(RuntimeAnimatorController animatorController)
                    {
                        var animator = CreateEnv().ProvideProxyAnimator();
                        animator.runtimeAnimatorController = animatorController;
                        if (!animatorController) return;
                        Selection.SetActiveObjectWithContext(animator.gameObject, animatorController);
                    }

                    var gestureController = avatarDescriptor.FindAnimationLayer(VRCAvatarDescriptor.AnimLayerType.Gesture);
                    var fxController = avatarDescriptor.FindAnimationLayer(VRCAvatarDescriptor.AnimLayerType.FX);
                    var actionController = avatarDescriptor.FindAnimationLayer(VRCAvatarDescriptor.AnimLayerType.Action);
                    var editorController = env.GetContext<EditorLayerContext>().OutputAsset;

                    var avatarAnimator = RuntimeUndoable.Instance.EnsureComponent<Animator>(avatarDescriptor);
                    if (LGUILayout.Button(Loc("EmoteWizardRoot::Disconnect Avatar Output Assets"), new GUILayoutOption[0]))
                    {
                        CreateEnv().CleanupVrcAvatar();
                    }
                    EmoteWizardGUILayout.Undoable(Loc("EmoteWizardRoot::Generate Everything and Update Avatar"),
                        "Generate Everything and Update Avatar",
                        undoable =>
                        {
                            undoable.EnsureComponent<EditorLayerConfig>(soleTarget);
                            CreateEnv().BuildVrcAvatar(undoable, true);
                        });

                    using (new GUILayout.HorizontalScope())
                    {
                        using (new EditorGUI.DisabledScope(editorController == null))
                        {
                            if (LGUILayout.Button(Loc("EmoteWizardRoot::Edit"), new GUILayoutOption[0]))
                            {
                                EditAnimator(editorController);
                            }
                        }

                        using (new EditorGUI.DisabledScope(gestureController == null || env.OverrideGesture == OverrideGeneratedControllerType2.Default1 || env.OverrideGesture == OverrideGeneratedControllerType2.Default2))
                        {
                            if (LGUILayout.Button(Loc("EmoteWizardRoot::Edit Gesture"), new GUILayoutOption[0]))
                            {
                                EditAnimator(gestureController);
                            }
                        }

                        using (new EditorGUI.DisabledScope(fxController == null))
                        {
                            if (LGUILayout.Button(Loc("EmoteWizardRoot::Edit FX"), new GUILayoutOption[0]))
                            {
                                EditAnimator(fxController);
                            }
                        }

                        using (new EditorGUI.DisabledScope(actionController == null || env.OverrideAction == OverrideGeneratedControllerType1.Default))
                        {
                            if (LGUILayout.Button(Loc("EmoteWizardRoot::Edit Action"), new GUILayoutOption[0]))
                            {
                                EditAnimator(actionController);
                            }
                        }
                    }

                    if (LGUILayout.Button(Loc("EmoteWizardRoot::Remove Animator Controller"), new GUILayoutOption[0]))
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
#endif
    }
}