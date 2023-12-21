using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SkillType {Melee, Range, Magic}
public class SkillScriptableObject : ScriptableObject
{
    public Sprite icon;
    public SkillType skillType;
    public string skillName;
    public string skillDescription;
    public int maximumAmount;
    
}
