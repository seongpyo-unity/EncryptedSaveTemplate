using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �̸� ������ ������Ʈ Ǯ�� �����ϰ�, ���� ������ ������Ʈ�� �����ϴ� Ǯ �Ŵ��� Ŭ����
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
    /// ���� �� ���ǵ� Ǯ �迭�� ���� ��� Ǯ�� ����
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
    /// ������ �����հ� Ÿ������ Ǯ�� �����ϰ� ��Ȱ��ȭ�� �ν��Ͻ��� ť�� ����ϴ� �Լ�
    /// </summary>
    /// <param name="prefab">Ǯ���� ������</param>
    /// <param name="poolSize">������ ������Ʈ ��</param>
    /// <param name="componentType">�����տ��� ����� ������Ʈ�� Ÿ�� �̸�</param>
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
    /// ������ �������� Ǯ���� ������Ʈ�� ���� ��ġ/ȸ���� ������ �� ��ȯ�ϴ� �Լ�
    /// </summary>
    /// <param name="prefab">Ǯ���� ������</param>
    /// <param name="position">���� ��ġ</param>
    /// <param name="rotaition">���� ȸ����</param>
    /// <returns>���� ������ ������Ʈ �ν��Ͻ�</returns>
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
    /// ������ Ǯ���� ������Ʈ�� �ϳ� ������ �ٽ� ť�� �ִ� �Լ�
    /// </summary>
    /// <param name="poolKey">�������� GetInstanceID�� ������ Ű</param>
    /// <returns>Ǯ���� ���� ������Ʈ</returns>
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
    /// ������ ������Ʈ�� ��ġ/ȸ��/�������� ���� ������ �������� �ʱ�ȭ�ϴ� �Լ�
    /// </summary>
    /// <param name="position">������ ��ġ</param>
    /// <param name="rotation">������ ȸ����</param>
    /// <param name="componentToReuse">�ʱ�ȭ�� ������Ʈ</param>
    /// <param name="prefab">���� ������ ���� (������ ����)</param>
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