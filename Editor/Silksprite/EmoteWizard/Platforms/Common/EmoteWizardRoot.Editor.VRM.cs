using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.Contexts;
using Silksprite.Loch;
using Silksprite.Loch.IMGUI;
using UnityEditor;
using static Silksprite.Loch.Tools.LochTool;

namespace Silksprite.EmoteWizard.Platforms.Common
{
    public class EmoteWizardRootVRMEditor : EmoteWizardRootPlatformEditorBase<EmoteWizardRoot>
    {
        [InitializeOnLoadMethod]
        static void InitializeOnLoadVRM()
        {
            EmoteWizardRootEditor.RegisterPlatformUIs += editor =>
            {
                var platformUI = new EmoteWizardRootVRMEditor(editor);
                editor.PlatformExportOptionsUI += platformUI.ExportOptionsVRM;
            };
        }

        readonly LocalizedProperty _author;

        EmoteWizardRootVRMEditor(EmoteWizardRootEditor editor) : base(editor)
        {
            _author = Lop(nameof(EmoteWizardRoot.author), Loc("EmoteWizardRoot::author"));
        }

        void ExportOptionsVRM(EmoteWizardEnvironment environment)
        {
            if (!environment.MaybeVRM())
            {
                return;
            }

            HeadingOnce(Loc("EmoteWizardRoot::Options"));

            LEditorGUILayout.Prop(_author);
        }
    }
}
