using UnityEngine;

namespace Unity.BossRoom.Gameplay.Actions
{
    [CreateAssetMenu(menuName = "BossRoom/Actions/Shadowraze Near")]
    public class ShadowrazeNearAction : ShadowrazeAction
    {
        public override int Distance { get; set; } = 2;

    }
}
