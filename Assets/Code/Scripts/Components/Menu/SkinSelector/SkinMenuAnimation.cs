using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SkinAnimation : MonoBehaviour
{
    public Transform SkinTransform;

    [SerializeField] private float RotationSpeed = 1.0f;

    public void SetRotationAnimation(bool rotate)
    {
        if(rotate)
            StartCoroutine(RotationAnimation());
        else
            SkinTransform.rotation = Quaternion.identity;
    }

    IEnumerator RotationAnimation()
    {
        while(true)
        {
            SkinTransform.rotation = Quaternion.Euler(SkinTransform.rotation.x * Vector3.up * Time.deltaTime * RotationSpeed);
            yield return null;
        }
    }
}
