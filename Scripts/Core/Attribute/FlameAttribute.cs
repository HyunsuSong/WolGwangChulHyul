using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameAttribute : MonoBehaviour, IAttribute
{
    public void Use()
    {
        Debug.Log("Flame");
    }
}
