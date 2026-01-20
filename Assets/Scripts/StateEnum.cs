/// <summary>
/// プレイヤーの状態
/// </summary>
public enum PlayerState
{
    Idle,
    Walk,
    Attack,
    Dodge,
    Hide
}
/// <summary>
/// 敵の状態
/// </summary>
public enum EnemyState
{
    Idle,
    Walk,
    Attack,
    Dead
}
/// <summary>
/// 敵の種類
/// </summary>
public enum EnemyType
{
    Normal,
}
/// <summary>
/// シーンの名前
/// </summary>
public enum SceneName
{
    Title,
    InGame
}