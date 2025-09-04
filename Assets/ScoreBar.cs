using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBar : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] Image image;

    Coroutine routine;
    public void UpdateBar(float amount)
    {
        float cashAmount = Mathf.Clamp01(amount);
        if(routine != null)
        {
            StopCoroutine(routine);
        }
        routine = StartCoroutine(To(cashAmount));
    }

    IEnumerator To(float amount)
    {
        float current = image.fillAmount;
        float timer = speed;
        while (timer > 0)
        {
            yield return null;
            timer -= Time.deltaTime;
            image.fillAmount = Mathf.Lerp(current, amount, timer / speed);
        }
        image.fillAmount = Mathf.Lerp(current, amount, 1f);
        routine = null;
    }
}
