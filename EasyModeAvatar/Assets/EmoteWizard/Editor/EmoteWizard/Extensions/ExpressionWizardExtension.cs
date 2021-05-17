using System.Collections.Generic;
using System.Linq;
using EmoteWizard.DataObjects;
using EmoteWizard.Tools;

namespace EmoteWizard.Extensions
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

            foreach (var item in folders.Concat(expressionWizard.expressionItems))
            {
                groups.First(f => f.Path == item.Folder).Items.Add(item);
            }

            return groups;
        }
    }
}