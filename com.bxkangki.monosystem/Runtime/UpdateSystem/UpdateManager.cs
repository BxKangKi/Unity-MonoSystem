using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;

namespace MonoSystem
{
    public readonly struct UpdateManager
    {
        private static readonly List<IUpdate> updates = new List<IUpdate>(0);
        private static readonly List<IStartUpdate> startUpdates = new List<IStartUpdate>(0);
        private static readonly List<IEndUpdate> endUpdates = new List<IEndUpdate>(0);
        private static readonly List<IPreUpdate> preUpdates = new List<IPreUpdate>(0);
        private static readonly List<IPostUpdate> postUpdates = new List<IPostUpdate>(0);
        private static readonly List<IFixedUpdate> fixedUpdates = new List<IFixedUpdate>(0);
        private static readonly List<ILateUpdate> lateUpdates = new List<ILateUpdate>(0);

        private static CancellationTokenSource disable = new CancellationTokenSource();

        public static void Update()
        {
            for (int i = 0; i < startUpdates.Count; i++)
            {
                startUpdates[i].OnStartUpdate();
            }

            for (int i = 0; i < preUpdates.Count; i++)
            {
                preUpdates[i].OnPreUpdate();
            }

            for (int i = 0; i < updates.Count; i++)
            {
                updates[i].OnUpdate();
            }

            for (int i = 0; i < postUpdates.Count; i++)
            {
                postUpdates[i].OnPostUpdate();
            }

            for (int i = 0; i < endUpdates.Count; i++)
            {
                endUpdates[i].OnEndUpdate();
            }
        }

        public static void FixedUpdate()
        {
            for (int i = 0; i < fixedUpdates.Count; i++)
            {
                fixedUpdates[i].OnFixedUpdate();
            }
        }

        public static void LateUpdate()
        {
            for (int i = 0; i < lateUpdates.Count; i++)
            {
                lateUpdates[i].OnLateUpdate();
            }
        }


        public static void OnEnable()
        {
            CancellationExtension.Reset(ref disable);
        }

        public static void OnDisable()
        {
            CancellationExtension.Cancel(ref disable);
        }

        public static void Add<T>(T value) where T : IUpdateSystem
        {
            if (value is IUpdate)
            {
                AddUpdate<IUpdate>(updates, value as IUpdate).Forget();
            }
            if (value is IStartUpdate)
            {
                AddUpdate<IStartUpdate>(startUpdates, value as IStartUpdate).Forget();
            }
            if (value is IEndUpdate)
            {
                AddUpdate<IEndUpdate>(endUpdates, value as IEndUpdate).Forget();
            }
            if (value is IPreUpdate)
            {
                AddUpdate<IPreUpdate>(preUpdates, value as IPreUpdate).Forget();
            }
            if (value is IPostUpdate)
            {
                AddUpdate<IPostUpdate>(postUpdates, value as IPostUpdate).Forget();
            }
            if (value is IFixedUpdate)
            {
                AddUpdate<IFixedUpdate>(fixedUpdates, value as IFixedUpdate).Forget();
            }
            if (value is ILateUpdate)
            {
                AddUpdate<ILateUpdate>(lateUpdates, value as ILateUpdate).Forget();
            }
        }



        private static async UniTaskVoid AddUpdate<T>(List<T> list, T value)
        {
            await UniTask.DelayFrame(2, cancellationToken: disable.Token);
            if (!list.Contains(value))
            {
                list.Add(value);
            }
        }

        public static void Remove<T>(T value) where T : IUpdateSystem
        {
            if (value is IUpdate)
            {
                RemoveUpdate(updates, value as IUpdate);
            }

            if (value is IStartUpdate)
            {
                RemoveUpdate(startUpdates, value as IStartUpdate);
            }

            if (value is IEndUpdate)
            {
                RemoveUpdate(endUpdates, value as IEndUpdate);
            }

            if (value is IPreUpdate)
            {
                RemoveUpdate(preUpdates, value as IPreUpdate);
            }

            if (value is IPostUpdate)
            {
                RemoveUpdate(postUpdates, value as IPostUpdate);
            }

            if (value is IFixedUpdate)
            {
                RemoveUpdate(fixedUpdates, value as IFixedUpdate);
            }

            if (value is ILateUpdate)
            {
                RemoveUpdate(lateUpdates, value as ILateUpdate);
            }

        }


        private static void RemoveUpdate<T>(List<T> list, T value)
        {
            if (list.Contains(value))
            {
                list.Remove(value);
            }
        }
    }
}