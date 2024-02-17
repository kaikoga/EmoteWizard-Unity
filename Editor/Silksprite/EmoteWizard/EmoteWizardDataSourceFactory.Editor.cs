using System;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Sources;
using Silksprite.EmoteWizard.Sources.Impl;
using Silksprite.EmoteWizard.Sources.Sequence;
using Silksprite.EmoteWizard.Utils;
using Silksprite.EmoteWizardSupport.L10n;
using Silksprite.EmoteWizardSupport.Scopes;
using Silksprite.EmoteWizardSupport.UI;
using Silksprite.EmoteWizardSupport.Undoable;
using UnityEditor;
using static Silksprite.EmoteWizardSupport.L10n.LocalizationTool;

namespace Silksprite.EmoteWizard
{
    [CustomEditor(typeof(EmoteWizardDataSourceFactory))]
    public class EmoteWizardDataSourceFactoryEditor : EmoteWizardEditorBase<EmoteWizardDataSourceFactory>
    {
        bool _advanced;

        protected override void OnInnerInspectorGUI()
        {
            void UndoableButton(LocalizedContent loc, LocalizedContent desc, Action<IUndoable> callback)
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    EmoteWizardGUILayout.Undoable(loc, callback);
                    if (!desc.IsNullOrEmpty()) EmoteWizardGUILayout.Label(desc);
                }
            }

            var env = CreateEnv();
            
            if (env.Platform.IsVRChat())
            {
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
                        undoable => { undoable.AddChildComponentAndSelect<ParameterSource>(soleTarget, "Parameter Source"); });
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

            using (new BoxLayoutScope())
            {
                var advanced = _advanced = EmoteWizardGUILayout.HeaderFoldout(_advanced, Loc("EmoteWizardDataSourceFactory::advanced"));
                if (advanced)
                {
                    if (env.Platform.IsVRChat())
                    {
                        UndoableButton(Loc("EmoteWizardDataSourceFactory::Emote Item Source"),
                            default,
                            undoable =>
                            {
                                undoable.AddChildComponentAndSelect<EmoteItemSource>(soleTarget, "Emote Item Source");
                            });
                        UndoableButton(Loc("EmoteWizardDataSourceFactory::Emote Sequence Source"),
                            default,
                            undoable =>
                            {
                                undoable.AddChildComponentAndSelect<EmoteSequenceSource>(soleTarget, "Emote Sequence Source");
                            });
                    }

                    UndoableButton(Loc("EmoteWizardDataSourceFactory::Generic Emote Item Source"),
                        default,
                        undoable =>
                        {
                            undoable.AddChildComponentAndSelect<GenericEmoteItemSource>(soleTarget, "Generic Emote Item Source");
                        });
                    UndoableButton(Loc("EmoteWizardDataSourceFactory::Generic Emote Sequence Source"),
                        default,
                        undoable =>
                        {
                            undoable.AddChildComponentAndSelect<GenericEmoteSequenceSource>(soleTarget, "Generic Emote Sequence Source");
                        });
                }
            }
        }
    }
}