using UnityEngine;
using VRC.SDKBase;

namespace EmoteWizard
{
    public class EmoteWizardRoot : MonoBehaviour
    {
        [SerializeField] VRC_AvatarDescriptor avatarDescriptor;

        [SerializeField] [HideInInspector] public string generatedAssetRoot;
    }
}