#if ATIV_DETECTED_VRM0 || ATIV_DETECTED_VRM1

using Silksprite.EmoteWizard.Contexts;
using Silksprite.Loch.IMGUI;
using UnityEditor;
using static Silksprite.Loch.Tools.LochTool;

namespace Silksprite.EmoteWizard
{
    public partial class EmoteWizardRootEditor
    {
        [InitializeOnLoadMethod]
        static void InitializeOnLoadVRM()
        {
            RegisterPlatformUIs += editor =>
            {
                editor.PlatformExportOptionsUI += editor.ExportOptionsVRM;
            };
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

#endif
