using System.Collections;
using UnityEngine;

public class SquashAndStretch : MonoBehaviour
{
    [SerializeField]
    private Transform playerSprite;

    [SerializeField]
    private bool toggle;

    public void SquashStretch(float xScale, float yScale, float time)
    {
        if (toggle)
        {
            StartCoroutine(SandS(xScale, yScale, time));
        }
    }

    private IEnumerator SandS(float xScale, float yScale, float time)
    {
        playerSprite.localScale = new Vector2(xScale, yScale);
        float elapsedTime = 0f;
        while (elapsedTime < time)
        {
            float x = Mathf.Lerp(xScale, 1f, elapsedTime / time);
            float y = Mathf.Lerp(yScale, 1f, elapsedTime / time);
            playerSprite.localScale = new Vector2(x, y);
            elapsedTime += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        playerSprite.localScale = new Vector2(1f, 1f);
        yield return null;
    }
}
