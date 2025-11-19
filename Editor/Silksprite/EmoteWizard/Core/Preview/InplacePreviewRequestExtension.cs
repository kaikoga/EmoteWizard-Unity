using Silksprite.EmoteWizard.Preview.Core;
using Silksprite.EmoteWizardSupport.UI;
using UnityEditor;

using static Silksprite.EmoteWizardSupport.L10n.LocalizationTool;

namespace Silksprite.EmoteWizard.Preview
{
    public static class InplacePreviewRequestExtension
    {
        public static void OnInspectorGUI(this InplacePreviewRequest request)
        {
            if (request.IsCurrentPreview)
            {
                EmoteWizardGUILayout.HelpBox(Loc("AnimationPreview::Active."), MessageType.Info);
            }
            else
            {
                EmoteWizardGUILayout.HelpBox(Loc("AnimationPreview::Blocked."), MessageType.Warning);
            }
        }
    }
}