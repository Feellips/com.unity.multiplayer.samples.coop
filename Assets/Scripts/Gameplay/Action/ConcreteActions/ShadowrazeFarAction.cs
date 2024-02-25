using UnityEngine;

namespace Unity.BossRoom.Gameplay.Actions
{
    [CreateAssetMenu(menuName = "BossRoom/Actions/Shadowraze Far")]
    public class ShadowrazeFarAction : ShadowrazeAction
    {
        public override int Distance { get; set; } = 6;
    }
}
