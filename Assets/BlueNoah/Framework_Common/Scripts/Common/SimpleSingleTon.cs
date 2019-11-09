using UnityEngine;

public class SimpleSingleTon<T> where T : new()
{
    private static T t ;

    public static T Instance
    {
        get
        {
            if (t == null)
            {
                t = new T();
            }
            return t;
        }
    }
}
