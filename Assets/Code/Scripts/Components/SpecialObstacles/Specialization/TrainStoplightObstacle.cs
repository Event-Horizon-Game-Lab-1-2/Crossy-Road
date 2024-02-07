using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainStoplightObstacle: SpecialObstacle
{
    [SerializeField] Transform ObjectToShow = null;

    private void Awake()
    {
        ObjectToShow.gameObject.SetActive(false);
    }

    public override void Trigger()
    {
        ObjectToShow.gameObject.SetActive(true);
    }

    public override void Untrigger()
    {
        ObjectToShow.gameObject.SetActive(false);
    }
}
