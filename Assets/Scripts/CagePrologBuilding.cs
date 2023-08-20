using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CagePrologBuilding : Building
{
    [SerializeField] private List<Transform> _hostages = new();
    #region MonoBehaviour
    private void Awake()
    {
        Show();
    }
    #endregion

    public override void Build()
    {
        TaskController.Instance.CompleteTask(TaskType.BuildStructure, this);

        foreach (Transform hostage in _hostages)
            hostage?.SetParent(null);

        transform.SetParent(null);
        transform.localScale = Vector3.one;
        transform.DOScale(Vector3.zero, 0.5f)
            .OnComplete(() => Hide());
    }
}
