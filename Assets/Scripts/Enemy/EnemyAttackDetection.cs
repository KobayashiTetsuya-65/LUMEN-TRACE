using UnityEngine;
/// <summary>
/// 範囲内のプレイヤーにダメージを与える
/// </summary>
public class EnemyAttackDetection : MonoBehaviour
{
    public bool IsAttack = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<IPlayer>(out var player))
        {
            if(!IsAttack)
            {
                player.Damaged();
                IsAttack = true;
            }
        }
    }
}
