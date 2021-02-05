using System.Linq;
using UnityEngine;
 
public class AutoReferencer<T> : MonoBehaviour where T : AutoReferencer<T> {
 
   #if UNITY_EDITOR
   // This method is called once when we add component do game object
   protected new virtual void Reset()
   {
       // Magic of reflection
       // For each field in your class/component we are looking only for those that are empty/null
       foreach (var field in typeof(T).GetFields().Where(field => field.GetValue(this) == null))
       {
           // Now we are looking for object (self or child) that have same name as a field
           Transform obj;
           if (transform.name == field.Name)
           {
               obj = transform;
           }
           else
           {
               obj = transform.Find(field.Name); // Or you need to implement recursion to looking into deeper childs
           }
 
           // If we find object that have same name as field we are trying to get component that will be in type of a field and assign it
           if (obj!=null)
           {
               field.SetValue(this, obj.GetComponent(field.FieldType));
           }
       }
   }
   #endif
}