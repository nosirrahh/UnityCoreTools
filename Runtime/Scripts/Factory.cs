using System.Collections.Generic;
using UnityEngine;

namespace NosirrahhTools.UnityCoreTools
{
    /// <summary>
    /// Represents a factory for managing and recycling objects of type T.
    /// </summary>
    /// <typeparam name="T">The type of object managed by the factory.</typeparam>
    [System.Serializable]
    public class Factory<T> where T : Object
    {
        #region Enumerations

        /// <summary>
        /// Defines the cast type for objects of type T.
        /// </summary>
        private enum CastType
        {
            /// <summary>
            /// No specific cast type is defined.
            /// </summary>
            None,

            /// <summary>
            /// The object is treated as a Component.
            /// </summary>
            Component,

            /// <summary>
            /// The object is treated as a GameObject.
            /// </summary>
            GameObject
        }

        #endregion

        #region Fields

        /// <summary>
        /// The cast type used for objects of type T.
        /// </summary>
        private CastType castType;

        /// <summary>
        /// The template object used for instantiating new objects.
        /// </summary>
        private T template;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the list of enabled elements managed by the factory.
        /// </summary>
        public List<T> EnabledElements { private set; get; }

        /// <summary>
        /// Gets the list of disabled elements managed by the factory.
        /// </summary>
        public List<T> DisabledElements { private set; get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Factory{T}"/> class with a template.
        /// </summary>
        /// <param name="template">The template object used for instantiation.</param>
        public Factory (T template)
        {
            this.template = template;
            EnabledElements = new List<T> ();
            DisabledElements = new List<T> ();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Retrieves an element from the factory.
        /// </summary>
        /// <param name="templateParent">The parent transform for the instantiated object.</param>
        /// <param name="setAsLastSibling">Determines if the instantiated object should be set as the last sibling.</param>
        /// <returns>The instantiated object.</returns>
        public T GetElement (Transform templateParent, bool setAsLastSibling = true)
        {
            SetCastType (template);

            T element = null;
            if (DisabledElements.Count > 0)
            {
                element = DisabledElements[0];
                SetElementParent (element, templateParent);
                DisabledElements.RemoveAt (0);
            }
            else
            {
                element = Object.Instantiate (template, templateParent);
            }
            EnabledElements.Add (element);
            if (setAsLastSibling)
                SetElementAsLastSibling (element);
            SetElementActive (element, true);
            return element;
        }

        /// <summary>
        /// Disables an element managed by the factory.
        /// </summary>
        /// <param name="elementToDisable">The element to disable.</param>
        public void DisableElement (T elementToDisable)
        {
            if (EnabledElements != null && DisabledElements != null)
            {
                bool removed = EnabledElements.Remove (elementToDisable);
                if (removed)
                {
                    SetElementActive (elementToDisable, false);
                    DisabledElements.Add (elementToDisable);
                }
            }
        }

        /// <summary>
        /// Disables all elements managed by the factory.
        /// </summary>
        public void DisableAllElements ()
        {
            if (EnabledElements != null && DisabledElements != null)
            {
                for (int i = 0; i < EnabledElements.Count; i++)
                    SetElementActive (EnabledElements[i], false);
                DisabledElements.AddRange (EnabledElements);
                EnabledElements.Clear ();
            }
        }

        /// <summary>
        /// Disables elements based on a condition.
        /// </summary>
        /// <param name="match">The condition used to disable elements.</param>
        public void DisableElements (System.Predicate<T> match)
        {
            if (EnabledElements != null && DisabledElements != null)
            {
                List<T> toDisable = new List<T> ();
                for (int i = 0; i < EnabledElements.Count; i++)
                    if (match.Invoke (EnabledElements[i]))
                        toDisable.Add (EnabledElements[i]);

                for (int i = 0; i < toDisable.Count; i++)
                {
                    SetElementActive (toDisable[i], false);
                    EnabledElements.Remove (toDisable[i]);
                }

                DisabledElements.AddRange (toDisable);
                toDisable.Clear ();
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Sets the cast type based on the provided template.
        /// </summary>
        /// <param name="template">The template for which the cast type will be determined.</param>
        private void SetCastType (T template)
        {
            if (castType == CastType.None)
            {
                if (typeof (Component).IsInstanceOfType (template))
                    castType = CastType.Component;
                else if (typeof (GameObject).IsInstanceOfType (template))
                    castType = CastType.GameObject;
                else
                    Debug.LogWarning ($"[{nameof (Factory<T>)}] The template type <{template.GetType ()}> is unexpected.");
            }
        }

        /// <summary>
        /// Sets the active state of the element based on the cast type.
        /// </summary>
        /// <param name="element">The element whose state will be set.</param>
        /// <param name="active">Indicates whether the element should be active.</param>
        private void SetElementActive (Object element, bool active)
        {
            if (castType == CastType.Component)
                (element as Component).gameObject.SetActive (active);
            else if (castType == CastType.GameObject)
                (element as GameObject).SetActive (active);
        }

        /// <summary>
        /// Sets the parent of the element based on the cast type.
        /// </summary>
        /// <param name="element">The element whose parent will be set.</param>
        /// <param name="parent">The parent transform.</param>
        private void SetElementParent (Object element, Transform parent)
        {
            if (castType == CastType.Component)
                (element as Component).transform.SetParent (parent, false);
            else if (castType == CastType.GameObject)
                (element as GameObject).transform.SetParent (parent, false);
        }

        /// <summary>
        /// Sets the element as the last sibling based on the cast type.
        /// </summary>
        /// <param name="element">The element to set as the last sibling.</param>
        private void SetElementAsLastSibling (Object element)
        {
            if (castType == CastType.Component)
                (element as Component).transform.SetAsLastSibling ();
            else if (castType == CastType.GameObject)
                (element as GameObject).transform.SetAsLastSibling ();
        }

        #endregion
    }
}
