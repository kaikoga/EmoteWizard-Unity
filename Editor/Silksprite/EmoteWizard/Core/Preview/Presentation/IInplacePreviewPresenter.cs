using UnityEngine;

namespace Silksprite.EmoteWizard.Preview.Presentation
{
    public interface IInplacePreviewPresenter
    {
        GameObject Avatar { get; }
        void EndPreview();
        void StartPreview(InplacePreviewRequest currentPreviewRequest);
    }
}