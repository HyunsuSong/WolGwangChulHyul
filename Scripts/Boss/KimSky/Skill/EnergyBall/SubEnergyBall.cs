using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HyunSu.Core;

public class SubEnergyBall : Skill
{
    [SerializeField]
    private GameObject[] effectsEnergyBall;
    [SerializeField]
    private GameObject[] effectsExplosion;

    private GameObject mainEnergyBall;
    private MainEnergyBall mainEnergyBallScript;
    private float moveSpeed = 10.0f;
    private float energyBallAngle = 0.0f;
    private float explosionArea = 0.0f;
    private int userObjectPhase = 0;
    private int ballNumber = 0;
    private bool isFire = false;
    private bool hitGround = false;
    private Vector3 hitGroundPos;

    private void Awake()
    {
        if (base.userObject == null)
        {
            base.userObject = GameObject.FindGameObjectWithTag("KimSky");
            base.skillAttribute = base.userObject.GetComponent<sampleAttribute>().myAttributeState;
            effectsEnergyBall[base.skillAttribute].SetActive(true);
        }
        if (base.targetObject == null)
        {
            base.targetObject = base.targetObject = GameObject.FindGameObjectWithTag("Player");
        }

        if (mainEnergyBall == null)
        {
            mainEnergyBall = FindObjectOfType<MainEnergyBall>().gameObject;
            mainEnergyBallScript = mainEnergyBall.GetComponent<MainEnergyBall>();
            moveSpeed = mainEnergyBallScript.MoveSpeed();
            energyBallAngle = mainEnergyBallScript.EnergyBallAngle();
            userObjectPhase = mainEnergyBallScript.UserObjectPhase();
            explosionArea = mainEnergyBallScript.ExplosionArea();
            base.skillDamage = mainEnergyBallScript.MySkillDamage();
        }

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

    public bool IsFire
    {
        get { return isFire; }
        set
        {
            base.FindTarget(base.targetObject);

            if (userObjectPhase != 4)
            {
                transform.Rotate(new Vector3(0.0f, -energyBallAngle * ballNumber, 0.0f), Space.World);
            }

            isFire = value;
        }
    }

    public int BallNumber
    {
        get { return ballNumber; }
        set { ballNumber = value; }
    }

    private void SkillMovement()
    {
        if(!IsFire)
        {
            if (userObjectPhase != 4)
            {
                transform.position = mainEnergyBall.transform.position;
            }
        }

        if (IsFire && !hitGround)
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
            base.ApplyDamage(hitObject.transform.gameObject, base.skillDamage);
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
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionArea);
    }
}
