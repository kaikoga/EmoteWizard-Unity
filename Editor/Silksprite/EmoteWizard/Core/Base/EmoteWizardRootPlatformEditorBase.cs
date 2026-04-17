using System.Diagnostics.CodeAnalysis;
using Silksprite.EmoteWizard.Contexts;
using Silksprite.Loch;
using Silksprite.Loch.Extensions;

namespace Silksprite.EmoteWizard.Base
{
    public abstract class EmoteWizardRootPlatformEditorBase<T>
    where T : EmoteWizardBehaviour
    {
        readonly EmoteWizardEditorBase<T> _editor;

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        protected T soleTarget => _editor.soleTarget;
        protected EmoteWizardEnvironment CachedEnv() => _editor.CachedEnv();
        protected void HeadingOnce(LocalizedContent loc) => _editor.HeadingOnce(loc);

        protected EmoteWizardRootPlatformEditorBase(EmoteWizardEditorBase<T> editor)
        {
            _editor = editor;
        }

        protected LocalizedProperty Lop(string propertyPath, LocalizedContent loc) => _editor.serializedObject.Lop(propertyPath, loc);
    }
}
