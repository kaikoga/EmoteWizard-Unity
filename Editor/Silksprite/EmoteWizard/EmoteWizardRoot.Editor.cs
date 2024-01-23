using System;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.Configs;
using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.Contexts.Extensions;
using Silksprite.EmoteWizard.Extensions;
using Silksprite.EmoteWizard.UI;
using Silksprite.EmoteWizard.Utils;
using Silksprite.EmoteWizardSupport.Extensions;
using Silksprite.EmoteWizardSupport.L10n;
using Silksprite.EmoteWizardSupport.UI;
using Silksprite.EmoteWizardSupport.Undoable;
using UnityEditor;
using UnityEngine;
using VRC.SDK3.Avatars.Components;
using static Silksprite.EmoteWizardSupport.L10n.LocalizationTool;
using static Silksprite.EmoteWizardSupport.Tools.EmoteWizardEditorTools;

namespace Silksprite.EmoteWizard
{
    [CustomEditor(typeof(EmoteWizardRoot))]
    public class EmoteWizardRootEditor : EmoteWizardEditorBase<EmoteWizardRoot>
    {
        bool _isSetup;

        LocalizedProperty _avatarDescriptor;
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
        LocalizedProperty _showTutorial;

        void OnEnable()
        {
            _avatarDescriptor = Lop(nameof(EmoteWizardRoot.avatarDescriptor), Loc("EmoteWizardRoot::avatarDescriptor"));
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
            _showTutorial = Lop(nameof(EmoteWizardRoot.showTutorial), Loc("EmoteWizardRoot::showTutorial"));
        }

