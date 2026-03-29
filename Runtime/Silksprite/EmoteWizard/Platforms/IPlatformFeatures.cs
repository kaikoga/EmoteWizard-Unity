using System.Collections.Generic;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.DataObjects.Internal;
using Silksprite.EmoteWizard.Templates;
using Silksprite.EmoteWizard.Wizards;
using UnityEngine;

namespace Silksprite.EmoteWizard.Platforms
{
    public interface IPlatformFeatures
    {
        string ParameterForAlwaysTrue { get; }
        string ParameterReferenceForActionSelect { get; }
        string ResolveMirrorParameter(string virtualParameter, EmoteHand hand);

        Motion GestureClipIdleLeft { get; }
        Motion GestureClipFistLeft { get; }
        Motion GestureClipOpenLeft { get; }
        Motion GestureClipPointLeft { get; }
        Motion GestureClipPeaceLeft { get; }
        Motion GestureClipRockNRollLeft { get; }
        Motion GestureClipGunLeft { get; }
        Motion GestureClipThumbsUpLeft { get; }
        Motion GestureClipIdleRight { get; }
        Motion GestureClipFistRight { get; }
        Motion GestureClipOpenRight { get; }
        Motion GestureClipPointRight { get; }
        Motion GestureClipPeaceRight { get; }
        Motion GestureClipRockNRollRight { get; }
        Motion GestureClipGunRight { get; }
        Motion GestureClipThumbsUpRight { get; }

        AvatarMask HandLeft { get; }
        AvatarMask HandRight { get; }

        int HandSignValue(HandSign handSign);
        List<ParameterInstance> DefaultParameters();
        bool IsDefaultParameterReference(string parameterReference);
        string ResolveParameterReference(string parameterReference);

        IEnumerable<IEmoteTemplate> UnpackDefaultHandSign(EmoteTemplatePath path, EmoteItemKind emoteItemKind, EmoteSequenceFactoryKind emoteSequenceFactoryKind, LayerKind layerKind, HandSign handSign);
        IEnumerable<DefaultActionIndex> DefaultActionIndexes();
        IEnumerable<IEmoteTemplate> UnpackDefaultAction(IPlatformFeatures platformFeatures, EmoteTemplatePath path, DefaultActionIndex index);
    }
}
