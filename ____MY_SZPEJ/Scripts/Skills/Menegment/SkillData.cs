using UnityEngine;

[System.Serializable]
public class SkillData
{
    public int id;
    public string skillName;
    public Sprite icon;
    public bool isUnlocked;

    public SkillData(int id, string name, Sprite icon, bool isUnlocked = true)
    {
        this.id = id;
        this.skillName = name;
        this.icon = icon;
        this.isUnlocked = isUnlocked;
    }
}
