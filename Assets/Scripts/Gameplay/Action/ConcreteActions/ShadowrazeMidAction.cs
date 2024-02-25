using UnityEngine;

namespace Unity.BossRoom.Gameplay.Actions
{
    [CreateAssetMenu(menuName = "BossRoom/Actions/Shadowraze Mid")]
    public class ShadowrazeMidAction : ShadowrazeAction
    {
        public override int Distance { get; set; } = 4;

    }
}
