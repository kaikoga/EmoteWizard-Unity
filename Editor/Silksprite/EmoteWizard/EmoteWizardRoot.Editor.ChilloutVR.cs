#if CVR_CCK_EXISTS || ADLIB_CVR_CCK_STUBBED

using Silksprite.AdLib.ChilloutVR.Extensions;
using Silksprite.EmoteWizard.Configs;
using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.Contexts.Extensions;
using Silksprite.EmoteWizard.Platforms.ChilloutVR.Contexts.Extensions;
using Silksprite.EmoteWizardSupport.UI;
using Silksprite.EmoteWizardSupport.Undoable;
using Silksprite.Loch;
using Silksprite.Loch.IMGUI;
using UnityEditor;
using UnityEngine;
using static Silksprite.Loch.Tools.LochTool;

namespace Silksprite.EmoteWizard
{
    public partial class EmoteWizardRootEditor
    {
        void AvatarOutputCvr(EmoteWizardEnvironment env)
        {
            LGUILayout.Heading(Loc("EmoteWizardRoot::Avatar Output"));

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
                        CachedEnv().CleanupCvrAvatar();
                    }
                    EmoteWizardSupportGUILayout.Undoable(Loc("EmoteWizardRoot::Generate Everything and Update Avatar"),
                        "Generate Everything and Update Avatar",
                        undoable =>
                        {
                            undoable.EnsureComponent<EditorLayerConfig>(soleTarget);
                            CachedEnv().BuildCvrAvatar(undoable, true);
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

                    DummyController(_proxyAnimator, avatarAnimator.runtimeAnimatorController);

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

#endif
