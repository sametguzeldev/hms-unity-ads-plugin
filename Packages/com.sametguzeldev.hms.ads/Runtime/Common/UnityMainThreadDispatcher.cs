using System;
using System.Collections.Concurrent;
using UnityEngine;

namespace SametGuzelDev.HMS.Ads
{
    /// <summary>
    /// Dispatches <see cref="Action"/> callbacks from background threads (e.g., Java callbacks)
    /// to the Unity main thread. Automatically initialised before the first scene loads.
    /// </summary>
    [AddComponentMenu("")]   // Hidden from Add Component menu
    public sealed class UnityMainThreadDispatcher : MonoBehaviour
    {
        private static UnityMainThreadDispatcher _instance;
        private readonly ConcurrentQueue<Action> _queue = new ConcurrentQueue<Action>();

        /// <summary>
        /// Automatically creates the dispatcher GameObject before the first scene loads.
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            if (_instance != null) return;

            var go = new GameObject("[HMS Ads] MainThreadDispatcher");
            DontDestroyOnLoad(go);
            _instance = go.AddComponent<UnityMainThreadDispatcher>();
        }

        /// <summary>
        /// Enqueues an <paramref name="action"/> to be executed on the Unity main thread
        /// during the next <c>Update</c> frame. Safe to call from any thread.
        /// </summary>
        /// <param name="action">The action to dispatch. Must not be null.</param>
        public static void Enqueue(Action action)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));

            if (_instance == null)
            {
                Debug.LogWarning("[HMS Ads] UnityMainThreadDispatcher is not initialized. " +
                                 "The action will be dropped. Ensure RuntimeInitializeOnLoadMethod has run.");
                return;
            }

            _instance._queue.Enqueue(action);
        }

        private void Update()
        {
            while (_queue.TryDequeue(out var action))
            {
                try
                {
                    action.Invoke();
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
        }
    }
}
