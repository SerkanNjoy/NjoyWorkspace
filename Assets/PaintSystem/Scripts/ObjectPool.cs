using System.Collections.Generic;
using UnityEngine;

namespace SeroJob.ObjectPooling
{
    public class ObjectPool<T>
    {
        private readonly GameObject _poolItemPref;
        private readonly Transform _poolHolder;

        private readonly int _poolStartSize;

        private readonly List<T> _pool = new List<T>();

        private int _poolSize;

        public ObjectPool(GameObject itemPref, Transform poolHolder, int startSize)
        {
            _poolItemPref = itemPref;
            _poolHolder = poolHolder;
            _poolStartSize = startSize;
            _poolSize = 0;
            _pool = new List<T>();

            PushCachedItems();
            FillRemainingItems();
        }

        private void PushCachedItems()
        {
            int step = _poolHolder.childCount;

            for(int i = 0; i < step; i++)
            {
                var item = _poolHolder.GetChild(0);
                PushItem(item.gameObject, true);
            }
        }

        private void FillRemainingItems()
        {
            if (_poolSize >= _poolStartSize) return;

            int step = _poolStartSize - _poolSize;
            for(int i = 0; i < step; i++)
            {
                SpawnNewItem();
            }
        }

        private void SpawnNewItem()
        {
            var item = Object.Instantiate(_poolItemPref, _poolHolder);
            PushItem(item, false);
        }

        public void PushItem(GameObject item, bool setParent = true)
        {
            _poolSize++;
            _pool.Add(item.GetComponent<T>());
            item.SetActive(false);
            if(setParent) item.transform.SetParent(_poolHolder, false);
        }

        public T Pull()
        {
            if (_poolSize <= 0) SpawnNewItem();

            var item = _pool[0];
            _poolSize--;
            _pool.RemoveAt(0);
            return item;
        }
    }
}