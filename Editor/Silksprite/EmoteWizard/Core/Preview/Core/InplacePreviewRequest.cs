using Silksprite.EmoteWizard.Preview.Core.Internal;
using Silksprite.EmoteWizard.Preview.Core.Internal.Presentation;
using UnityEngine;

namespace Silksprite.EmoteWizard.Preview.Core
{
    public class InplacePreviewRequest
    {
        public readonly GameObject OriginalAvatar;
        IInplacePreviewPosing _currentPosing;

        public bool IsCurrentPreview => InplacePreviewManager.Instance.CurrentPreviewRequest == this;
        public bool HasPreview => _currentPosing != null && OriginalAvatar;

        public InplacePreviewRequest(GameObject originalAvatar)
        {
            OriginalAvatar = originalAvatar;
        }

        public void SetPosing(IInplacePreviewPosing posing)
        {
            _currentPosing = posing;
            InplacePreviewManager.Instance.TryRefreshPreview(this);
        }

        public void Apply(IInplacePreviewPresenter presenter)
        {
            _currentPosing.Apply(presenter);
        }

        public void Dispose()
        {
            InplacePreviewManager.Instance.TryDisposeRequest(this);
        }
    }
}
