using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HyunSu.Core;

public class ScytheAttack : Skill
{
    [SerializeField]
    private float moveSpeed = 10.0f;
    [SerializeField]
    private GameObject[] effectsScytheAttack;

    private void Awake()
    {
        if(userObject==null)
        {
            base.userObject = GameObject.FindGameObjectWithTag("KimSky");
            base.skillAttribute = userObject.GetComponent<sampleAttribute>().myAttributeState;
            effectsScytheAttack[base.skillAttribute].SetActive(true);
            base.baseSkillDamage = userObject.GetComponent<KimSkyPassive>().GetAttackPower();
            base.skillAttribute = userObject.GetComponent<sampleAttribute>().myAttributeState;
        }
        if (base.targetObject == null)
        {
            base.targetObject = GameObject.FindGameObjectWithTag("Player");
            base.FindTarget(base.targetObject);
        }

        SetSkillDamage();
        StartCoroutine(base.DestroySelf(5.0f));
    }

    private void Update()
    {
        base.Update();
        transform.Translate(transform.forward * moveSpeed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            base.ApplyDamage(other.gameObject, base.skillDamage, true);
            Destroy(gameObject);
        }
    }
    
    private void SetSkillDamage()
    {
        switch(userObject.GetComponent<HealthByStone>().GetPhase())
        {
            case 3:
                base.IntensifySkillDamage(base.baseSkillDamage, 1.5f);
                break;

            case 4:
                base.IntensifySkillDamage(base.baseSkillDamage, 2.0f);
                transform.localScale *= 2.0f;
                break;

            default:
                base.IntensifySkillDamage(base.baseSkillDamage, 1.0f);
                break;
        }
    }
}
