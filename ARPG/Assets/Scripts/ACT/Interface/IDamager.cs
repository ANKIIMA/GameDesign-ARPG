
using UnityEngine;

public interface IDamager
{
    void TakeDamager(float damager);
    void TakeDamager(string hitAnimationName);
    void TakeDamager(float damager, string hitAnimationName);
    void TakeDamager(float damagar, string hitAnimationName, Transform attacker);
}
