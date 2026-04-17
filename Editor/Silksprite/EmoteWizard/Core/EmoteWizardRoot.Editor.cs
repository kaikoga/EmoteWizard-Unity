using System;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.Contexts.Extensions;
using Silksprite.EmoteWizard.DataObjects;
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
    public class EmoteWizardRootEditor : EmoteWizardEditorBase<EmoteWizardRoot>
    {
        // NOTE: EmoteWizardRoot Editor is not in Core asmdef because Platform extensions 
        bool _isSetup;

        // ReSharper disable NotAccessedField.Local
        LocalizedProperty _avatarRootTransform = null!;
        LocalizedProperty _proxyAnimator = null!;
        LocalizedProperty _persistGeneratedAssets = null!;
        LocalizedProperty _generatedAssetRoot = null!;
        LocalizedProperty _generatedAssetPrefix = null!;
        LocalizedProperty _emptyClip = null!;
        LocalizedProperty _generateTrackingControlLayer = null!;
        LocalizedProperty _overrideGesture = null!;
        LocalizedProperty _overrideGestureController = null!;
        LocalizedProperty _overrideAction = null!;
        LocalizedProperty _overrideActionController = null!;
        LocalizedProperty _overrideSitting = null!;
        LocalizedProperty _overrideSittingController = null!;
        LocalizedProperty _author = null!;
        LocalizedProperty _showTutorial = null!;
        LocalizedProperty _detectPlatform = null!;
        LocalizedProperty _parameterScheme = null!;
        // ReSharper restore NotAccessedField.Local

        public static event Action<EmoteWizardRootEditor>? RegisterPlatformUIs;
        public event Action<EmoteWizardEnvironment>? PlatformLayerOptionsUI;
        public event Action<EmoteWizardEnvironment>? PlatformExportOptionsUI;
        public event Action<EmoteWizardEnvironment>? PlatformOutputOptionsUI;

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
            _parameterScheme = Lop(nameof(EmoteWizardRoot.parameterScheme), Loc("EmoteWizardRoot::sourcePlatform"));
            
            RegisterPlatformUIs?.Invoke(this);
        }

        protected override void OnInnerInspectorGUI()
        {
            var env = CachedEnv();
            EmoteWizardSupportGUILayout.ConfigUIArea(() =>
            {
                LEditorGUILayout.LocaleSelector();
                LEditorGUILayout.Prop(_showTutorial);
                LEditorGUILayout.Prop(_detectPlatform);
#if CVR_CCK_EXISTS || ADLIB_CVR_CCK_STUBBED
                LEditorGUILayout.PropAsEnumPopup<ParameterScheme>(_parameterScheme);
#endif
            });

            EmoteWizardSupportGUILayout.Undoable(Loc("EmoteWizardRoot::Add Empty Data Source"), undoable =>
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

                EmoteWizardSupportGUILayout.OutputUIArea(env.PersistGeneratedAssets, () => { EmoteWizardSupportGUILayout.PropWithGenerate(_emptyClip, () => CachedEnv().ProvideEmptyClip()); });
                if (LGUILayout.Button(Loc("EmoteWizardRoot::Disconnect Output Assets"), new GUILayoutOption[0]))
                {
                    CachedEnv().DisconnectAllOutputAssets();
                }
            });
            if (EditorGUI.EndChangeCheck() && !_persistGeneratedAssets.Property.boolValue)
            {
                env.DisconnectAllOutputAssets();
            }

            PlatformLayerOptionsUI?.Invoke(env);
            PlatformExportOptionsUI?.Invoke(env);
            PlatformOutputOptionsUI?.Invoke(env);
 
            serializedObject.ApplyModifiedProperties();
        }

    }
}