using UnityEngine;
/// <summary>
/// プレイヤーを捜索しターゲットする
/// </summary>
public class EnemyPlayerDetector : MonoBehaviour
{
    public Transform CurrentTarget {  get; private set; }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CurrentTarget = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CurrentTarget = null;
        }
    }
}
