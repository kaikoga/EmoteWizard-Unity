using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.Contexts.Extensions;
using Silksprite.EmoteWizard.UI;
using Silksprite.EmoteWizardSupport.UI;
using Silksprite.EmoteWizardSupport.Undoable;
using Silksprite.Loch;
using Silksprite.Loch.IMGUI;
using UnityEditor;
using UnityEngine;
using static Silksprite.Loch.Tools.LochTool;
using static Silksprite.EmoteWizardSupport.Tools.EmoteWizardEditorTools;

namespace Silksprite.EmoteWizard
{
    [CustomEditor(typeof(EmoteWizardRoot))]
    public partial class EmoteWizardRootEditor : EmoteWizardEditorBase<EmoteWizardRoot>
    {
        // NOTE: EmoteWizardRoot Editor is not in Core asmdef because Platform extensions 
        bool _isSetup;

        // ReSharper disable NotAccessedField.Local
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
        // ReSharper restore NotAccessedField.Local

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
            LEditorGUILayout.PropAsFoldout(_persistGeneratedAssets, () =>
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
            });
            if (EditorGUI.EndChangeCheck() && !_persistGeneratedAssets.Property.boolValue)
            {
                env.DisconnectAllOutputAssets();
            }

#if EW_VRCSDK3_AVATARS
            if (env.IsVRChatAvatar())
            {
                LayerOptionsVrc(env);
            }
#endif

#if ATIV_DETECTED_VRM0 || ATIV_DETECTED_VRM1
            if (env.MaybeVRM())
            {
                ExportOptionsVrm();
            }
#endif

#if EW_VRCSDK3_AVATARS
            if (env.IsVRChatAvatar())
            {
                AvatarOutputVrc(env);
            }
#endif
 
#if CVR_CCK_EXISTS
            if (env.IsChilloutVRAvatar())
            {
                AvatarOutputCvr(env);
            }
#endif
 
            serializedObject.ApplyModifiedProperties();
        }
    }
}