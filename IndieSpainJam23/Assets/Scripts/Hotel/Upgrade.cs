using UnityEngine;

[CreateAssetMenu]
public class Upgrade : ScriptableObject
{
    public enum UpgradeType
    {
        Speed = 0,
        Dash = 1,
        Vacuum = 2,
        TV = 3,
        Bar = 4
    };

    public UpgradeType Type;
    public string Text;
    public int Cost;
}
