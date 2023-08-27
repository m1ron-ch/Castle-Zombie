using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using Unity.VisualScripting;

public class UITaskController : MonoBehaviour
{
    [Header("Task Panel")]
    [SerializeField] private UnityEngine.Sprite _completeIcon;
    [SerializeField] private Image _taskIcon;
    [SerializeField] private Slider _slider;
    [SerializeField] private TMP_Text _description;
    [SerializeField] private TMP_Text _progressInSlider;

    [Header("Reward Panel")]
    [SerializeField] private Image _rewardIcon;
    [SerializeField] private TMP_Text _reward;

    [Header("Panels")]
    [SerializeField] private Transform _taskPanel;
    [SerializeField] private Transform _rewardTaskPanel;

    public Slider Slider => _slider;

    public void ShowTask()
    {
        if (_taskPanel.gameObject.activeInHierarchy)
            return;

        _taskPanel.gameObject.SetActive(true);
        _taskPanel.transform.localScale = Vector3.zero;
        _taskPanel.transform.DOScale(Vector3.one, 0.6f)
            .SetEase(Ease.OutBack);
    }

    public void ShowCompleTask()
    {
        _taskIcon.transform.localScale = Vector3.zero;
        _taskIcon.sprite = _completeIcon;

        _taskIcon.transform.DOScale(Vector3.one, 0.6f)
            .SetEase(Ease.OutBack)
            .OnComplete(() => HideTask());
    }

    public void RefreshUI(Task task)
    {
        ShowTask();

        _taskIcon.sprite = task?.Sprite;
        _description.text = task?.Description;
        RefreshProgress(task);
        _slider.value = Util.CalculatePercentage(task.CurrentProgress, task.TotalProgress);

        RefreshReward(task);
    }

    public void HideTask(float delay = 1)
    {
        _taskPanel.transform.DOScale(0, 0.5f)
            .SetDelay(delay)
            .OnComplete(() =>
            {
                _taskPanel.gameObject.SetActive(false);
                ShowReward();
            });
    }

    public void Show()
    {
        transform.gameObject.SetActive(true);
    }

    public void Hide()
    {
        transform.gameObject.SetActive(false);
    }

    private void RefreshProgress(Task task)
    {
        _progressInSlider.text = $"{task.CurrentProgress} / {task.TotalProgress}";
    }

    private void RefreshReward(Task task)
    {
        _reward.text = $"+{task.RewardValue}";
        _rewardIcon.sprite = Sprite.Instance.GetSprite(task.RewardResource);
    }

    private void ShowReward()
    {
        _rewardTaskPanel.gameObject.SetActive(true);
        _rewardTaskPanel.transform.localScale = Vector3.zero;
        _rewardTaskPanel.transform.DOScale(Vector3.one, 0.6f)
            .OnComplete(() => HideReward());
    }

    private void HideReward(float delay = 1)
    {
        _rewardTaskPanel.transform.DOScale(Vector3.zero, 0.5f)
            .SetDelay(delay)
            .OnComplete(() => TaskController.Instance.OnComplete());
    }

}
