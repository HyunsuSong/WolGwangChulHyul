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

        //구조 바꾸기.
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
        protected void ApplyDamage(GameObject target, float damage, bool isHit)
        {
            if (target != null && target.GetComponent<Health>() != null)
            {
                if(target.GetComponent<Health>().TakeDamage(damage, isHit))
                {
                    userObject.GetComponent<KimSky>().AttackSuccess();
                }
            }

            if(applyAttribute)
            {
                userObject.GetComponent<sampleAttribute>().ApplyMyAttributeToTarget(skillAttribute, target);
            }
        }

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

        protected IEnumerator DestroySelf(float delay)
        {
            yield return new WaitForSeconds(delay);

            Destroy(gameObject);
        }
    }
}
