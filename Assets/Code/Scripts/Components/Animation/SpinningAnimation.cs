using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinningAnimation : MonoBehaviour
{


    [SerializeField] Transform ObjectTransform;
    [SerializeField] float RotationSpeed = 1f;
    private float RotationY = -180f;

    private void FixedUpdate()
    {
        ObjectTransform.transform.rotation = Quaternion.Euler (0f, RotationY, 0f);
        RotationY += Time.fixedDeltaTime * RotationSpeed;
    }

}


