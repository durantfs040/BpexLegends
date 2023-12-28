using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class rightWarningManager : MonoBehaviour
{
    public static rightWarningManager instance;
    int i = 0;
    bool activated = false;
    public RawImage img;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        img.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        i = (i + 1) % 3;
        if (i == 0 && activated)
        {
            img.enabled = !img.enabled;
            activated = false;
        }
        else if (i == 0)
        {
            img.enabled = false;
        }
    }

    public void activate()
    {
        activated = true;
    }
}
