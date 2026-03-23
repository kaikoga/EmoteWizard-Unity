using System;
using System.Collections.Generic;
using System.Linq;
using Silksprite.EmoteWizard.DataObjects;
using Silksprite.EmoteWizard.DataObjects.Internal;
using Silksprite.EmoteWizard.Platforms.Defaults;
using Silksprite.EmoteWizard.Templates;
using Silksprite.EmoteWizard.Utils;
using Silksprite.EmoteWizard.Wizards;
using UnityEngine;
using static Silksprite.EmoteWizard.EmoteWizardConstants;

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
