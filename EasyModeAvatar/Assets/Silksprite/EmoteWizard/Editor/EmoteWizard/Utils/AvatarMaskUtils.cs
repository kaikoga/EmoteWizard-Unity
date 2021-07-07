using UnityEngine;

namespace Silksprite.EmoteWizard.Utils
{
    public static class AvatarMaskUtils
    {
        public static AvatarMask SetupAsGestureDefault(AvatarMask avatarMask)
        {
            avatarMask.SetHumanoidBodyPartActive(AvatarMaskBodyPart.Root, false);
            avatarMask.SetHumanoidBodyPartActive(AvatarMaskBodyPart.Body, false);
            avatarMask.SetHumanoidBodyPartActive(AvatarMaskBodyPart.Head, false);
            avatarMask.SetHumanoidBodyPartActive(AvatarMaskBodyPart.LeftLeg, false);
            avatarMask.SetHumanoidBodyPartActive(AvatarMaskBodyPart.RightLeg, false);
            avatarMask.SetHumanoidBodyPartActive(AvatarMaskBodyPart.LeftArm, false);
            avatarMask.SetHumanoidBodyPartActive(AvatarMaskBodyPart.RightArm, false);
            avatarMask.SetHumanoidBodyPartActive(AvatarMaskBodyPart.LeftFootIK, false);
            avatarMask.SetHumanoidBodyPartActive(AvatarMaskBodyPart.RightFootIK, false);
            avatarMask.SetHumanoidBodyPartActive(AvatarMaskBodyPart.LeftHandIK, false);
            avatarMask.SetHumanoidBodyPartActive(AvatarMaskBodyPart.RightHandIK, false);
            return avatarMask;
        }
    }
}