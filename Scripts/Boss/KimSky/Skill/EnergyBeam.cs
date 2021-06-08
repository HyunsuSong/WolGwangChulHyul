using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WolKwangChulHyeol.Core;
using HyunSu.Core;

public class EnergyBeam : Skill
{
    public GameObject[] effectsEnergyBeam;

    private CapsuleCollider beamCollider;
    private Vector3 beamColliderSize;

    private bool isHIt;

    private float betweenLength;

    [SerializeField]
    private float followSpeed = 1.0f;

    private float recordMyEulerY = 0.0f;
    private float recordMyEulerZ = 0.0f;
    private Vector3 direction;
    private Vector3 myEulerAngles;
    private Quaternion lookForTarget;

    public float MyOldEulerAngleX { get; set; }

    private void Awake()
    {
        if (base.userObject == null)
        {
            base.userObject = GameObject.FindGameObjectWithTag("KimSky");
            base.skillAttribute = userObject.GetComponent<sampleAttribute>().myAttributeState;
            base.baseSkillDamage = userObject.GetComponent<KimSkyPassive>().GetAttackPower();
        }
        if (base.targetObject == null)
        {
            base.targetObject = GameObject.FindGameObjectWithTag("Player");
        }

        beamCollider = GetComponent<CapsuleCollider>();

        SetSkillDamage();
    }

    private new void Update()
    {
        base.Update();

        LookRotationX(base.targetObject.transform);
        //최적화적 측면에서 굳이 계산해줄 필요 없을 듯함.
        //CheckAttackLength();

        if (isHIt)
        {
            base.ApplyDamage(base.targetObject, base.skillDamage * Time.deltaTime, false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            isHIt = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            isHIt = false;
        }
    }

    private void SetSkillDamage()
    {
        switch (userObject.GetComponent<HealthByStone>().GetPhase())
        {
            case 2:
                base.IntensifySkillDamage(base.baseSkillDamage, 1.5f);
                break;

            case 3:
                base.IntensifySkillDamage(base.baseSkillDamage, 1.5f);
                break;

            case 4:
                base.IntensifySkillDamage(base.baseSkillDamage, 1.5f);
                break;

            default:
                base.IntensifySkillDamage(base.baseSkillDamage, 1.5f);
                break;
        }
    }

    private void LookRotationX(Transform target)
    {
        direction = target.position - transform.position;
        lookForTarget = Quaternion.LookRotation(direction);
        recordMyEulerY = transform.eulerAngles.y;
        recordMyEulerZ = transform.eulerAngles.z;
        transform.rotation = Quaternion.Slerp(transform.rotation, lookForTarget, Time.deltaTime * followSpeed);

        myEulerAngles.x = transform.eulerAngles.x;
        myEulerAngles.y = recordMyEulerY;
        myEulerAngles.z = recordMyEulerZ;
        transform.eulerAngles = myEulerAngles;
    }

    private void CheckAttackLength()
    {
        betweenLength = (base.targetObject.transform.position - transform.position).magnitude;
        beamCollider.height = betweenLength / 6.0f;

        beamColliderSize.x = beamCollider.center.x;
        beamColliderSize.y = beamCollider.center.y;
        beamColliderSize.z = beamCollider.height / 2.0f;
        beamCollider.center = beamColliderSize;
    }
}
