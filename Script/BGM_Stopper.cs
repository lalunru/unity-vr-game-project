using UnityEngine;

public class BGM_Stopper : MonoBehaviour
{
    void Start()
    {
        GameObject globalBGM = GameObject.Find("BGM_Manager");

        if (globalBGM != null)
        {
            Destroy(globalBGM);
        }
    }
}