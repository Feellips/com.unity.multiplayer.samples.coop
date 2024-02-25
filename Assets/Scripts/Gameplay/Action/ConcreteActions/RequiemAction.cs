using Unity.BossRoom.Gameplay.GameplayObjects;
using Unity.BossRoom.Gameplay.GameplayObjects.Character;
using UnityEngine;

namespace Unity.BossRoom.Gameplay.Actions
{
    [CreateAssetMenu(menuName = "BossRoom/Actions/Requiem")]
    public class RequiemAction : Action
    {
        private bool m_DidRequiem;

        public override bool OnStart(ServerCharacter serverCharacter)
        {
            serverCharacter.serverAnimationHandler.NetworkAnimator.SetTrigger(Config.Anim);

            // tell clients to visualize this action
            serverCharacter.clientCharacter.RecvDoActionClientRPC(Data);

            return ActionConclusion.Continue;
        }

        public override void Reset()
        {
            base.Reset();
            m_DidRequiem = false;
        }

        public override bool OnUpdate(ServerCharacter clientCharacter)
        {
            if (TimeRunning >= Config.ExecTimeSeconds && !m_DidRequiem)
            {
                // actually perform the AoE attack
                m_DidRequiem = true;
                PerformShadowraze(clientCharacter);
            }

            return ActionConclusion.Continue;
        }

        public override void End(ServerCharacter serverCharacter)
        {
            // Anim2 contains the name of the end-loop-sequence trigger
            if (!string.IsNullOrEmpty(Config.Anim2))
            {
                serverCharacter.serverAnimationHandler.NetworkAnimator.SetTrigger(Config.Anim2);
            }
        }

        public override void Cancel(ServerCharacter serverCharacter)
        {
            // OtherAnimatorVariable contains the name of the cancellation trigger
            if (!string.IsNullOrEmpty(Config.OtherAnimatorVariable))
            {
                serverCharacter.serverAnimationHandler.NetworkAnimator.SetTrigger(Config.OtherAnimatorVariable);
            }

            // because the client-side visualization of the action moves the character visualization around,
            // we need to explicitly end the client-side visuals when we abort
            serverCharacter.clientCharacter.RecvCancelActionsByPrototypeIDClientRpc(ActionID);

        }

        public override void BuffValue(BuffableValue buffType, ref float buffedValue)
        {
            if (TimeRunning >= Config.ExecTimeSeconds && buffType == BuffableValue.PercentDamageReceived)
            {
                // we suffer no damage during the "dash" (client-side pretend movement)
                buffedValue = 0;
            }
        }

        private void PerformShadowraze(ServerCharacter parent)
        {
            // Note: could have a non alloc version of this overlap sphere where we statically store our collider array, but since this is a self
            // destroyed object, the complexity added to have a static pool of colliders that could be called by multiplayer players at the same time
            // doesn't seem worth it for now.
            var colliders = Physics.OverlapSphere(parent.transform.position, Config.Radius);
            for (var i = 0; i < colliders.Length; i++)
            {
                var enemy = colliders[i].GetComponent<IDamageable>();
                if (enemy != null)
                {
                    // actually deal the damage
                    enemy.ReceiveHP(parent, -Config.Amount);
                }
            }
        }

        public override bool OnUpdateClient(ClientCharacter clientCharacter)
        {
            if (m_DidRequiem) { return ActionConclusion.Stop; } // we're done!

            return ActionConclusion.Continue;
        }

        public override bool OnStartClient(ClientCharacter clientCharacter)
        {
            base.OnStartClient(clientCharacter);
            GameObject.Instantiate(Config.Spawns[0], clientCharacter.transform.position, Quaternion.identity);
            return ActionConclusion.Stop;
        }
    }
}