        protected override void OnInnerInspectorGUI()
        {
            var env = CreateEnv();
            EmoteWizardGUILayout.ConfigUIArea(() =>
            {
                LocalizationSettingGUI.LocalizationSelector();
                EmoteWizardGUILayout.Prop(_showTutorial);
            });

            if (EmoteWizardGUILayout.Undoable(Loc("EmoteWizardRoot::Add Empty Data Source")) is IUndoable undoable)
            {
                undoable.AddChildComponentAndSelect<EmoteWizardDataSourceFactory>(soleTarget, "New Source");
            }

            LocalizedContent loc = Loc("EmoteWizardRoot::Setup");
            _isSetup = EmoteWizardGUILayout.Foldout(_isSetup, loc);
            if (_isSetup)
            {
                if (SetupGUI.OnInspectorGUI(env)) return;
            }

            EmoteWizardGUILayout.Header(Loc("EmoteWizardRoot::Avatar"));
            EmoteWizardGUILayout.Prop(_avatarDescriptor);
            var avatarDescriptor = env.AvatarDescriptor;
            if (!avatarDescriptor)
            {
                EmoteWizardGUILayout.HelpBox(Loc("EmoteWizardRoot::AvatarDescriptor::notFound."), MessageType.Error);
            }
            else if (!_avatarDescriptor.Property.objectReferenceValue)
            {
                using (new EditorGUI.IndentLevelScope())
                using (new EditorGUI.DisabledScope(true))
                {
                    EmoteWizardGUILayout.ObjectField(Loc("EmoteWizardRoot::Detected Avatar Descriptor"), env.AvatarDescriptor, true);
                }
            }

            EmoteWizardGUILayout.Header(Loc("EmoteWizardRoot::Assets Generation"));
            EditorGUI.BeginChangeCheck();
            EmoteWizardGUILayout.PropertyFoldout(_persistGeneratedAssets, () =>
            {
                using (new GUILayout.HorizontalScope())
                {
                    EmoteWizardGUILayout.Prop(_generatedAssetRoot);
                    if (EmoteWizardGUILayout.Button(Loc("EmoteWizardRoot::Browse...")))
                    {
                        SelectFolder(Loc("EmoteWizardRoot::Select Generated Assets Root"), _generatedAssetRoot.Property);
                    }
                }
                EmoteWizardGUILayout.Prop(_generatedAssetPrefix);

                EmoteWizardGUILayout.OutputUIArea(env.PersistGeneratedAssets, () =>
                {
                    EmoteWizardGUILayout.PropertyFieldWithGenerate(_emptyClip, () => CreateEnv().ProvideEmptyClip());
                });
                if (EmoteWizardGUILayout.Button(Loc("EmoteWizardRoot::Disconnect Output Assets")))
                {
                    CreateEnv().DisconnectAllOutputAssets();
                }
            });
            if (EditorGUI.EndChangeCheck() && !_persistGeneratedAssets.Property.boolValue)
            {
                env.DisconnectAllOutputAssets();
            }

            EmoteWizardGUILayout.Header(Loc("EmoteWizardRoot::Options"));
            EmoteWizardGUILayout.Prop(_generateTrackingControlLayer);

            void DummyController(LocalizedProperty lop, RuntimeAnimatorController dummyController)
            {
                using (new EditorGUI.DisabledScope(true))
                {
                    EmoteWizardGUILayout.ObjectField(lop.Loc, dummyController, false);
                }
            }

            EmoteWizardGUILayout.Prop(_overrideGesture);
            using (new EditorGUI.IndentLevelScope())
            {
                switch (env.OverrideGesture)
                {
                    case OverrideGeneratedControllerType2.Generate:
                        break;
                    case OverrideGeneratedControllerType2.Override:
                        EmoteWizardGUILayout.Prop(_overrideGestureController);
                        break;
                    case OverrideGeneratedControllerType2.Default1:
                        DummyController(_overrideGestureController, VrcSdkAssetLocator.HandsLayerController1());
                        break;
                    case OverrideGeneratedControllerType2.Default2:
                        DummyController(_overrideGestureController, VrcSdkAssetLocator.HandsLayerController2());
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            EmoteWizardGUILayout.Prop(_overrideAction);
            using (new EditorGUI.IndentLevelScope())
            {
                switch (env.OverrideAction)
                {
                    case OverrideGeneratedControllerType1.Generate:
                        break;
                    case OverrideGeneratedControllerType1.Override:
                        EmoteWizardGUILayout.Prop(_overrideActionController);
                        break;
                    case OverrideGeneratedControllerType1.Default:
                        DummyController(_overrideActionController, VrcSdkAssetLocator.ActionLayerController());
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            EmoteWizardGUILayout.Prop(_overrideSitting);
            using (new EditorGUI.IndentLevelScope())
            {
                switch (env.OverrideSitting)
                {
                    case OverrideControllerType2.Override:
                        EmoteWizardGUILayout.Prop(_overrideSittingController);
                        break;
                    case OverrideControllerType2.Default1:
                        DummyController(_overrideSittingController, VrcSdkAssetLocator.SittingLayerController1());
                        break;
                    case OverrideControllerType2.Default2:
                        DummyController(_overrideSittingController, VrcSdkAssetLocator.SittingLayerController2());
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            EmoteWizardGUILayout.Header(Loc("EmoteWizardRoot::Avatar Output"));

            EmoteWizardGUILayout.Prop(_proxyAnimator);

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

                    var avatarAnimator = RuntimeUndoable.Instance.EnsureComponent<Animator>(env.AvatarDescriptor);
                    if (EmoteWizardGUILayout.Button(Loc("EmoteWizardRoot::Disconnect Avatar Output Assets")))
                    {
                        CreateEnv().CleanupAvatar();
                    }
                    if (EmoteWizardGUILayout.Button(Loc("EmoteWizardRoot::Generate Everything and Update Avatar")))
                    {
                        var undoable = new EditorUndoable("Generate Everything and Update Avatar");
                        undoable.EnsureComponent<EditorLayerConfig>(soleTarget);
                        CreateEnv().BuildAvatar(undoable, true);
                    }

                    using (new GUILayout.HorizontalScope())
                    {
                        using (new EditorGUI.DisabledScope(editorController == null))
                        {
                            if (EmoteWizardGUILayout.Button(Loc("EmoteWizardRoot::Edit")))
                            {
                                EditAnimator(editorController);
                            }
                        }

                        using (new EditorGUI.DisabledScope(gestureController == null || env.OverrideGesture == OverrideGeneratedControllerType2.Default1 || env.OverrideGesture == OverrideGeneratedControllerType2.Default2))
                        {
                            if (EmoteWizardGUILayout.Button(Loc("EmoteWizardRoot::Edit Gesture")))
                            {
                                EditAnimator(gestureController);
                            }
                        }

                        using (new EditorGUI.DisabledScope(fxController == null))
                        {
                            if (EmoteWizardGUILayout.Button(Loc("EmoteWizardRoot::Edit FX")))
                            {
                                EditAnimator(fxController);
                            }
                        }

                        using (new EditorGUI.DisabledScope(actionController == null || env.OverrideAction == OverrideGeneratedControllerType1.Default))
                        {
                            if (EmoteWizardGUILayout.Button(Loc("EmoteWizardRoot::Edit Action")))
                            {
                                EditAnimator(actionController);
                            }
                        }
                    }

                    if (EmoteWizardGUILayout.Button(Loc("EmoteWizardRoot::Remove Animator Controller")))
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
                        EmoteWizardGUILayout.HelpBox(Loc("EmoteWizardRoot::runtimeAnimatorController::editor."), MessageType.Warning);
                    }
                    else if (avatarAnimator.runtimeAnimatorController == gestureController)
                    {
                        EmoteWizardGUILayout.HelpBox(Loc("EmoteWizardRoot::runtimeAnimatorController::gesture."), MessageType.Warning);
                    }
                    else if (avatarAnimator.runtimeAnimatorController == fxController)
                    {
                        EmoteWizardGUILayout.HelpBox(Loc("EmoteWizardRoot::runtimeAnimatorController::fx."), MessageType.Warning);
                    }
                    else if (avatarAnimator.runtimeAnimatorController == actionController)
                    {
                        EmoteWizardGUILayout.HelpBox(Loc("EmoteWizardRoot::runtimeAnimatorController::action."), MessageType.Warning);
                    }
                    else
                    {
                        EmoteWizardGUILayout.HelpBox(Loc("EmoteWizardRoot::runtimeAnimatorController::unknown."), MessageType.Warning);
                    }
                });
            }
 
            serializedObject.ApplyModifiedProperties();
        }
    }
}