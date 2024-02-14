using System.Collections;
using UnityEditor;
using UnityEngine;
using static DeathTypeClass;

public class FlyingDeath : MonoBehaviour
{
    [Header("Death Path")]
    [SerializeField] Vector3 StartingPos = Vector3.forward;
    [SerializeField] Vector3 TargetToPickUpOffset = Vector3.zero;
    [SerializeField] Transform TargetToPickUp;
    [SerializeField] Vector3 EndingPos = Vector3.back;
    
    [Header("Death Time")]

    [Space]
    [Header("Death Object")]
    [SerializeField] Transform DeathObject;
    [Space]
    [Header("Sound")]
    [SerializeField] AnimationCurve SoundCurve;
    [SerializeField] float SoundCurveSpeed = 0.1f;
    [SerializeField] AudioSource AudioSource;
    [Space]
    [Header("Gizmo")]
    [SerializeField] bool ShowGismos = true;
    [SerializeField] Color GizmosColor = Color.magenta;

    private void Start()
    {
        AudioSource.Play();
    }

    private IEnumerator PickupPlayer()
    {
        float time = 0f;
        AudioSource.Play();
        while (time < 1f)
        {
            //DeathObject.position = Vector3.Lerp(StartingPos, TargetToPickUp.position + TargetToPickUpOffset, )
            AudioSource.volume = SoundCurve.Evaluate(time);
            time += Time.deltaTime * SoundCurveSpeed;
            yield return null;
        }
    }

    private void OnEnable()
    {
        PlayerManager.OnDeath += (DeathType deathType) =>
        {
            if (deathType == DeathType.Idling)
                StartCoroutine(PickupPlayer());
        };
    }

    private void OnDisable()
    {
        PlayerManager.OnDeath -= (DeathType deathType) => {};
    }

    private void OnDrawGizmos()
    {
        if (!ShowGismos)
            return;

        Handles.color = GizmosColor;
        Handles.DrawLine(StartingPos + TargetToPickUp.position, TargetToPickUp.position + TargetToPickUpOffset);
        Handles.DrawLine(TargetToPickUp.position + TargetToPickUpOffset, EndingPos + TargetToPickUp.position);
    }
}
