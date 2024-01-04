using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.Contexts;
using Silksprite.EmoteWizard.Internal.ClipBuilders;
using Silksprite.EmoteWizard.Sources;
using Silksprite.EmoteWizard.Sources.Impl;
using Silksprite.EmoteWizard.Sources.Sequence;
using Silksprite.EmoteWizard.Sources.Sequence.Base;
using Silksprite.EmoteWizardSupport.Extensions;
using Silksprite.EmoteWizardSupport.Undoable;
using Silksprite.EmoteWizardSupport.Utils;
using UnityEditor;
using UnityEngine;

namespace Silksprite.EmoteWizard.Utils
{
    public static class WizardExploder
    {
        public static void ExplodeImmediate(IUndoable undoable, EmoteWizardBase wizard)
        {
            foreach (var template in wizard.SourceTemplates())
            {
                template.PopulateSources(undoable, wizard);
            }
            undoable.DestroyObject(wizard);
        }
    }
}