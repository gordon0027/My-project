// МЕНЕДЖЕР СПОСОБНОСТЕЙ
// ДОБАВЛЕНИЕ СПОСОБНОСТЕЙ В СЛОТ AddSkill()

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SkillManager : MonoBehaviour
{
    public Transform skillPanel;
    public List<SkillSlot> skillSlots = new List<SkillSlot>();
    public MeleeSkill drunkPunchSkill; // SkillScriptableObject для DrunkPunch
    public MagicSkill vortexSkill;
    public MagicSkill frostRainSkill;
    private EnemyDamage enemyDamage; // Ссылка на компонент EnemyDamage
    private Coroutine drunkPunchCoroutine; // Добавлен флаг для корутины DrunkPunch
    private Coroutine vortexCoroutine; // Добавлен флаг корутины Vortex
    private Coroutine frostRainCoroutine; // Добавлен флаг корутины Vortex

    

    void Start()
    {
        enemyDamage = GetComponent<EnemyDamage>(); // Ссылка на скрипт атаки игрока где находятся все корутины скилов

        if (enemyDamage == null)
        {
            Debug.LogError("EnemyDamage component not found on the same GameObject as SkillManager.");
        }
        for(int i = 0; i < skillPanel.childCount; i++)
        {
            if(skillPanel.GetChild(i).GetComponent<SkillSlot>() != null)
            {
                skillSlots.Add(skillPanel.GetChild(i).GetComponent<SkillSlot>());
            }
        }
    }

    
    void Update()
    {
        CheckDrunkPunchSkill();
        CheckVortexSkill();
        CheckFrostRainSkill();
    }

    // DrunkPunch
    // Проверка наличии скила в слотах скилов и активация/деактивация корутины скила DrunkPunch
    private void CheckDrunkPunchSkill()
    {
        // Проверяем наличие навыка DrunkPunch в слотах скиллов
        bool hasDrunkPunchSkill = false;
        foreach (SkillSlot skillSlot in skillSlots)
        {
            if (skillSlot.skill == drunkPunchSkill)
            {
                hasDrunkPunchSkill = true;
                break;
            }
        }

        // Включаем или выключаем корутину DrunkPunch в зависимости от наличия навыка
        if (hasDrunkPunchSkill && drunkPunchCoroutine == null)
        {
            float skillInterval = drunkPunchSkill.skillInterval;
            float damageAmount = drunkPunchSkill.damageAmount;
            float skillRadius = drunkPunchSkill.skillRadius;

            drunkPunchCoroutine = StartCoroutine(enemyDamage.DrunkPunch(skillInterval, damageAmount, skillRadius));
        }
        else if (!hasDrunkPunchSkill && drunkPunchCoroutine != null)
        {
            StopCoroutine(drunkPunchCoroutine);
            drunkPunchCoroutine = null;
        }
    }

    // Vortex
    // Проверка наличии скила в слотах скилов и активация/деактивация корутины скила Vortex
    private void CheckVortexSkill()
    {
        // Проверяем наличие навыка Vortex в слотах скиллов
        bool hasVortexSkill = false;
        foreach (SkillSlot skillSlot in skillSlots)
        {
            if (skillSlot.skill == vortexSkill)
            {
                hasVortexSkill = true;
                break;
            }
        }

        // Включаем или выключаем корутину Vortex в зависимости от наличия навыка
        if (hasVortexSkill && vortexCoroutine == null)
        {
            float skillInterval = vortexSkill.skillInterval;
            float damageAmount = vortexSkill.damageAmount;
            float skillRadius = vortexSkill.skillRadius;

            vortexCoroutine = StartCoroutine(enemyDamage.Vortex(skillInterval, damageAmount, skillRadius));
        }
        else if (!hasVortexSkill && vortexCoroutine != null)
        {
            StopCoroutine(vortexCoroutine);
            vortexCoroutine = null;
        }
    }

    // Frost Rain
    // Проверка наличии скила в слотах скилов и активация/деактивация корутины скила Frost Rain
    private void CheckFrostRainSkill()
    {
        // Проверяем наличие навыка Vortex в слотах скиллов
        bool hasFrostRainSkill = false;
        foreach (SkillSlot skillSlot in skillSlots)
        {
            if (skillSlot.skill == frostRainSkill)
            {
                hasFrostRainSkill = true;
                break;
            }
        }

        // Включаем или выключаем корутину Vortex в зависимости от наличия навыка
        if (hasFrostRainSkill && frostRainCoroutine == null)
        {
            float skillInterval = frostRainSkill.skillInterval;
            float damageAmount = frostRainSkill.damageAmount;
            float skillRadius = frostRainSkill.skillRadius;

            frostRainCoroutine = StartCoroutine(enemyDamage.FrostRain(skillInterval, damageAmount, skillRadius));
        }
        else if (!hasFrostRainSkill && frostRainCoroutine != null)
        {
            StopCoroutine(frostRainCoroutine);
            frostRainCoroutine = null;
        }
    }


    private void AddSkill(SkillScriptableObject _skill, int _amount)
    {
        foreach(SkillSlot skillSlot in skillSlots)
        {
            if(skillSlot.skill == _skill)
            {
                skillSlot.amount += _amount;
                skillSlot.skillAmountText.text = skillSlot.amount.ToString();
                return;
            }
        }
        foreach(SkillSlot skillSlot in skillSlots)
        {
            if(skillSlot.isEmpty == true)
            {
                skillSlot.skill = _skill;
                skillSlot.amount = _amount;
                skillSlot.isEmpty = false;
                skillSlot.SetIcon(_skill.icon);
                skillSlot.skillAmountText.text = _amount.ToString();
                break;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
    if(other.gameObject.tag == "Skill")
    {
        Skill skillComponent = other.gameObject.GetComponent<Skill>();
        if (skillComponent != null)
        {
            AddSkill(skillComponent.skill, skillComponent.amount);
            Destroy(other.gameObject);
        }
    }
    }
}
