using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WolKwangChulHyeol.Core;

public class SkyStoneInfo : MonoBehaviour
{
    public GameObject[] bossKimSkyStone;
    public int bossKimSkyStoneMaxCount;
    public int bossKimSkyStoneCurCount;

    public int maxHealthStoneCount;
    public int notMaxHealthStoneCount;
    public int[] notMaxHealthStoneIndex;

    void Awake()
    {
        if(bossKimSkyStone.Length != 4)
        {
            Debug.LogError("비석의 갯수가 4개가 아닙니다.");
            Debug.Break();
        }

        bossKimSkyStoneMaxCount = transform.childCount;
        bossKimSkyStoneCurCount = bossKimSkyStoneMaxCount;

        notMaxHealthStoneIndex = new int[bossKimSkyStoneMaxCount];
    }

    // Update is called once per frame
    void Update()
    {
        bossKimSkyStoneCurCount = transform.childCount;

        maxHealthStoneCount = 0;

        for (int i = 0; i < bossKimSkyStoneMaxCount; i++)
        {
            notMaxHealthStoneIndex[i] = 0;
        }

        for(int i=0;i< bossKimSkyStoneMaxCount;i++)
        {
            if (bossKimSkyStone[i] != null)
            {
                if (bossKimSkyStone[i].GetComponent<Health>().IsFullHealth())
                {
                    maxHealthStoneCount++;
                }
                else
                {
                    notMaxHealthStoneIndex[i] = i + 1;
                }
            }
        }

        notMaxHealthStoneCount = bossKimSkyStoneCurCount - maxHealthStoneCount;

    }
    
}
