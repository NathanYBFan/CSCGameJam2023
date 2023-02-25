using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialScript : MonoBehaviour
{
    private void Start()
    {
        Time.timeScale = 0;
    }
    public void BackButton()
    {
        Time.timeScale = 1;
        Destroy(this.gameObject);
    }
}
