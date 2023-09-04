using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CagePrologBuilding : Building
{
    [SerializeField] private ParticleSystem _smoke;
    [SerializeField] private List<PlayerAI> _hostages = new();
    [SerializeField] private List<Transform> _moveToPoints = new();

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
        for (int i = 0; i < _hostages.Count; i++)
        {
            _hostages[i].transform.SetParent(null);
            _hostages[i].Active();
            _hostages[i].MoveTo(_moveToPoints[i].position);
        }
    }
}
