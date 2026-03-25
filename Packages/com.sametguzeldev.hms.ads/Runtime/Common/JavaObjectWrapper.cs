using System;
#if UNITY_ANDROID && !UNITY_EDITOR
using UnityEngine;
#endif

namespace SametGuzelDev.HMS.Ads
{
    /// <summary>
    /// Base wrapper for Android Java objects. Holds an <c>AndroidJavaObject</c> reference
    /// and handles safe JNI disposal via <see cref="IDisposable"/>.
    /// </summary>
    public abstract class JavaObjectWrapper : IDisposable
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        /// <summary>The underlying Android Java object.</summary>
        protected AndroidJavaObject JavaObject { get; private set; }

        private bool _disposed;

        /// <param name="javaObject">The AndroidJavaObject to wrap. Must not be null.</param>
        protected JavaObjectWrapper(AndroidJavaObject javaObject)
        {
            JavaObject = javaObject ?? throw new ArgumentNullException(nameof(javaObject));
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>Override to release additional managed/unmanaged resources.</summary>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;
            if (disposing)
            {
                JavaObject?.Dispose();
                JavaObject = null;
            }
            _disposed = true;
        }

        /// <summary>Finalizer — disposes unmanaged JNI resources.</summary>
        ~JavaObjectWrapper() => Dispose(false);
#else
        /// <inheritdoc/>
        public void Dispose() { }
#endif
    }
}
