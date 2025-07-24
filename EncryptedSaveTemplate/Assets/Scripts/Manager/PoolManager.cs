using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 미리 생성된 오브젝트 풀을 관리하고, 재사용 가능한 컴포넌트를 제공하는 풀 매니저 클래스
/// </summary>
[DisallowMultipleComponent]
public class PoolManager : SingletonMonobehaviour<PoolManager>
{
    [SerializeField] private Pool[] poolArray = null;
    private Transform objectPoolTransform;
    private Dictionary<int, Queue<Component>> poolDictionary = new Dictionary<int, Queue<Component>>();

    [System.Serializable]
    public struct Pool
    {
        public int poolSize;
        public GameObject prefab;
        public string componentType;
    }

    /// <summary>
    /// 시작 시 정의된 풀 배열에 따라 모든 풀을 생성
    /// </summary>
    private void Start()
    {
        objectPoolTransform = this.gameObject.transform;

        for (int i = 0; i < poolArray.Length; i++)
        {
            CreatePool(poolArray[i].prefab, poolArray[i].poolSize, poolArray[i].componentType);
        }
    }

    /// <summary>
    /// 지정된 프리팹과 타입으로 풀을 생성하고 비활성화된 인스턴스를 큐에 등록하는 함수
    /// </summary>
    /// <param name="prefab">풀링할 프리팹</param>
    /// <param name="poolSize">생성할 오브젝트 수</param>
    /// <param name="componentType">프리팹에서 사용할 컴포넌트의 타입 이름</param>
    private void CreatePool(GameObject prefab, int poolSize, string componentType)
    {
        int poolKey = prefab.GetInstanceID();

        string prefabName = prefab.name;

        GameObject parentGameobject = new GameObject(prefabName + "Anchor");

        parentGameobject.transform.SetParent(objectPoolTransform);

        if (!poolDictionary.ContainsKey(poolKey))
        {
            poolDictionary.Add(poolKey, new Queue<Component>());

            for (int i = 0; i < poolSize; i++)
            {
                GameObject newObject = Instantiate(prefab, parentGameobject.transform) as GameObject;

                newObject.SetActive(false);

                poolDictionary[poolKey].Enqueue(newObject.GetComponent(Type.GetType(componentType)));
            }
        }
    }

    /// <summary>
    /// 지정된 프리팹의 풀에서 컴포넌트를 꺼내 위치/회전을 설정한 후 반환하는 함수
    /// </summary>
    /// <param name="prefab">풀링할 프리팹</param>
    /// <param name="position">재사용 위치</param>
    /// <param name="rotaition">재사용 회전값</param>
    /// <returns>재사용 가능한 컴포넌트 인스턴스</returns>
    public Component ReuseComponent(GameObject prefab, Vector3 position, Quaternion rotaition)
    {
        int poolKey = prefab.GetInstanceID();

        if (poolDictionary.ContainsKey(poolKey))
        {
            Component componentToReuse = GetComponentFromPool(poolKey);

            ResetObject(position, rotaition, componentToReuse, prefab);

            return componentToReuse;
        }
        else
        {
            Debug.Log("No object pool for" + prefab);
            return null;
        }
    }

    /// <summary>
    /// 지정된 풀에서 컴포넌트를 하나 꺼내고 다시 큐에 넣는 함수
    /// </summary>
    /// <param name="poolKey">프리팹의 GetInstanceID로 생성된 키</param>
    /// <returns>풀에서 꺼낸 컴포넌트</returns>
    private Component GetComponentFromPool(int poolKey)
    {
        Component componentToReuse = poolDictionary[poolKey].Dequeue();
        poolDictionary[poolKey].Enqueue(componentToReuse);

        if (componentToReuse.gameObject.activeSelf == true)
        {
            componentToReuse.gameObject.SetActive(false);
        }

        return componentToReuse;
    }

    /// <summary>
    /// 재사용할 오브젝트의 위치/회전/스케일을 원본 프리팹 기준으로 초기화하는 함수
    /// </summary>
    /// <param name="position">설정할 위치</param>
    /// <param name="rotation">설정할 회전값</param>
    /// <param name="componentToReuse">초기화할 컴포넌트</param>
    /// <param name="prefab">원본 프리팹 참조 (스케일 기준)</param>
    private void ResetObject(Vector3 position, Quaternion rotation, Component componentToReuse, GameObject prefab)
    {
        componentToReuse.transform.position = position;
        componentToReuse.transform.rotation = rotation;
        componentToReuse.gameObject.transform.localScale = prefab.transform.localScale;
    }

    #region Validation
#if UNITY_EDITOR
    private void OnValidate()
    {
        HelperUtilities.ValidateCheckEnumerableValues(this, nameof(poolArray), poolArray);
    }
#endif
    #endregion
}