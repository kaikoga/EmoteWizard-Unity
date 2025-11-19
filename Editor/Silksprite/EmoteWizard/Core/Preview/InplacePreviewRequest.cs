using Silksprite.EmoteWizard.Preview.Presentation;
using Silksprite.EmoteWizardSupport.UI;
using UnityEditor;
using UnityEngine;
using static Silksprite.EmoteWizardSupport.L10n.LocalizationTool;

namespace Silksprite.EmoteWizard.Preview
{
    public class InplacePreviewRequest
    {
        public readonly GameObject OriginalAvatar;
        InplaceAnimationPreview _currentPreview;

        public bool IsCurrentPreview => InplacePreviewManager.Instance.CurrentPreviewRequest == this;
        public bool HasPreview => _currentPreview != null && OriginalAvatar;

        public InplacePreviewRequest(GameObject originalAvatar)
        {
            OriginalAvatar = originalAvatar;
        }

        public void OnInspectorGUI()
        {
            if (IsCurrentPreview)
            {
                EmoteWizardGUILayout.HelpBox(Loc("AnimationPreview::Active."), MessageType.Info);
            }
            else
            {
                EmoteWizardGUILayout.HelpBox(Loc("AnimationPreview::Blocked."), MessageType.Warning);
            }
        }

        public void SetClip(InplaceAnimationPreview preview)
        {
            _currentPreview = preview;
            InplacePreviewManager.Instance.TryRefreshPreview(this);
        }

        public void Apply(IInplacePreviewPresenter presenter)
        {
            _currentPreview.Apply(presenter);
        }

        public void Dispose()
        {
            InplacePreviewManager.Instance.TryDisposeRequest(this);
        }
    }
}
