using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
[RequireComponent(typeof(Button))]
public class SceneMoveButton : MonoBehaviour,ITitleSelectable
{
    [SerializeField, Tooltip("表示する画像")] private GameObject _obj;
    [SerializeField, Tooltip("移行先のシーン")] private SceneName _sceneName;

    Button _button;
    private void Awake()
    {
        _obj.SetActive(false);
    }
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
    public void OnSelect()
    {
        // 色変更・拡大・SE
        _obj.SetActive(true);
        Debug.Log($"{name} + が選択された");
    }

    public void OnDeselect()
    {
        // 元に戻す
        _obj.SetActive(false);
    }

    public void OnSubmit()
    {
        SceneChange();
        Debug.Log("決定！");
    }
}
