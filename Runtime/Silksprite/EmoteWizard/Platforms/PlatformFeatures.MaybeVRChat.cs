using System;
using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.DataObjects;

namespace Silksprite.EmoteWizard.Platforms
{
    public static partial class PlatformFeatures
    {
        // mostly works like VRChat, but some features may take care of other platforms
        class MaybeVRChatFeatures : VRChatFeatures
        {
            static readonly DefaultActionIndex[] Afk = { DefaultActionIndex.Afk };

            public override IEnumerable<DefaultActionIndex> DefaultActionIndexes() =>
                Enum.GetValues(typeof(DefaultActionIndex)).OfType<DefaultActionIndex>()
                    .Except(Afk)
                    .Concat(Afk);
        }
    }
}
