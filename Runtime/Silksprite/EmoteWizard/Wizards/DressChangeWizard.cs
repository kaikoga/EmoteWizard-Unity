using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.Base;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.Templates;
using Silksprite.EmoteWizard.Templates.Impl;
using Silksprite.EmoteWizard.Utils;
using UnityEngine;

namespace Silksprite.EmoteWizard.Sources
{
    [AddComponentMenu("Emote Wizard/Wizards/Dress Change Wizard", 300)]
    public class DressChangeWizard : EmoteWizardBase
    {
        [SerializeField] public int itemCount = 2;
        [SerializeField] public ExpressionKind expressionKind;
        [SerializeField] public EmoteSequenceFactoryKind emoteSequenceFactoryKind;
        [ItemPath]
        [SerializeField] public string itemPath;
        [SerializeField] public bool hasGroupName;
        [SerializeField] public string groupName;
        [SerializeField] public bool hasParameterName;
        [ParameterName(false, true)]
        [SerializeField] public string parameterName;

        public override IEnumerable<IEmoteTemplate> SourceTemplates()
        {
            var paramName = hasParameterName ? parameterName : itemPath;
            var actualItemCount = expressionKind == ExpressionKind.SimpleToggle ? 2 : Mathf.Clamp(itemCount, 2, 64);

            foreach (var value in Enumerable.Range(0, actualItemCount))
            {
                var childName = $"{itemPath}/Item {value}";
                yield return new EmoteItemTemplate(
                    childName,
                    new EmoteTrigger
                    {
                        name = childName,
                        priority = 0,
                        conditions = new List<EmoteCondition>
                        {
                            expressionKind == ExpressionKind.SimpleRadial 
                                ? new EmoteCondition
                                {
                                    kind = ParameterItemKind.Float,
                                    parameter = paramName,
                                    mode = EmoteConditionMode.Less,
                                    threshold = value == actualItemCount - 1 ? 1.01f : (value + 1f) / actualItemCount 
                                }
                                : new EmoteCondition
                                {
                                    kind = ParameterItemKind.Int,
                                    parameter = paramName,
                                    mode = EmoteConditionMode.Equals,
                                    threshold = value
                                }
                        }
                    },
                    GenerateEmoteSequenceFactoryTemplate(emoteSequenceFactoryKind,
                        LayerKind.FX,
                        groupName = hasGroupName ? groupName : itemPath),
                    expressionKind == ExpressionKind.Builtin,
                    childName,
                    VrcSdkAssetLocator.ItemWand()
                );
                
                if (expressionKind == ExpressionKind.ToggleItems)
                {
                    yield return new ExpressionItemTemplate(
                        childName, new ExpressionItem
                        {
                            enabled = true,
                            icon = VrcSdkAssetLocator.ItemWand(),
                            path = childName,
                            parameter = paramName,
                            value = value,
                            itemKind = ExpressionItemKind.Toggle
                        });
                }
            }
            switch (expressionKind)
            {
                case ExpressionKind.SimpleToggle:
                    yield return new ExpressionItemTemplate(
                        itemPath, new ExpressionItem
                        {
                            enabled = true,
                            icon = VrcSdkAssetLocator.ItemWand(),
                            path = itemPath,
                            parameter = paramName,
                            value = 1,
                            itemKind = ExpressionItemKind.Toggle
                        });
                    break;
                case ExpressionKind.SimpleRadial:
                    yield return new ExpressionItemTemplate(
                        itemPath, new ExpressionItem
                        {
                            enabled = true,
                            icon = VrcSdkAssetLocator.ItemWand(),
                            path = itemPath,
                            value = 0,
                            itemKind = ExpressionItemKind.RadialPuppet,
                            subParameters = new [] { paramName },
                            labels = new [] { itemPath }
                        });
                    break;
            }
        }

        public enum ExpressionKind
        {
            Builtin,
            ToggleItems,
            SimpleToggle,
            SimpleRadial
        }
    }
}