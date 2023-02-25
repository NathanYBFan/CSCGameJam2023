using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Checkpoint : MonoBehaviour
{

    [SerializeField] private GameObject monster;
    [SerializeField] private bool isEnd;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHandler._PlayerHandlerInstance.SetSpawnPoint(this.transform);
            Debug.Log("Collided with a checkpoint");
            
            if(monster != null)
            {
                monster.gameObject.SetActive(false);
            }

            if (isEnd)
            {
                SceneManager.LoadScene("MainMenu"); //change to be win screen
            }
        }
    }
}
