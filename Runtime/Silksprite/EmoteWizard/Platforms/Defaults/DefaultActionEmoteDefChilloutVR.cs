using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.DataObjects.Extensions;
using Silksprite.EmoteWizard.Templates;
using Silksprite.EmoteWizard.Templates.Impl;
using Silksprite.EmoteWizard.Utils;
using Silksprite.EmoteWizard.Wizards;
using UnityEngine;

namespace Silksprite.EmoteWizard.Platforms.Defaults
{
    class DefaultActionEmoteDefChilloutVR
    {
        static readonly List<TrackingOverride> ActionTrackingOverrides = new[]
        {
            TrackingTarget.Head,
            TrackingTarget.LeftHand,
            TrackingTarget.RightHand,
            TrackingTarget.Hip,
            TrackingTarget.LeftFoot,
            TrackingTarget.RightFoot,
            TrackingTarget.LeftFingers,
            TrackingTarget.RightFingers
        }.Select(target => new TrackingOverride { target = target }).ToList();

        public DefaultActionIndex Key;
        int _index;
        Motion? _clip;

        string Name => Key.Name();
            
        public IEmoteTemplate ToEmoteItemTemplate(EmoteTemplatePath path)
        {
            if (Key == DefaultActionIndex.Afk)
            {
                return UnpackAsAfk(path);
            }

            var expressionItemIcon = VrcSdkAssetLocator.PersonDance();

            return EmoteItemTemplate.Builder(LayerKind.Action, path.Join(Name), EmoteWizardConstants.Groups.Action,
                    default,
                    EmoteItemKind.EmoteItem, EmoteSequenceFactoryKind.EmoteSequence)
                .AddCondition(new EmoteCondition { kind = ParameterItemKind.Int, parameter = EmoteWizardConstants.Params.ActionSelect, mode = EmoteConditionMode.Equals, threshold = _index })
                .AddFixedDuration(true)
                .AddClip(_clip)
                .AddClipExitTime(true, 1f)
                .AddTrackingOverrides(true, ActionTrackingOverrides)
                .AddExpressionItem(true, $"Default/{Name}", expressionItemIcon)
                .ToEmoteItemTemplate();
        }

        IEmoteTemplate UnpackAsAfk(EmoteTemplatePath path)
        {
            return EmoteItemTemplate.Builder(LayerKind.Action, path.Join(Name), EmoteWizardConstants.Groups.Action,
                    default,
                    EmoteItemKind.EmoteItem, EmoteSequenceFactoryKind.EmoteSequence)
                .AddPriority(100)
                .AddCondition(new EmoteCondition { kind = ParameterItemKind.Bool, parameter = EmoteWizardConstants.Params.Afk, mode = EmoteConditionMode.If, threshold = 0 })
                .AddFixedDuration(true)
                .AddTrackingOverrides(true, ActionTrackingOverrides)
                .ToEmoteItemTemplate();
        }

        public static DefaultActionEmoteDefChilloutVR? Default(DefaultActionIndex index)
        {
            return index switch
            {
                DefaultActionIndex.Emote1 => new DefaultActionEmoteDefChilloutVR
                {
                    _index = 1,
                    _clip = CvrCckAssetLocator.EmotesEmote1()
                },
                DefaultActionIndex.Emote2 => new DefaultActionEmoteDefChilloutVR
                {
                    _index = 2,
                    _clip = CvrCckAssetLocator.EmotesEmote2()
                },
                DefaultActionIndex.Emote3 => new DefaultActionEmoteDefChilloutVR
                {
                    _index = 3,
                    _clip = CvrCckAssetLocator.EmotesEmote3()
                },
                DefaultActionIndex.Emote4 => new DefaultActionEmoteDefChilloutVR
                {
                    _index = 4,
                    _clip = CvrCckAssetLocator.EmotesEmote4()
                },
                DefaultActionIndex.Emote5 => new DefaultActionEmoteDefChilloutVR
                {
                    _index = 5,
                    _clip = CvrCckAssetLocator.EmotesEmote5()
                },
                DefaultActionIndex.Emote6 => new DefaultActionEmoteDefChilloutVR
                {
                    _index = 6,
                    _clip = CvrCckAssetLocator.EmotesEmote6()
                },
                DefaultActionIndex.Emote7 => new DefaultActionEmoteDefChilloutVR
                {
                    _index = 7,
                    _clip = CvrCckAssetLocator.EmotesEmote7()
                },
                DefaultActionIndex.Emote8 => new DefaultActionEmoteDefChilloutVR
                {
                    _index = 8,
                    _clip = CvrCckAssetLocator.EmotesEmote8()
                },
                _ => null
            };
        }
    }
}
