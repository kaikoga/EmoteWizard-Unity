using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Silksprite.EmoteWizard.Preview.Presentation
{
    class InplacePreviewPresenter : IInplacePreviewPresenter
    {
        public static readonly IInplacePreviewPresenter Instance = new InplacePreviewPresenter();

        public GameObject Avatar { get; private set; }

        public void EndPreview()
        {
            SceneVisibilityManager.instance.ExitIsolation();
            if (Avatar)
            {
                Object.DestroyImmediate(Avatar);
            }
        }

        public void StartPreview(InplacePreviewRequest previewRequest)
        {
            Avatar = Object.Instantiate(previewRequest.OriginalAvatar);
            SceneVisibilityManager.instance.Isolate(Avatar.gameObject, true);
            Avatar.hideFlags = HideFlags.HideAndDontSave;
        }
    }
}
