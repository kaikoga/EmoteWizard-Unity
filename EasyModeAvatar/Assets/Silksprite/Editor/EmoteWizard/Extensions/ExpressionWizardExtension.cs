using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.DataObjects.Internal;
using Silksprite.EmoteWizard.Utils;
using Unity.Collections;
using VRC.SDK3.Avatars.ScriptableObjects;

namespace Silksprite.EmoteWizard.Extensions
{
    public static class ExpressionWizardExtension
    {
        public static IEnumerable<ExpressionItemSet> GroupExpressionItems(this ExpressionWizard expressionWizard)
        {
            var itemFolderIcon = VrcSdkAssetLocator.ItemFolder();

            var folderNames = expressionWizard.expressionItems.SelectMany(item => item.Folders()).Distinct().ToList();

            var groups = folderNames
                .Select(folder => new ExpressionItemSet
                {
                    Items = new List<ExpressionItem>(),
                    Path = folder
                }).ToList();

            var folders = folderNames
                .Where(folder => !string.IsNullOrEmpty(folder))
                .Select(folder => ExpressionItem.PopulateFolder(itemFolderIcon, folder))
                .ToList();

            var allItems = Enumerable.Empty<ExpressionItem>()
                .Concat(expressionWizard.expressionItems.Where(item => item.itemKind == ExpressionItemKind.SubMenu))
                .Concat(folders)
                .Concat(expressionWizard.expressionItems.Where(item => item.itemKind != ExpressionItemKind.SubMenu))
                .DistinctBy(item => item.path);

            foreach (var item in allItems)
            {
                groups.First(f => f.Path == item.Folder).Items.Add(item);
            }

            return groups;
        }
    }
}