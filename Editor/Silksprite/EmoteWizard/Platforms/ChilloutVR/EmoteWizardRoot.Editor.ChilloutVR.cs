using Silksprite.AdLib.ChilloutVR.Extensions;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.Configs;
using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.Contexts.Extensions;
using Silksprite.EmoteWizard.Platforms.ChilloutVR.Contexts.Builder;
using Silksprite.EmoteWizard.UI;
using Silksprite.EmoteWizardSupport.UI;
using Silksprite.EmoteWizardSupport.Undoable;
using Silksprite.Loch;
using Silksprite.Loch.IMGUI;
using UnityEditor;
using UnityEngine;
using static Silksprite.Loch.Tools.LochTool;

namespace Silksprite.EmoteWizard.Platforms.ChilloutVR
{
    public class EmoteWizardRootChilloutVREditor : EmoteWizardRootPlatformEditorBase<EmoteWizardRoot>
    {
        [InitializeOnLoadMethod]
        static void InitializeOnLoadChilloutVR()
        {
            EmoteWizardRootEditor.RegisterPlatformUIs += editor =>
            {
                var platformUI = new EmoteWizardRootChilloutVREditor(editor);
                editor.PlatformOutputOptionsUI += platformUI.AvatarOutputChilloutVR;
            };
        }

        readonly LocalizedProperty _proxyAnimator;

        EmoteWizardRootChilloutVREditor(EmoteWizardRootEditor editor) : base(editor)
        {
            _proxyAnimator = Lop(nameof(EmoteWizardRoot.proxyAnimator), Loc("EmoteWizardRoot::proxyAnimator"));
        }

        void AvatarOutputChilloutVR(EmoteWizardEnvironment env)
        {
            if (!env.IsChilloutVRAvatar())
            {
                return;
            }

            HeadingOnce(Loc("EmoteWizardRoot::Avatar Output"));

            LEditorGUILayout.Prop(_proxyAnimator);
            if (env.AvatarRoot.TryGetCVRAvatarAccess(out var cvrAvatar))
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

                    var editorController = env.GetContext<EditorLayerContext>().OutputAsset;

                    var avatarAnimator = RuntimeUndoable.Instance.EnsureComponent<Animator>(cvrAvatar.BaseObject);
                    if (LGUILayout.Button(Loc("EmoteWizardRoot::Disconnect Avatar Output Assets")))
                    {
                        CachedEnv().GetContext<ChilloutVRAvatarBuilderContext>().CleanupAvatar();
                    }
                    EmoteWizardSupportGUILayout.Undoable(Loc("EmoteWizardRoot::Generate Everything and Update Avatar"),
                        "Generate Everything and Update Avatar",
                        undoable =>
                        {
                            undoable.EnsureComponent<EditorLayerConfig>(soleTarget);
                            CachedEnv().GetContext<ChilloutVRAvatarBuilderContext>().BuildAvatar(undoable, true);
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
                    }

                    if (LGUILayout.Button(Loc("EmoteWizardRoot::Remove Animator Controller")))
                    {
                        EditAnimator(null);
                    }

                    EmoteWizardGUILayout.DummyController(_proxyAnimator, avatarAnimator.runtimeAnimatorController);

                    if (avatarAnimator.runtimeAnimatorController == null)
                    {
                        // do nothing
                    }
                    else if (avatarAnimator.runtimeAnimatorController == editorController)
                    {
                        LEditorGUILayout.HelpBox(Loc("EmoteWizardRoot::runtimeAnimatorController::editor."), MessageType.Warning);
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