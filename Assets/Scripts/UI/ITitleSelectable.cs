/// <summary>
/// タイトル画面のボタンの処理を共通化
/// </summary>
public interface ITitleSelectable
{
    /// <summary>
    /// 選択されたとき
    /// </summary>
    void OnSelect();
    /// <summary>
    /// 選択が外れた時
    /// </summary>
    void OnDeselect();
    /// <summary>
    /// 決定されたとき
    /// </summary>
    void OnSubmit();
}
