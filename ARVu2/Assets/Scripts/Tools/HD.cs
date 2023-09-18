using UnityEngine;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace HD.Math
{
    class Math
    {
        //Add function
        public int Add(int number1, int number2)
        {
            return number1 + number2;
        }

        public float Add(float number1, float number2)
        {
            return number1 + number2;
        }
    }
}

//useing Pool reference: https://github.com/andy091045/poolTest
namespace HD.Pooling
{
    //Abstract pooling
    public interface IPool<T>
    {
        T GetInstance();
    }

    //ListPool
    class ListPool<T> : IPool<T> where T : class
    {
        Func<T> produce;
        int capacity;
        List<T> objects;
        Func<T, bool> useTest;
        bool expandable;

        public ListPool(Func<T> factoryMethod, int maxSize, Func<T, bool> inUse, bool expandable = true)
        {
            produce = factoryMethod;
            capacity = maxSize;
            objects = new List<T>(maxSize);
            useTest = inUse;
            this.expandable = expandable;
        }

        public T GetInstance()
        {
            var count = objects.Count;
            foreach (var item in objects)
            {
                if (!useTest(item))
                {
                    return item;
                }
            }
            if (count >= capacity && !expandable)
            {
                return null;
            }
            var obj = produce();
            objects.Add(obj);
            return obj;
        }
    }

    //QueuePool
    class QueuePool<T> : IPool<T>
    {
        Func<T> produce;
        int capacity;
        T[] objects;
        int index;

        public QueuePool(Func<T> factoryMethod, int maxSize)
        {
            produce = factoryMethod;
            capacity = maxSize;
            index = -1;
            objects = new T[maxSize];
        }

        public T GetInstance()
        {
            //stuff
            index = (index + 1) % capacity;

            if (objects[index] == null)
            {
                objects[index] = produce();
            }

            return objects[index];
        }
    }
}

namespace HD.Singleton
{
    public class TSingletonMonoBehavior<T> : MonoBehaviour where T : MonoBehaviour
    {

        public static T Instance => GetInstance();
        private static T instance = null;

        private static T GetInstance()
        {
            if (instance == null)
            {
                var type = typeof(T);
                var gameObject = new GameObject(type.Name);
                instance = gameObject.AddComponent<T>();
                DontDestroyOnLoad(gameObject);
            }
            return instance;
        }

        void Awake()
        {
            if (instance == null) instance = this as T;
            if (instance == this) DontDestroyOnLoad(this);
            else
            {
                DestroyImmediate(gameObject);
                return;
            }
            init();
        }

        protected virtual void init()
        {
        }

        //protected virtual void OnDestroy() => instance = null;
    }
}

namespace HD.FindObject
{
    public class Find
    {
        //前面放入要找的物?名?，後面放入要指?物?底下類別的已宣?類別
        public void FindObject<T>(string name, out T component) where T : Component
        {
            GameObject target = GameObject.Find(name);
            if (target != null)
            {
                component = target.GetComponent<T>();
            }
            else
            {
                Debug.LogError($"Can't find {name}");
                component = null;
            }
        }
    }
}

namespace HD.GetAddressablesItem
{
    public class GetGameObjectByPath
    {
        //使用方法: var obj = await GetGameObjectByPath.GetItem<GameObject>(path);
        public static async Task<T> GetItem<T>(string path) where T : UnityEngine.Object
        {
            AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(path);
            await handle.Task;
            return handle.Result;
        }
    }
}

namespace HD.FrameworkDesign
{
    public class Event<T> where T : Event<T>
    {
        private static Action mOnEvent;

        public static void Register(Action onEvent)
        {
            mOnEvent += onEvent;
        }

        public static void UnRegister(Action onEvent)
        {
            mOnEvent -= onEvent;
        }

        public static void Trigger()
        {
            mOnEvent?.Invoke();
        }
    }

    public class BindableProperty<T> where T : IEquatable<T>
    {
        private T mValue = default(T);

        public T Value
        {
            get { return mValue; }
            set
            {
                if (!value.Equals(mValue))
                {
                    mValue = value;
                    OnValueChanged?.Invoke(value);
                }
            }
        }
        public Action<T> OnValueChanged;
    }

    public class OnPropertyChange<T>
    {
        private Func<T> getter_;

        private T lastValue_;
        public Action<T> OnValueChanged;

        public OnPropertyChange(Func<T> getter)
        {
            getter_ = getter;
            lastValue_ = getter_.Invoke();
        }

        public void Update()
        {
            if (!lastValue_.Equals(getter_.Invoke()))
            {
                lastValue_ = getter_.Invoke();
                OnValueChanged?.Invoke(lastValue_);
            }
        }
    }

    public class IOCContainer
    {
        private Dictionary<Type, object> mInstances = new Dictionary<Type, object> ();

        public void Register<T>(T instance)
        {
            var key = typeof(T);

            if(mInstances.ContainsKey(key))
            {
                mInstances[key] = instance;
            }
            else
            {
                mInstances.Add(key, instance);
            }
        }

        public T Get<T>() where T : class
        {
            var key = typeof (T);   

            if(mInstances.TryGetValue(key, out var retInstance))
            {
                return retInstance as T;
            }

            return null;
        }
    }

    public abstract class Architecture<T> where T : Architecture<T>, new()
    {
        private static T mArchitecture;

        static void MakeSureArchitecture()
        {
            if(mArchitecture == null)
            {
                mArchitecture = new T();
                mArchitecture.Init();
            }
        }

        protected abstract void Init(); 

        private IOCContainer mContainer = new IOCContainer();

        public static T Get<T>() where T : class
        {
            MakeSureArchitecture();

            return mArchitecture.mContainer.Get<T>();
        }

        public void Register<T>(T instance)
        {
            MakeSureArchitecture();

            mArchitecture.mContainer.Register<T>(instance); 
        }
    }

}

