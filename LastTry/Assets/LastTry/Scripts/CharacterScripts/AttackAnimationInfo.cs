
[System.Serializable]
public class AttackAnimationInfo
{
    public enum WeaponType { None, Sword, Staff, Hammer};

    public float TotalTime;
    public float ProcessTime;
    public WeaponType Weapon = WeaponType.None;

    /// <summary>
    /// This method converts this class to CombatInfo object.
    /// </summary>
    /// <returns>The converted CombatInfo, of type CombatInfo</returns>
    public CombatInfo ToCombatInfo()
    {
        return new CombatInfo(TotalTime, ProcessTime, Weapon);
    }
}
