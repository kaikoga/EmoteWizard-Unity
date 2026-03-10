#if ATIV_DETECTED_VRM0 || ATIV_DETECTED_VRM1

using Silksprite.Loch.IMGUI;
using static Silksprite.Loch.Tools.LochTool;

namespace Silksprite.EmoteWizard
{
    public partial class EmoteWizardRootEditor
    {
        void ExportOptionsVrm()
        {
            HeaderOnce(Loc("EmoteWizardRoot::Options"));

            LEditorGUILayout.Prop(_author);
        }
    }
}

#endif
