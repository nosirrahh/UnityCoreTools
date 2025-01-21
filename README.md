# UnityCoreTools

...

## Installation
1. Open your Unity project.
2. Add the package via the Package Manager:
   ```
   https://github.com/nosirrahh/UnityCoreTools.git
   ```

## How to Use

These examples demonstrate how you can use the package scripts.

### Singleton
   ```csharp
   public class MyManager : Singleton<Manager>
   {
      public override void Initialize ()
      {
         Debug.Log("MyManager has been initialized!");
         base.Initilize();
      }
   }
   ```

### Factory
  ```csharp
   public class MyClass : MonoBehaviour
   {
      public GameObject template;
      public Transform templateParent;

      private Factory<GameObject> factory;

      private void Awake ()
      {
         factory = new Factory<GameObject> (template);
         GameObject element = factory.GetElement (templateParent);
         element.name = "MyFactoryElement";
      }
   }
   ```