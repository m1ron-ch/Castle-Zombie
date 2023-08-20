using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public enum TaskType
{
    MiningResource,
    BuildStructure,
    DestroyEnemies,
    AccumulateCoins,
}

[System.Serializable]
public class Task
{
    [JsonIgnore] public UnityEngine.Sprite Sprite;
    [JsonIgnore] public TaskType Type;
    public int CurrentProgress;
    public int TotalProgress;
    [JsonIgnore] public string Description;
    public bool IsCompleted;
    [JsonIgnore] public List<Transform> TargetsForPointerHelper = new();

    [Header("Reward")]
    [JsonIgnore] public Key.ResourcePrefs Resource;
    [JsonIgnore] public int Count;

    [Header("Not Necessary"), JsonIgnore] public Building Building;
}

public class TaskController : MonoBehaviour
{
    [Header("Scripts")]
    [SerializeField] private BuildingManager _buildingManager;

    [Header("Task Panel")]
    [SerializeField] private UnityEngine.Sprite _completeIcon;
    [SerializeField] private UnityEngine.UI.Image _icon;
    [SerializeField] private UnityEngine.UI.Slider _slider;
    [SerializeField] private TMPro.TMP_Text _description;
    [SerializeField] private TMPro.TMP_Text _progress;
    
    [Header("Task Panels")]
    [SerializeField] private Transform _taskPanel;
    [SerializeField] private Transform _completeTaskPanel;

    [SerializeField, Header("Tasks")] private List<Task> _tasks = new();

    private int _taskIndex = 0;
    private bool _isCanComplete = true;

    private static TaskController s_instance;

    public static TaskController Instance => s_instance ?? null;
    public Task Task => _tasks[_taskIndex];

    #region MonoBehaviour
    private void Awake()
    {
        if (s_instance == null)
        {
            s_instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        ResetJSON();
        Load();

        ShowCurrentTask();
    }
    #endregion

    public void ResetJSON()
    {
        Save();
    }

    public void CompleteTask(TaskType taskType, int value)
    {
        if (!_isCanComplete)
            return;

        if (taskType == _tasks[_taskIndex].Type)
        {
            _tasks[_taskIndex].CurrentProgress += value;
            _slider.value = Util.CalculatePercentage(_tasks[_taskIndex].CurrentProgress, _tasks[_taskIndex].TotalProgress);
            RefreshProgress(_tasks[_taskIndex]);

            if (_tasks[_taskIndex].CurrentProgress >= _tasks[_taskIndex].TotalProgress)
            {
                PointerHelper.Instance.ResetTarget();
                ScaleOnCompleteSprite();
            }
        }
    }

    public void CompleteTask(TaskType taskType, Building building)
    {
        if (!_isCanComplete)
            return;

        if (taskType == _tasks[_taskIndex].Type ||
            building == _tasks[_taskIndex].Building)
        {
            _tasks[_taskIndex].CurrentProgress++;
            _slider.value = Util.CalculatePercentage(_tasks[_taskIndex].CurrentProgress, _tasks[_taskIndex].TotalProgress);
            RefreshProgress(_tasks[_taskIndex]);

            if (_tasks[_taskIndex].CurrentProgress == _tasks[_taskIndex].TotalProgress)
            {
                PointerHelper.Instance.ResetTarget();
                ScaleOnCompleteSprite();
            }
        }
    }

    private void OnComplete()
    {
        _isCanComplete = true;
        _tasks[_taskIndex].IsCompleted = true;
        ShowCurrentTask();
        Save();
    }

    private void ShowCurrentTask()
    {
        for (int i = 0; i < _tasks.Count; i++)
        {
            if (!_tasks[i].IsCompleted)
            {
                _taskIndex = i;
                RefreshUI(_tasks[i]);

                if (_tasks[_taskIndex].TargetsForPointerHelper.Any())
                {
                    Transform target = _tasks[_taskIndex].TargetsForPointerHelper.First();
                    PointerHelper.Instance.SetTarget(target);
                }
                else
                {
                    switch(_tasks[_taskIndex].Type)
                    {
                        case TaskType.DestroyEnemies:
                            Transform nearEnemy = EnemyManager.Instance.GetNearestEnemy().transform;
                            PointerHelper.Instance.SetTarget(nearEnemy.transform);
                            break;
                    }
                }

                if (IsBuildingBuild(_tasks[_taskIndex].Building))
                {
                    _tasks[_taskIndex].IsCompleted = true;
                    ShowCurrentTask();
                }

                return;
            }
        }

        _taskPanel.gameObject.SetActive(false);
    }

    private bool IsBuildingBuild(Building building)
    {
        foreach (BuildingRow row in _buildingManager.BuildingRows)
        {
            foreach (BuildingPoint buildingPoint in row.BuildingPoints)
            {
                if (buildingPoint.Building == building)
                {
                    if (buildingPoint.IsBuild)
                    {
                        return true;
                    }

                    PointerHelper.Instance.SetTarget(buildingPoint.Building.transform);
                }
            }
        }

        return false;
    }

    private void ScaleOnCompleteSprite()
    {
        _isCanComplete = false;

        _icon.transform.localScale = Vector3.zero;
        _icon.sprite = _completeIcon;

        _icon.transform.DOScale(Vector3.one, 0.6f)
            .SetEase(Ease.OutBack)
            .OnComplete(() => _taskPanel.transform.DOScale(0, 0.5f).SetDelay(1f)
                .OnComplete(() =>
                {
                    _completeTaskPanel.gameObject.SetActive(true);
                    _completeTaskPanel.transform.localScale = Vector3.zero;
                    _completeTaskPanel.transform.DOScale(Vector3.one, 0.6f)
                        .OnComplete(() =>
                        {
                            _completeTaskPanel.transform.DOScale(Vector3.zero, 0.5f)
                                .OnComplete(() =>
                                {
                                    _completeTaskPanel.gameObject.SetActive(false);
                                    _taskPanel.transform.localScale = Vector3.zero;
                                    _taskPanel.transform.DOScale(Vector3.one, 0.6f);
                                    OnComplete();
                                }).SetDelay(1f);
                        });
                })); 
    }

    private void RefreshUI(Task task)
    {
        _icon.sprite = task?.Sprite;
        _description.text = task?.Description;
        RefreshProgress(task);
        _slider.value = Util.CalculatePercentage(task.CurrentProgress, task.TotalProgress);
    }

    private void RefreshProgress(Task task)
    {
        _progress.text = $"{task.CurrentProgress} / {task.TotalProgress}";
    }

    private void Save()
    {
        string json = JsonConvert.SerializeObject(_tasks, Formatting.Indented);
        PlayerPrefs.SetString(Key.Prefs.Tasks.ToString(), json);
    }

    private void Load()
    {
        string json = PlayerPrefs.GetString(Key.Prefs.Tasks.ToString());
        if (json == "")
        {
            Save();
            return;
        }

        List<Task> tasks = JsonConvert.DeserializeObject<List<Task>>(json);

        if (_tasks.Count != tasks.Count)
            throw new System.Exception(" оличество tasks в JSON не совподают с текущим количеством tasks");

        for (int i = 0; i < _tasks.Count; i++)
        {
            _tasks[i].CurrentProgress = tasks[i].CurrentProgress;
            _tasks[i].IsCompleted = tasks[i].IsCompleted;
        }

        Debug.Log("Tasks Load()\n" + json);
    }
}
