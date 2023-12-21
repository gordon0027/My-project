using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Magic Skill", menuName = "Skills/Melee/New Magic Skill")]
public class MagicSkill : SkillScriptableObject
{
    public float damageAmount;
    public float skillRadius;
    public float skillInterval;
    
    

    private void Start()
    {
        skillType = SkillType.Magic;
    }

    
            
}
