using UnityEngine;

namespace Silksprite.EmoteWizard.Preview.Core.Internal.Presentation
{
    public interface IInplacePreviewPresenter
    {
        GameObject Avatar { get; }
        void EndPreview();
        void StartPreview(InplacePreviewRequest currentPreviewRequest);
    }
}