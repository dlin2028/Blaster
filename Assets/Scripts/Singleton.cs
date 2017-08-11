using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton : MonoBehaviour {
    public static GameObject Instance;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
