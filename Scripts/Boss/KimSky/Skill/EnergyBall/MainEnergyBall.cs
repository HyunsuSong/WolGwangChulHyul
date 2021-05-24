using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WolKwangChulHyeol.Core;
using HyunSu.Core;

public class MainEnergyBall : Skill
{
    [SerializeField]
    private float moveSpeed = 10.0f;
    [SerializeField]
    private float energyBallAngle = 30.0f;
    [SerializeField]
    private GameObject prefabSubEnergyBall;
    [SerializeField]
    private GameObject[] effectsEnergyBall;
    [SerializeField]
    private GameObject[] effectsExplosion;

    private GameObject[] subEnergyBall;
    private GameObject slot;
    private GameObject slotFourthPhase;

    private int userObjectPhase = 0;
    private int rotationPerSubBalls = 0;
    private float explosionArea = 0.0f;
    private float mySkillDamage = 0.0f;
    private bool isFire = false;
    private bool hitGround = false;
    private Vector3 hitGroundPos;

    private void Awake()
    {
        if (userObject == null)
        {
            base.userObject = GameObject.FindGameObjectWithTag("KimSky");
            skillAttribute = userObject.GetComponent<sampleAttribute>().myAttributeState;
            effectsEnergyBall[base.skillAttribute].SetActive(true);
            base.baseSkillDamage = base.userObject.GetComponent<KimSkyPassive>().GetAttackPower();
            userObjectPhase = base.userObject.GetComponent<HealthByStone>().GetPhase();
            rotationPerSubBalls = (((userObjectPhase - 1) / 2) + 1);
        }
        if (base.targetObject == null)
        {
            base.targetObject = GameObject.FindGameObjectWithTag("Player");
        }

        slot = GameObject.Find("slotEnergyBall").gameObject;
        slotFourthPhase = GameObject.Find("slotFourthEnergyBall").gameObject;

        SetSkillDamage();
        SetSkillLogic();

        StartCoroutine(DestroySelf(5.0f));
    }

    private void Update()
    {
        base.Update();
        SkillMovement();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (!hitGround)
            {
                base.ApplyDamage(collision.gameObject, base.skillDamage);
                base.userObject.GetComponent<KimSky>().AttackSuccess();
                Destroy(gameObject);
            }
        }

        if (collision.gameObject.tag == "Finish")
        {
            hitGroundPos = collision.contacts[0].point;
        }
    }

    public GameObject EffectsEnergyBall(int index)
    {
        return effectsEnergyBall[index];
    }

    public GameObject EffectsExplosion(int index)
    {
        return effectsExplosion[index];
    }

    public float MoveSpeed()
    {
        return moveSpeed;
    }

    public int UserObjectPhase()
    {
        return userObjectPhase;
    }

    public float EnergyBallAngle()
    {
        return energyBallAngle;
    }

    public float ExplosionArea()
    {
        return explosionArea;
    }

    public float MySkillDamage()
    {
        return mySkillDamage;
    }

    public bool IsFire
    {
        get { return isFire; }
        set 
        {
            base.FindTarget(base.targetObject);

            if (userObjectPhase != 4)
            {
                transform.Rotate(new Vector3(0.0f, energyBallAngle * rotationPerSubBalls, 0.0f), Space.World);
            }

            isFire = value;
        }
    }

    private void SetSkillDamage()
    {
        switch (userObjectPhase)
        {
            case 3:
                base.IntensifySkillDamage(base.baseSkillDamage, 1.5f);
                break;

            case 4:
                base.IntensifySkillDamage(base.baseSkillDamage, 2.0f);
                break;

            default:
                base.IntensifySkillDamage(base.baseSkillDamage, 1.0f);
                break;
        }

        mySkillDamage = base.skillDamage;
    }

    private void SetSkillLogic()
    {
        explosionArea = transform.localScale.x * 2.0f;

        if (userObjectPhase == 4)
        {
            transform.position = slotFourthPhase.transform.position;
            transform.rotation = slotFourthPhase.transform.rotation;
        }

        CreateSubBalls(rotationPerSubBalls * 2);
    }

    private void CreateSubBalls(int ballCount)
    {
        subEnergyBall = new GameObject[ballCount];

        for (int i = 0; i < subEnergyBall.Length; i++)
        {
            subEnergyBall[i] = Instantiate(prefabSubEnergyBall, transform.position, transform.rotation);
            
            if (userObjectPhase == 4)
            {
                subEnergyBall[i].transform.Rotate(new Vector3(0.0f, 0.0f, -45.0f * (i + 1)), Space.Self);
                subEnergyBall[i].transform.Translate(subEnergyBall[i].transform.up * userObject.transform.localScale.x *2, Space.World);

                if (i == subEnergyBall.Length - 1)
                {
                    transform.Translate(transform.up * userObject.transform.localScale.x*2, Space.World);
                }
            }
            else
            {
                subEnergyBall[i].GetComponent<SubEnergyBall>().BallNumber = (rotationPerSubBalls - subEnergyBall.Length + 1) + i;
            }
        }
    }

    private void SkillMovement()
    {
        if (!IsFire) //대기상태
        {
            if (userObjectPhase != 4)
            {
                transform.position = slot.transform.position;
            }
        }

        if (IsFire && !hitGround) //발사 상태
        {
            transform.Translate(transform.forward * moveSpeed * Time.deltaTime, Space.World);
        }

        if (!hitGround && hitGroundPos != Vector3.zero)
        {
            if (transform.position.y <= hitGroundPos.y + 2.0f)
            {
                hitGround = true;
                ExplosionEnergyBall();
            }
        }
    }

    private void ExplosionEnergyBall()
    {
        transform.GetComponent<SphereCollider>().enabled = false;
        effectsEnergyBall[base.skillAttribute].SetActive(false);
        effectsExplosion[base.skillAttribute].SetActive(true);

        RaycastHit[] rayHits = Physics.SphereCastAll(transform.position, explosionArea, Vector3.up, 0.0f, LayerMask.GetMask("Player"));

        foreach (RaycastHit hitObject in rayHits)
        {
            base.ApplyDamage(hitObject.transform.gameObject, base.skillDamage / 2.0f);
            base.userObject.GetComponent<KimSky>().AttackSuccess();
        }

        StartCoroutine(DestroySelf(1.8f));
    }

    IEnumerator DestroySelf(float delay)
    {
        yield return new WaitForSeconds(delay);

        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, explosionArea);
    }
}