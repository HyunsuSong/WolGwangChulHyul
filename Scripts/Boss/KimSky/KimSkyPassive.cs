using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WolKwangChulHyeol.Core;

public class KimSkyPassive : MonoBehaviour
{
    [SerializeField]
    private float changeTimeFirstPhase = 30.0f;
    [SerializeField]
    private float changeTimeThirdPhase = 20.0f;
    [SerializeField]
    private float changeTimeFourthPhase = 15.0f;
    [SerializeField]
    private float forceChangeFirstPhase = 120.0f;
    [SerializeField]
    private float forceChangeThirdPhase = 90.0f;
    [SerializeField]
    private float forceChangeFourthPhase = 60.0f;
    [SerializeField]
    private const int successPersentage = 25;

    private KimSkyAnimList bossKimSkyAnimList;
    private SkyStoneInfo skyStoneInfo;
    //
    public float skyAttackPower = 50.0f;
    private float skyAttackPowerTempByHoly = 50.0f;

    private bool canUpdate = false;

    private bool isBerserk = false;
    private bool isRealBerserk = false;
    private float healTimer = 0.0f;

    private float changeAttributeTimer = 0.0f;
    private float changeTime = 0.0f;
    private float failSeconds = 0.0f;
    private float forceChangeSeconds = 0.0f;
    
    //패시브는 아니지만 버프/디버프도 패시브로 들어가기 때문에
    //패시브 스크립트에서 속성별 버프 디버프를 넣는 것으로 해야 할 듯 함.

    private void Awake()
    {
        if (GetComponent<KimSkyAnimList>() != null)
        {
            bossKimSkyAnimList = GetComponent<KimSkyAnimList>();
        }
        else
        {
            Debug.Break();
        }

        if (GameObject.Find("KimSkyStoneInfo") != null)
        {
            skyStoneInfo = GameObject.Find("KimSkyStoneInfo").GetComponent<SkyStoneInfo>();
        }
        else
        {
            Debug.Break();
        }

        skyAttackPowerTempByHoly = skyAttackPower;

        CheckAttribute();
        SetChangeAttributeStatus();
    }

    void Update()
    {
        if (canUpdate)
        {
            PlayBerserk();
            ChangeAttribute();
        }
    }

    public float GetAttackPower()
    {
        return skyAttackPower;
    }

    public bool CanUpdate
    {
        get { return canUpdate; }
        set { canUpdate = value; }
    }

    public bool IsBerserk
    {
        get { return isBerserk; }
        set { isBerserk = value; }
    }
    public bool IsRealBerserk
    {
        get { return isRealBerserk; }
        set { isRealBerserk = value; }
    }

    private void PlayBerserk()
    {
        switch (GetComponent<HealthByStone>().GetPhase())
        {
            case 3:
                if (!IsBerserk)
                {
                    IntensifyStatus();
                    bossKimSkyAnimList.StartCoroutine("AnimBerserk");
                    IsBerserk = true;
                }
                HealPerSec(0.5f);
                break;

            case 4:
                if (!IsRealBerserk)
                {
                    IntensifyStatus();
                    bossKimSkyAnimList.StartCoroutine("AnimBerserk");
                    IsRealBerserk = true;
                }
                HealPerSec(1.0f);
                break;

            default:
                break;
        }
    }

    private void HealPerSec(float healGage)
    {
        healTimer += Time.deltaTime;

        if (skyStoneInfo.notMaxHealthStoneCount != 0 && healTimer >= 1.0f)
        {
            for (int i = 0; i < skyStoneInfo.bossKimSkyStoneMaxCount; i++)
            {
                if (skyStoneInfo.notMaxHealthStoneIndex[i] != 0)
                {
                    skyStoneInfo.bossKimSkyStone[skyStoneInfo.notMaxHealthStoneIndex[i] - 1].GetComponent<Health>().MHealthCurrentPoints += (healGage / skyStoneInfo.notMaxHealthStoneCount);
                }
            }

            healTimer = 0.0f;
        }
    }

    private void IntensifyStatus()
    {
        skyAttackPower = skyAttackPowerTempByHoly;
        skyAttackPower += 25.0f;
        skyAttackPowerTempByHoly = skyAttackPower;

        for (int i = 0; i < skyStoneInfo.bossKimSkyStoneMaxCount; i++)
        {
            if (skyStoneInfo.bossKimSkyStone[i] != null)
            {
                skyStoneInfo.bossKimSkyStone[i].GetComponent<Health>().MHealthMaxPoints += (200.0f / Mathf.Pow(skyStoneInfo.bossKimSkyStoneCurCount, 2));
                skyStoneInfo.bossKimSkyStone[i].GetComponent<Health>().MHealthCurrentPoints += (200.0f / Mathf.Pow(skyStoneInfo.bossKimSkyStoneCurCount, 2));
            }
        }
    }

    private void SetChangeAttributeStatus()
    {
        switch(GetComponent<HealthByStone>().GetPhase())
        {
            case 3:
                changeTime = changeTimeThirdPhase;
                forceChangeSeconds = forceChangeThirdPhase;
                break;

            case 4:
                changeTime = changeTimeFourthPhase;
                forceChangeSeconds = forceChangeFourthPhase;
                break;

            default:
                changeTime = changeTimeFirstPhase;
                forceChangeSeconds = forceChangeFirstPhase;
                break;
        }
    }

    private void CheckAttribute()
    {
        int randomAttribute = Random.Range(0, 4);
        GetComponent<sampleAttribute>().myAttributeState = randomAttribute;

        if (GetComponent<sampleAttribute>().myAttributeState == GetComponent<sampleAttribute>().GetAttributeEnum("holy"))
        {
            GetComponent<sampleAttribute>().holyTimer = Mathf.Infinity;
            skyAttackPower = skyAttackPowerTempByHoly * 1.25f;
        }
        else
        {
            GetComponent<sampleAttribute>().holyTimer = 0.0f;
            skyAttackPower = skyAttackPowerTempByHoly;

        }
    }

    private void ChangeAttribute()
    {
        changeAttributeTimer += Time.deltaTime;
        failSeconds += Time.deltaTime;

        if (changeAttributeTimer >= changeTime || failSeconds >= forceChangeSeconds)
        {
            changeAttributeTimer = 0.0f;
            failSeconds = 0.0f;

            int randomPersentage = Random.Range(0, 100);

            if (randomPersentage <= successPersentage || failSeconds >= forceChangeSeconds)
            {
                int randomAttribute = 0;

                do
                {
                    randomAttribute = Random.Range(0, 4);

                } while (GetComponent<sampleAttribute>().myAttributeState == randomAttribute);

                bossKimSkyAnimList.StartCoroutine("AnimChangeAttribute");
                GetComponent<sampleAttribute>().ChangeMyAttributeBuff(randomAttribute);
                CheckAttribute();
            }
        }
    }
}