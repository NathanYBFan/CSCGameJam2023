using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using NaughtyAttributes;
public class darkenScreen : MonoBehaviour
{
    [SerializeField, Required] private Light2D global2DLight;
    [SerializeField, Range(0, 1)] private float lightIntensity;
    [SerializeField, Range(0, 1)] private float darkIntensity;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(LerpLightingDown(0));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(LerpLightingUp(0.5f));
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            global2DLight.intensity = darkIntensity;
    }

    public IEnumerator LerpLightingUp(float targetIntensity)
    {
        while (global2DLight.intensity <= targetIntensity)
        {
            global2DLight.intensity += 0.5f * Time.deltaTime;
            yield return null;
        }
    }
    public IEnumerator LerpLightingDown(float targetIntensity)
    {
        while (global2DLight.intensity >= targetIntensity)
        {
            global2DLight.intensity -= 0.5f * Time.deltaTime;
            yield return null;
        }
    }
}
