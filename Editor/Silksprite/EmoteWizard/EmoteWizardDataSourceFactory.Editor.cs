using System;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Sources;
using Silksprite.EmoteWizard.Sources.Impl;
using Silksprite.EmoteWizard.Utils;
using Silksprite.EmoteWizardSupport.Extensions;
using Silksprite.EmoteWizardSupport.L10n;
using Silksprite.EmoteWizardSupport.Scopes;
using Silksprite.EmoteWizardSupport.UI;
using Silksprite.EmoteWizardSupport.Undoable;
using UnityEditor;
using UnityEngine;
using static Silksprite.EmoteWizardSupport.L10n.LocalizationTool;

namespace Silksprite.EmoteWizard
{
    [CustomEditor(typeof(EmoteWizardDataSourceFactory))]
    public class EmoteWizardDataSourceFactoryEditor : EmoteWizardEditorBase<EmoteWizardDataSourceFactory>
    {
        protected override void OnInnerInspectorGUI()
        {
            void UndoableButton(LocalizedContent loc, LocalizedContent desc, Action<IUndoable> callback)
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    EmoteWizardGUILayout.Undoable(loc, callback);
                    EmoteWizardGUILayout.Label(desc, new GUILayoutOption[0]);
                }
            }

            using (new BoxLayoutScope())
            {
                UndoableButton(Loc("EmoteWizardDataSourceFactory::Expression Item Source"),
                    Loc("EmoteWizardDataSourceFactory::Expression Item Source::Desc"),
                    undoable =>
                    {
                        undoable.AddChildComponentAndSelect<ExpressionItemSource>(soleTarget, "Expression Item Source")
                            .expressionItem = new ExpressionItem
                        {
                            enabled = true,
                            icon = VrcSdkAssetLocator.ItemWand(),
                            itemKind = ExpressionItemKind.Button
                        };
                    });
                UndoableButton(Loc("EmoteWizardDataSourceFactory::Sub Menu Expression Item Source"),
                    Loc("EmoteWizardDataSourceFactory::Sub Menu Expression Item Source::Desc"),
                    undoable =>
                    {
                        undoable.AddChildComponentAndSelect<ExpressionItemSource>(soleTarget, "Sub Menu")
                            .expressionItem = new ExpressionItem
                        {
                            enabled = true,
                            icon = VrcSdkAssetLocator.ItemFolder(),
                            value = 0,
                            itemKind = ExpressionItemKind.SubMenu
                        };
                    });
                UndoableButton(Loc("EmoteWizardDataSourceFactory::Parameter Source"),
                    Loc("EmoteWizardDataSourceFactory::Parameter Source::Desc"),
                    undoable =>
                    {
                        undoable.AddChildComponentAndSelect<ParameterSource>(soleTarget, "Parameter Source");
                    });
            }

            using (new BoxLayoutScope())
            {
                UndoableButton(Loc("EmoteWizardDataSourceFactory::Emote Item Wizard"),
                    Loc("EmoteWizardDataSourceFactory::Emote Item Wizard::Desc"),
                    undoable =>
                    {
                        undoable.AddChildComponentAndSelect<EmoteItemWizard>(soleTarget, "Emote Item Wizard");
                    });

                UndoableButton(Loc("EmoteWizardDataSourceFactory::Dress Change Wizard"),
                    Loc("EmoteWizardDataSourceFactory::Dress Change Wizard::Desc"),
                    undoable =>
                    {
                        undoable.AddChildComponentAndSelect<DressChangeWizard>(soleTarget, "Dress Change Wizard");
                    });
                UndoableButton(Loc("EmoteWizardDataSourceFactory::Custom Action Wizard"),
                    Loc("EmoteWizardDataSourceFactory::Custom Action Wizard::Desc"),
                    undoable =>
                    {
                        undoable.AddChildComponentAndSelect<CustomActionWizard>(soleTarget, "Custom Action Wizard");
                    });
            }
        }
    }
}