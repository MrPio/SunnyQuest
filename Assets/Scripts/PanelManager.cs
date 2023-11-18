using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelManager : MonoBehaviour
{
    private void Start()
    {
        if (Application.platform is not (RuntimePlatform.Android or RuntimePlatform.IPhonePlayer))
            gameObject.SetActive(false);
    }
}
