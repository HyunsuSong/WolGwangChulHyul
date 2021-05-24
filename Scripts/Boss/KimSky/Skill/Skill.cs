using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WolKwangChulHyeol.Core;

namespace HyunSu.Core
{
    public class Skill : MonoBehaviour
    {
        protected GameObject userObject;
        protected GameObject targetObject;
        protected bool applyAttribute = true;
        protected float baseSkillDamage = 0.0f;
        protected float skillDamage = 0.0f;
        protected int skillAttribute = 0;

        protected void Update()
        {
            if(userObject != null && userObject.GetComponent<HealthByStone>().GetIsDead())
            {
                StopAllCoroutines();
                Destroy(gameObject);
            }
        }

        protected void IntensifySkillDamage(float baseDamage, float multiple)
        {
            skillDamage = baseDamage * multiple;
        }
        protected void ApplyDamage(GameObject target, float damage)
        {
            if (target != null && target.GetComponent<Health>() != null)
            {
                target.GetComponent<Health>().TakeDamage(damage, true);
                Debug.Log(target.GetComponent<Health>().MHealthCurrentPoints);
            }

            if(applyAttribute)
            {
                userObject.GetComponent<sampleAttribute>().ApplyMyAttributeToTarget(skillAttribute, target);
            }
        }
        //protected void ApplyAttribute(GameObject target, int attributeState)
        //{
        //    if (target != null && target.GetComponent<sampleAttribute>() != null)
        //    {
        //        switch (attributeState)
        //        {
        //            case (int)attributeEnum.flame:
        //                target.GetComponent<sampleAttribute>().StartFlameDebuff();
        //                break;
        //            case (int)attributeEnum.electric:
        //                target.GetComponent<sampleAttribute>().StartEletricDebuff();
        //                break;
        //            case (int)attributeEnum.poision:
        //                target.GetComponent<sampleAttribute>().StartPoisionDebuff();
        //                break;
        //        }
        //    }
        //}

        protected void FindTarget(GameObject target)
        {
            if (target != null)
            {
                Vector3 direction = (new Vector3(target.transform.position.x, target.transform.position.y + target.transform.localScale.y, target.transform.position.z) - transform.position).normalized;

                Quaternion toRotation = Quaternion.LookRotation(direction);

                transform.rotation = toRotation;
            }
            else
            {
                Debug.Log("타겟 못찾음");
            }
        }
    }
}
