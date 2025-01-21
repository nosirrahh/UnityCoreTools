using UnityEngine;

namespace NosirrahhTools.UnityCoreTools
{
    /// <summary>
    /// Generic implementation of a Singleton for MonoBehaviour classes in Unity.
    /// </summary>
    /// <typeparam name="T">The type of the class to be treated as a Singleton.</typeparam>
    public class Singleton<T> : MonoBehaviour where T : Component
    {
        #region Fields

        /// <summary>
        /// Holds the Singleton instance.
        /// </summary>
        private static T instance;

        #endregion

        #region Properties

        /// <summary>
        /// <para>Returns the unique instance of the class.</para>
        /// <para>If no instance exists, it tries to find one in the scene.</para>
        /// <para>If none is found, a new instance is created.</para>
        /// </summary>
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType (typeof (T)) as T;
                    if (instance == null)
                    {
                        GameObject singleton = new GameObject (typeof (T).ToString (), typeof (T));
                        instance = singleton.GetComponent<T> ();
                    }
                    (instance as Singleton<T>).Initialize ();
                }
                return instance;
            }
        }

        /// <summary>
        /// Indicates whether the Singleton has been initialized.
        /// </summary>
        public bool IsInitialized { get; private set; }

        #endregion

        #region Unity Methods

        /// <summary>
        /// Called when the instance is destroyed.
        /// Resets the instance to null.
        /// </summary>
        protected virtual void OnDestroy ()
        {
            instance = null;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Initialization method called when the instance is created for the first time.
        /// </summary>
        /// <remarks>
        /// Can be overridden in derived classes to add specific initializations.
        /// </remarks>
        public virtual void Initialize ()
        {
            IsInitialized = true;
        }

        #endregion
    }
}
