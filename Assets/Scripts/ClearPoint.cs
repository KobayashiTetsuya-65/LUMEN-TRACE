using UnityEngine;
using UnityEngine.Playables;

public class ClearPoint : MonoBehaviour
{
    [SerializeField] private PlayableDirector _clearPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _clearPoint.Play();
        }
    }
}
