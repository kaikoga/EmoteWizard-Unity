using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.DataObjects.Internal;
using Silksprite.EmoteWizard.Extensions;
using Silksprite.EmoteWizard.Utils;
using UnityEditor;
using UnityEngine;
using VRC.SDK3.Avatars.ScriptableObjects;

namespace Silksprite.EmoteWizard.Contexts.Extensions
{
    public static class ExpressionContextExtension
    {
        static IEnumerable<ExpressionItemSet> GroupExpressionItems(this ExpressionContext context)
        {
            var activeExpressionItems = context.AllExpressionItems().ToList();
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

        public static VRCExpressionsMenu BuildOutputAsset(this ExpressionContext context)
        {
            var expressionMenu = context.ReplaceOrCreateOutputAsset(GeneratedPaths.GeneratedExprMenu);

            var rootItemPath = AssetDatabase.GetAssetPath(expressionMenu);
            var rootPath = string.IsNullOrEmpty(rootItemPath) ? null : $"{rootItemPath.Substring(0, rootItemPath.Length - 6)}/";

            var groups = context.GroupExpressionItems().ToList();

            var menus = new Dictionary<string, VRCExpressionsMenu>();

            // populate folders first
            foreach (var group in groups)
            {
                if (group.Path == "")
                {
                    menus[group.Path] = expressionMenu;
                    EditorUtility.SetDirty(expressionMenu);
                }
                else if (context.BuildAsSubAsset)
                {
                    var childMenu = ScriptableObject.CreateInstance<VRCExpressionsMenu>();
                    if (context.Environment.PersistGeneratedAssets)
                    {
                        AssetDatabase.AddObjectToAsset(childMenu, rootItemPath);
                    }
                    childMenu.name = group.Path;
                    menus[group.Path] = childMenu;
                }
                else
                {
                    var childMenu = ScriptableObject.CreateInstance<VRCExpressionsMenu>();
                    var childPath = new GeneratedPath($"{rootPath}{group.Path}.asset");
                    childMenu = context.Environment.EnsureAsset(childPath, childMenu);
                    menus[group.Path] = childMenu;
                }
            }
            
            foreach (var group in groups)
            {
                var controls = group.Items
                    .Select(item => item.ToControl(path => menus.TryGetValue(path, out var v) ? v : null))
                    .ToList();
                menus[group.Path].controls = controls;
            }

            AssetDatabase.SaveAssets();

            return expressionMenu;
        }
    }
}