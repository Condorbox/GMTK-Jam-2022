using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public bool dontDestroy = false;

    static T mInstance;

    public static T Instance 
    {
        get
        {
            if(mInstance == null)
            {
                mInstance = GameObject.FindObjectOfType<T>();

                if(mInstance == null)
                {
                    GameObject singleton = new GameObject(typeof(T).Name);
                    mInstance = singleton.AddComponent<T>();
                }
            }

            return mInstance;
        }
    }

    public virtual void Awake()
    {
        if(mInstance == null)
        {
            mInstance = this as T;
            if (dontDestroy)
            {
                transform.parent = null;
                DontDestroyOnLoad(this.gameObject);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
