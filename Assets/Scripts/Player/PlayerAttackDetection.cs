using UnityEngine;
/// <summary>
/// ”ÍˆÍ“à‚Ì“G‚Éƒ_ƒ[ƒW‚ğ—^‚¦‚é
/// </summary>
public class PlayerAttackDetection : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<IEnemy>(out var enemy))
        {
            enemy.Damaged();
        }
    }
}
