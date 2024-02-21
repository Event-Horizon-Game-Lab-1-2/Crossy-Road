using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using static DeathTypeClass;

public class FlyingDeath : MonoBehaviour
{
    [Header("Death Path")]
    [SerializeField] Vector3 StartingPos = Vector3.forward;
    [SerializeField] Vector3 TargetToPickUpOffset = Vector3.zero;
    [Tooltip("Is the object picked up by toothless")]
    [SerializeField] Transform TargetToPickUp;
    [SerializeField] Vector3 EndingPos = Vector3.back;
    [SerializeField] float DeathSpeed = 1.5f;
    [Space]
    [Header("Death Object")]
    [Tooltip("Toothless >:)")]
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

    private void Awake()
    {
        DeathObject.gameObject.SetActive(false);
    }

    private IEnumerator PickupPlayer()
    {
        DeathObject.gameObject.SetActive(true);
        AudioSource.Play();
        transform.position = TargetToPickUp.position;
        float progress = 0f;
        float audioProgress = 0f;
        //move
        while (progress <= 1f)
        {
            //position
            DeathObject.position = Vector3.Lerp(TargetToPickUp.position + StartingPos, TargetToPickUp.position + TargetToPickUpOffset, progress);
            //audio
            audioProgress = Mathf.Lerp(0, 0.5f, progress);
            AudioSource.volume = SoundCurve.Evaluate(audioProgress);
            //update progress
            progress += Time.deltaTime * DeathSpeed;
            yield return null;
        }
        //pick up player
        progress = 0f;
        Vector3 objectToPickUpStartpos = TargetToPickUp.position + TargetToPickUpOffset;
        while (progress <= 1f)
        {
            //position
            DeathObject.position = Vector3.Lerp(objectToPickUpStartpos, EndingPos, progress);
            TargetToPickUp.position = DeathObject.position - TargetToPickUpOffset;
            //audio
            audioProgress = Mathf.Lerp(0.5f, 1f, progress);
            AudioSource.volume = SoundCurve.Evaluate(audioProgress);
            //update progress
            progress += Time.deltaTime * DeathSpeed;
            yield return null;
        }

        DeathObject.gameObject.SetActive(false);
        TargetToPickUp.gameObject.SetActive(false);
    }

    private void Die(DeathType deathType)
    {
        if (deathType == DeathType.Idling)
            StartCoroutine(PickupPlayer());
    }

    private void OnEnable()
    {
        PlayerManager.OnDeath += Die;
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        PlayerManager.OnDeath -= Die;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (!ShowGismos)
            return;

        Handles.color = GizmosColor;
        if (TargetToPickUp == null) 
        {
            return;
        }
        Handles.DrawLine(StartingPos + TargetToPickUp.position, TargetToPickUp.position + TargetToPickUpOffset);
        Handles.DrawLine(TargetToPickUp.position + TargetToPickUpOffset, EndingPos + TargetToPickUp.position);
    }
#endif
}
