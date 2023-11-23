using System.Collections.Generic;
using Silksprite.EmoteWizard.DataObjects;
using VRC.SDK3.Avatars.ScriptableObjects;

namespace Silksprite.EmoteWizard.Contexts
{
    public interface IExpressionWizardContext : IOutputContext<VRCExpressionsMenu>
    {
        bool BuildAsSubAsset { get; }

        IEnumerable<ExpressionItem> CollectExpressionItems();
    }
}