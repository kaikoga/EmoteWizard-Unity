using Silksprite.EmoteWizard.Preview.Core.Internal.Presentation;
using UnityEditor;

namespace Silksprite.EmoteWizard.Preview.Core.Internal
{
    public class InplacePreviewManager
    {
        public static readonly InplacePreviewManager Instance = new InplacePreviewManager();

        public InplacePreviewRequest CurrentPreviewRequest;

        InplacePreviewManager()
        {
            AssemblyReloadEvents.beforeAssemblyReload += Dispose;
#if UNITY_2022_3_OR_NEWER
            ObjectChangeEvents.changesPublished += OnChangesPublished;
#endif
        }

#if UNITY_2022_3_OR_NEWER
        void OnChangesPublished(ref ObjectChangeEventStream stream)
        {
            if (stream.length > 0)
            {
                TryRefreshPreview(CurrentPreviewRequest);
            }
        }
#endif

        public void TryDisposeRequest(InplacePreviewRequest previewRequest)
        {
            if (previewRequest == CurrentPreviewRequest)
            {
                DisposeCurrentRequest();
            }
        }

        public void TryRefreshPreview(InplacePreviewRequest previewRequest)
        {
            if (CurrentPreviewRequest != null)
            {
                if (previewRequest != null && previewRequest != CurrentPreviewRequest)
                {
                    return;
                }
                DisposeCurrentRequest();
            }
            if (previewRequest?.HasPreview != true)
            {
                return;
            }
            CurrentPreviewRequest = previewRequest;
            InplacePreviewPresenter.Instance.StartPreview(CurrentPreviewRequest);
            CurrentPreviewRequest.Apply(InplacePreviewPresenter.Instance);
        }

        void DisposeCurrentRequest()
        {
            CurrentPreviewRequest = null;
            InplacePreviewPresenter.Instance.EndPreview();
        }

        void Dispose()
        {
            DisposeCurrentRequest();
            AssemblyReloadEvents.beforeAssemblyReload -= Dispose;
#if UNITY_2022_3_OR_NEWER
            ObjectChangeEvents.changesPublished -= OnChangesPublished;
#endif
        }
    }

}
