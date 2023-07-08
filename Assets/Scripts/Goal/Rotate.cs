using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Rotate : MonoBehaviour
{
    void Start()
    {
        transform.DORotate(new Vector3(170, 170, 170), 2f).SetEase(Ease.InOutQuint).SetLoops(-1, LoopType.Yoyo);
    }
}
