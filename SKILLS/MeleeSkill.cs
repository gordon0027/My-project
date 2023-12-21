using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Melee Skill", menuName = "Skills/Melee/New Melee Skill")]
public class MeleeSkill : SkillScriptableObject
{
    public float damageAmount;
    public float skillRadius;
    public float skillInterval;
    
    

    private void Start()
    {
        skillType = SkillType.Melee;
       
        
    }

    
            
}

