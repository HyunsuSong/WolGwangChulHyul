using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WolKwangChulHyeol.Core;

//공통 스크립트고, 모든 객체에게 동일하게 적용되므로 데이터 관리용 클래스를 만들고
//해당 클래스를 상속 받고, 기획쪽에서 부모 클래스의 값만 바꾸는 방향으로 해도 좋을 듯 함.
public class sampleAttribute : MonoBehaviour
{
    private enum attributeEnum { flame, electric, poision, holy };
    public int myAttributeState = 0;

    public float flameTimer = 0.0f;
    public float eletricTimer = 0.0f;
    public float poisionTimer = 0.0f;
    public float holyTimer = 0.0f;

    public bool debuffFlame = false;
    public bool debuffElectric = false;
    public bool debuffPoision = false;
    public bool buffHoly = false;

    public bool endFlameCoroutine = true;
    public bool endEletricCoroutine = true;
    public bool endPoisionCoroutine = true;
    public bool endHolyCoroutine = true;

    //List<float> attributeTimer = new List<float>();
    //List<bool> attributeState = new List<bool>();
    //List<bool> endAttributeCoroutine = new List<bool>();
    //List<IEnumerator> attributeDebuffIE = new List<IEnumerator>();

    private void Awake()
    {
        //attributeTimer.Add(flameTimer);
        //attributeTimer.Add(eletricTimer);
        //attributeTimer.Add(poisionTimer);
        //attributeTimer.Add(holyTimer);

        //attributeState.Add(debuffFlame);
        //attributeState.Add(debuffElectric);
        //attributeState.Add(debuffPoision);
        //attributeState.Add(buffHoly);

        //endAttributeCoroutine.Add(endFlameCoroutine);
        //endAttributeCoroutine.Add(endEletricCoroutine);
        //endAttributeCoroutine.Add(endPoisionCoroutine);
        //endAttributeCoroutine.Add(endHolyCoroutine);

        //attributeDebuffIE.Add(flameDebuff(5.0f, 1.0f));
        //attributeDebuffIE.Add(eletricDebuff(5.0f, 1.0f));
        //attributeDebuffIE.Add(poisionDebuff(5.0f, 1.0f));
        //attributeDebuffIE.Add(holyBuff());
    }

