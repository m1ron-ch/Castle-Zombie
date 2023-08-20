using UnityEngine;
using DG.Tweening;

public class InfiniteRotationAndMovement : MonoBehaviour
{
    public float moveDistance = 1f; 
    public float moveSpeed = 1f;
    public float rotationSpeed = 60f;

    private void Start()
    {
        StartAnimations();
    }

    private void StartAnimations()
    {
        Sequence moveSequence = DOTween.Sequence();
        moveSequence.Append(transform.DOLocalMoveY(transform.localPosition.y + moveDistance, moveSpeed).SetEase(Ease.OutQuad));
        moveSequence.Append(transform.DOLocalMoveY(transform.localPosition.y, moveSpeed).SetEase(Ease.InQuad));
        moveSequence.SetLoops(-1);

        transform.DOLocalRotate(new Vector3(0, 360, 0), (float)1.0f / rotationSpeed, RotateMode.LocalAxisAdd)
            .SetEase(Ease.Linear)
            .SetLoops(-1); 
    }
}
