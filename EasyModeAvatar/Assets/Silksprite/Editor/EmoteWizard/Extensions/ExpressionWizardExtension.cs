using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.DataObjects.Internal;
using Silksprite.EmoteWizard.Utils;
using UnityEditor;
using UnityEngine;
using VRC.SDK3.Avatars.ScriptableObjects;

namespace Silksprite.EmoteWizard.Extensions
{
    public static class ExpressionWizardExtension
    {
        static IEnumerable<ExpressionItemSet> GroupExpressionItems(this ExpressionWizard expressionWizard)
        {
            var activeExpressionItems = expressionWizard.CollectExpressionItems().ToList();
            var itemFolderIcon = VrcSdkAssetLocator.ItemFolder();

            var folderNames = activeExpressionItems.SelectMany(item => item.Folders()).Distinct().ToList();

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
                .Concat(activeExpressionItems.Where(item => item.itemKind == ExpressionItemKind.SubMenu))
                .Concat(folders)
                .Concat(activeExpressionItems.Where(item => item.itemKind != ExpressionItemKind.SubMenu))
                .DistinctBy(item => item.path);

            foreach (var item in allItems)
            {
                groups.First(f => f.Path == item.Folder).Items.Add(item);
            }

            return groups;
        }

        public static VRCExpressionsMenu BuildOutputAsset(this ExpressionWizard expressionWizard)
        {
            var expressionMenu = expressionWizard.ReplaceOrCreateOutputAsset(ref expressionWizard.outputAsset, "Expressions/@@@Generated@@@ExprMenu.asset");

            var rootItemPath = AssetDatabase.GetAssetPath(expressionMenu);
            var rootPath = $"{rootItemPath.Substring(0, rootItemPath.Length - 6)}/";

            var groups = expressionWizard.GroupExpressionItems().ToList();

            var menus = new Dictionary<string, VRCExpressionsMenu>();

            // populate folders first
            foreach (var group in groups)
            {
                if (@group.Path == "")
                {
                    menus[@group.Path] = expressionMenu;
                    EditorUtility.SetDirty(expressionMenu);
                }
                else if (expressionWizard.buildAsSubAsset)
                {
                    var childMenu = ScriptableObject.CreateInstance<VRCExpressionsMenu>();
                    AssetDatabase.AddObjectToAsset(childMenu, rootItemPath);
                    childMenu.name = @group.Path;
                    menus[@group.Path] = childMenu;
                }
                else
                {
                    var childMenu = ScriptableObject.CreateInstance<VRCExpressionsMenu>();
                    var childPath = $"{rootPath}{@group.Path}.asset";
                    expressionWizard.ReplaceOrCreateOutputAsset(ref childMenu, childPath);
                    menus[@group.Path] = childMenu;
                }
            }
            
            foreach (var group in groups)
            {
                var controls = @group.Items
                    .Select(item => item.ToControl(path => menus.TryGetValue(path, out var v) ? v : null))
                    .ToList();
                menus[@group.Path].controls = controls;
            }

            AssetDatabase.SaveAssets();

            return expressionMenu;
        }
    }
}