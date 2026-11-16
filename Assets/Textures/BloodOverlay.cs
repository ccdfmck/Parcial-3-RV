using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BloodOverlay : MonoBehaviour
{
    public Image[] manchas;
    public Image pantallaNegra;
    public GameObject gameOverPanel;

    public float velocidadSangre = 1.5f;
    public float velocidadNegro = 2f;

    private void Start()
    {
        pantallaNegra.transform.SetAsLastSibling();

        // Sangre invisible al comenzar
        foreach (Image mancha in manchas)
        {
            Color color = mancha.color;
            color.a = 0f;
            mancha.color = color;
        }

        // Pantalla negra invisible
        Color negro = pantallaNegra.color;
        negro.a = 0f;
        pantallaNegra.color = negro;

        // Game Over invisible al comenzar
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        gameObject.SetActive(false);
    }

    public void MostrarSangre()
    {
        gameObject.SetActive(true);

        StopAllCoroutines();
        StartCoroutine(EfectoSangre());
    }

    private IEnumerator EfectoSangre()
    {
        // ───────────────
        // 1. APARECE LA SANGRE
        // ───────────────

        float alpha = 0f;

        while (alpha < 1f)
        {
            alpha += velocidadSangre * Time.unscaledDeltaTime;
            alpha = Mathf.Clamp01(alpha);

            foreach (Image mancha in manchas)
            {
                Color color = mancha.color;
                color.a = alpha;
                mancha.color = color;
            }

            yield return null;
        }

        // ───────────────
        // 2. PEQUEÑA PAUSA
        // ───────────────

        yield return new WaitForSecondsRealtime(0.5f);

        // ───────────────
        // 3. APARECE EL NEGRO
        // ───────────────

        Color negro = pantallaNegra.color;

        while (negro.a < 1f)
        {
            negro.a += velocidadNegro * Time.unscaledDeltaTime;
            negro.a = Mathf.Clamp01(negro.a);

            pantallaNegra.color = negro;

            yield return null;
        }

        // ───────────────
        // 4. NEGRO TOTAL
        // ───────────────

        negro.a = 1f;
        pantallaNegra.color = negro;

        // ───────────────
        // 5. GAME OVER
        // ───────────────

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            gameOverPanel.transform.SetAsLastSibling(); 
        }
    }
}