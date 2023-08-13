using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ResourceStorage : MonoBehaviour
{
    [SerializeField] private Transform _resourcePrefab;
    [SerializeField] private List<Transform> _resourcesPoins = new();

    private List<Transform> _resources = new();
    private Key.ResourcePrefs _resourceKey;
    private int _valueOneResource;
    private int _resourcePointIndex = 0;

    #region MonoBehavior
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Player player))
        {
            TakeAllResources(player.transform);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        
    }
    #endregion

    public void Init(Key.ResourcePrefs resourceType, int valueOneCoin)
    {
        _resourceKey = resourceType;
        _valueOneResource = valueOneCoin;
    }

    public void AddResource()
    {
        Transform coin = Instantiate(_resourcePrefab, _resourcesPoins[_resourcePointIndex]);
        coin.SetParent(transform);
        _resources.Add(coin);
        _resourcePointIndex++;

        if (_resources.Count % _resourcesPoins.Count == 0)
        {
            foreach (Transform point in _resourcesPoins)
            {
                point.transform.position = new Vector3(point.transform.position.x, point.transform.position.y + 0.15f, point.transform.position.z);
            }

            _resourcePointIndex = 0;
        }
    }

    private void TakeAllResources(Transform player)
    {
        if (_resources.Count == 0)
            return;

        Vector3 jumpPosition = player.position;
        foreach (Transform resource in _resources)
            resource.transform.DOJump(jumpPosition, 2, 1, 0.5f)
                .OnComplete(() => Destroy(resource.gameObject));

        ResourceController.AddResource(_resourceKey, _valueOneResource * _resources.Count);
        SoundManager.Instance.PlayTakeResource();

        _resourcePointIndex = 0;
        _resources.Clear();
    }
}
