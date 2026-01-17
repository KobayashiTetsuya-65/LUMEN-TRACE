using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
[RequireComponent(typeof(Button))]
public class SceneMoveButton : MonoBehaviour
{
    [SerializeField, Tooltip("à⁄çsêÊÇÃÉVÅ[Éì")] private string _sceneName;

    Button _button;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(SceneChange);
    }

    private void SceneChange()
    {
        GameManager.Instance.SceneMove(_sceneName);
    }
}
