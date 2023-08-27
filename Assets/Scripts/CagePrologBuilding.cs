using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CagePrologBuilding : Building
{
    [SerializeField] private ParticleSystem _smoke;
    [SerializeField] private List<PlayerAI> _hostages = new();

    #region MonoBehaviour
    private void Awake()
    {
        Show();
    }
    #endregion

    public override void Build()
    {
        TaskController.Instance.CompleteTask(TaskType.BuildStructure, this);

        transform.SetParent(null);
        transform.DOScale(Vector3.zero, 0.75f)
            .OnStart(() =>
            {
                _smoke.Play();
                FreeHostage();
            })
            .OnComplete(() => Hide());
    }

    private void FreeHostage()
    {
        foreach (PlayerAI hostage in _hostages)
        {
            hostage.transform.SetParent(null);
            hostage.Active();
        }
    }
}
