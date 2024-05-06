using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RG.Utils
{
    public class ObjectPool<T> where T : Component
    {
        private Queue<T> pool;
        private T prefab;
        private Transform parentTransform;

        public ObjectPool(T prefab, int initialSize, Transform parentTransform = null)
        {
            this.prefab = prefab;
            this.parentTransform = parentTransform;
            pool = new Queue<T>();

            for (int i = 0; i < initialSize; i++)
            {
                CreateObject();
            }
        }

        public T GetObject()
        {
            if (pool.Count == 0)
            {
                CreateObject();
            }
            T obj = pool.Dequeue();
            obj.gameObject.SetActive(true);
            return obj;
        }

        public void ReturnObject(T obj)
        {
            obj.gameObject.SetActive(false);
            pool.Enqueue(obj);
        }

        private T CreateObject()
        {
            T newObj = Object.Instantiate(prefab, parentTransform);
            newObj.gameObject.SetActive(false);
            pool.Enqueue(newObj);
            return newObj;
        }
    }
}