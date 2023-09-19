using System.Collections.Generic;

[System.Serializable]
public class Skill
{
    public int level;
    public int maxLevel;
    public string name;
    public List<SkillData> levels;
}

[System.Serializable]
public enum AttackPatturn
{
    SPLASH,
    MELEE,
    FIRE,
    SHOTGUN
}

[System.Serializable]
public enum Rarity
{
    COMMON,
    RARE,
    EPIC,
    LEGEND
}

[System.Serializable]
public class SkillData
{
    public Rarity Rarity;
    public AttackPatturn pattern;
    public string objId;
    public float atk;
    public float coolTime;
    public float range;
    public float remainTime;
    public float knockback;
}