using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ACT.Health
{
    public class AIHealthSystem : BasicHealthModel
    {
        private void LateUpdate()
        {
            OnHitAnimationRotation(); 
        }


        public override void TakeDamager(float damagar, string hitAnimationName, Transform attacker)
        {
            SetAttacker(attacker);
            animator.Play(hitAnimationName, 0, 0f);
            GameAssets.Instance.PlaySoundEffect(_audioSource, SoundAssetsType.hit);

            

        }

        private void OnHitAnimationRotation()
        {
            if(animator.CheckAnimationTag("Hit"))
            {
                transform.rotation = transform.LockOnTarget(currentAttacker, transform, 50f);
            }
        }

    }

}