    void Update()
    {
        //화염이랑 똑같이 하면 됨.
        //코루틴은 중복 호출이 되지 않아야 하며 화염 지속시간동안은 계속 대미지를 받아야 함
        //3초 남았을 경우 같은 속성 공격을 받으면 추가가 아니라 초기화 되는 것이 통상적임.

        //for (int i = 0; i < 4; i++)
        //{
        //    if (attributeState[i])
        //    {
        //        attributeTimer[i] -= Time.deltaTime;

        //        if (attributeTimer[i] <= 0.0f)
        //        {
        //            attributeTimer[i] = 0.0f;
        //            attributeState[i] = false;
        //        }
        //        else
        //        {
        //            if (endAttributeCoroutine[i])
        //            {
        //                StartCoroutine(attributeDebuffIE[i]);
        //            }
        //        }
        //    }
        //}

        if (debuffFlame)
        {
            flameTimer -= Time.deltaTime;

            if (flameTimer <= 0.0f)
            {
                flameTimer = 0.0f;
                debuffFlame = false;
            }
            else
            {
                if (endFlameCoroutine)
                {
                    StartCoroutine(flameDebuff(5.0f, 1.0f));
                }
            }
        }

        if (debuffElectric)
        {
            eletricTimer -= Time.deltaTime;

            if (eletricTimer <= 0.0f)
            {
                eletricTimer = 0.0f;
                debuffElectric = false;
            }
            else
            {
                if (endEletricCoroutine)
                {
                    StartCoroutine(eletricDebuff(5.0f, 1.0f));
                }
            }
        }

        if (debuffPoision)
        {
            poisionTimer -= Time.deltaTime;

            if (poisionTimer <= 0.0f)
            {
                poisionTimer = 0.0f;
                debuffPoision = false;
            }
            else
            {
                if (endPoisionCoroutine)
                {
                    StartCoroutine(poisionDebuff(5.0f, 1.0f));
                }
            }
        }

        if (buffHoly)
        {
            holyTimer -= Time.deltaTime;

            if (holyTimer <= 0.0f)
            {
                holyTimer = 0.0f;
                buffHoly = false;
            }
            else
            {
                if (endHolyCoroutine)
                {
                    StartCoroutine(holyBuff());
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (debuffFlame)
        {
            if (collision.gameObject.GetComponent<sampleAttribute>() != null)
            {
                collision.gameObject.GetComponent<sampleAttribute>().debuffFlame = true;
                collision.gameObject.GetComponent<sampleAttribute>().flameTimer = 5.5f;
            }
        }
    }

    //용도는 KimSkyPassive 216번줄 참조하시길 바랍니다.
    public int GetAttributeEnum(string attributeName)
    {
        switch (attributeName)
        {
            case "flame":
                return (int)attributeEnum.flame;

            case "electric":
                return (int)attributeEnum.electric;

            case "poision":
                return (int)attributeEnum.poision;

            case "holy":
                return (int)attributeEnum.holy;

            
            default:
                Debug.LogError("속성 이름 오류");
                Debug.Break();
                return 99;
        }
    }

    //디버프 적용하는 함수, 내 스킬 또는 공격이 타겟에게 속성 적용이 되게 하고 싶다면 해당 함수를 사용하면 된다.
    public void ApplyMyAttributeToTarget(int userAttribute, GameObject targetObject)
    {
        if (targetObject.GetComponent<sampleAttribute>() != null)
        {
            switch(userAttribute)
            {
                case (int)attributeEnum.flame:
                    targetObject.GetComponent<sampleAttribute>().debuffFlame = true;
                    targetObject.GetComponent<sampleAttribute>().flameTimer = 5.5f;
                    break;

                case (int)attributeEnum.electric:
                    targetObject.GetComponent<sampleAttribute>().debuffElectric = true;
                    targetObject.GetComponent<sampleAttribute>().eletricTimer = 5.5f;
                    break;

                case (int)attributeEnum.poision:
                    targetObject.GetComponent<sampleAttribute>().debuffPoision = true;
                    targetObject.GetComponent<sampleAttribute>().poisionTimer = 5.5f;
                    break;

                //신성은 자버프이므로 적용하지 않음.
                case (int)attributeEnum.holy:
                    break;

                default:
                    Debug.Break();
                    Debug.LogError("속성 값 오류");
                    break;
            }
        }
    }

    //내 속성 변경 및 자버프인 경우 나에게 자버프 시간을 할당하는 함수
    //남에 의한 디버프는 위의 함수에서 해주지만, 자버프는 내가 직접 걸어야 함.
    public void ChangeMyAttributeBuff(int changeAttribute)
    {
        myAttributeState = changeAttribute;

        //스스로 걸 수 있는 속성에 관한 로직
        //내 속성에 따라서 내 스스로에게 적용되는 버프/디버프에 관함.
        switch (myAttributeState)
        {
            case (int)attributeEnum.holy:
                buffHoly = true;
                holyTimer = 5.5f;
                break;

            //자버프 효과가 안정해짐.
            default:
                break;
        }
    }

    //각 속성별 기능 추가될 수 있으므로, 따로 사용.

    IEnumerator flameDebuff(float  tickDamage, float tickTime)
    {
        endFlameCoroutine = false;

        while (debuffFlame)
        {
            GetComponent<Health>().TakeDamage(tickDamage, false);

            yield return new WaitForSeconds(tickTime);
        }
        endFlameCoroutine = true;
    }

    IEnumerator eletricDebuff(float tickDamage, float tickTime)
    {
        endEletricCoroutine = false;

        while (debuffElectric)
        {
            GetComponent<Health>().TakeDamage(tickDamage, false);

            yield return new WaitForSeconds(tickTime);
        }
        endEletricCoroutine = true;
    }

    IEnumerator poisionDebuff(float tickDamage, float tickTime)
    {
        endPoisionCoroutine = false;

        while (debuffPoision)
        {
            GetComponent<Health>().TakeDamage(tickDamage, false);

            yield return new WaitForSeconds(tickTime);
        }
        endPoisionCoroutine = true;
    }

    IEnumerator holyBuff()
    {
        endHolyCoroutine  = false;

        while (buffHoly)
        {
            yield return new WaitForSeconds(0.0f);
        }
        endHolyCoroutine = true;
    }
}
