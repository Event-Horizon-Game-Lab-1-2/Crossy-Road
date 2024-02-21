using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Animator : MonoBehaviour
{
    public virtual IEnumerator StartAnimation()
    {
        yield return null;
    }
}
