using System.Collections.Generic;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.DataObjects.Internal;
using Silksprite.EmoteWizard.Templates;
using UnityEngine;

namespace Silksprite.EmoteWizard.Platforms
{
    public interface IPlatformFeatures
    {
        string ParameterForAlwaysTrue { get; }
        string GestureLeft { get; }
        string GestureLeftWeight { get; }
        string GestureRight { get; }
        string GestureRightWeight { get; }

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

        bool IsHandSignParameterReference(string parameterReference);
        string ResolveParameterReference(string parameterReference);
        int HandSignValue(HandSign handSign);
        List<ParameterInstance> DefaultParameters();
        bool IsDefaultParameterReference(string parameterReference);

        IEnumerable<DefaultActionIndex> DefaultActionIndexes();
        IEnumerable<IEmoteTemplate> UnpackDefaultAction(EmoteTemplatePath path, DefaultActionIndex index);
    }
}
