using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;
using System.Data.Common;

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
    [JsonIgnore] public Key.ResourcePrefs RewardResource;
    [JsonIgnore] public int RewardValue;

    [Header("Not Necessary"), JsonIgnore] public Building Building;
}

public class TaskController : MonoBehaviour
{
    [Header("Scripts")]
    [SerializeField] private UITaskController _ui;
    [SerializeField] private BuildingManager _buildingManager;

    [Header("Tasks")]
    [SerializeField] private List<Task> _tasks = new();

    private int _taskIndex = 0;
    private bool _isCanComplete = true;
    private bool _isAllTasksComplete;

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
        // ResetJSON();
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
        if (!_isCanComplete || _isAllTasksComplete)
            return;

        if (taskType == _tasks[_taskIndex].Type)
        {
            _tasks[_taskIndex].CurrentProgress += value;
            _ui.Slider.value = Util.CalculatePercentage(_tasks[_taskIndex].CurrentProgress, _tasks[_taskIndex].TotalProgress);
            _ui.RefreshUI(_tasks[_taskIndex]);
            Save();

            if (_tasks[_taskIndex].CurrentProgress >= _tasks[_taskIndex].TotalProgress)
            {
                _tasks[_taskIndex].IsCompleted = true;
                Save();

                _isCanComplete = false;

                Util.Invoke(this, () => ResourceController.AddResource(_tasks[_taskIndex].RewardResource, _tasks[_taskIndex].RewardValue), 2.2f);
                PointerHelper.Instance.ResetTarget();
                _ui.ShowCompleTask();
            }
        }
    }

    public void CompleteTask(TaskType taskType, Building building)
    {
        if (building == _tasks[_taskIndex].Building)
        {
            CompleteTask(taskType, 1);
        }
    }

    public void OnComplete()
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
                _ui.RefreshUI(_tasks[i]);

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

        _isAllTasksComplete = true;
        _ui.Hide();
    }

    private bool IsBuildingBuild(Building building)
    {
        foreach (BuildingRow row in _buildingManager.BuildingRows)
        {
            foreach (BuildingPoint buildingPoint in row.BuildingPoints)
            {
                if (buildingPoint.Building == building)
                {
                    return buildingPoint.IsBuild;
                }
            }
        }

        return false;
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
