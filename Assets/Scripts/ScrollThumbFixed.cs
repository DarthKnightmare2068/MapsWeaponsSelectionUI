using UnityEngine;
using UnityEngine.UI;

public class ScrollThumbFixed : MonoBehaviour
{
    [SerializeField] private Scrollbar scrollbar;
    [SerializeField] private RectTransform handleRect; // Scrollbar Handle

    private float initialSize = 0.05f; // normalized size to apply on scene start
    private RectTransform scrollbarRect;

    private void Awake()
    {
        if (scrollbar == null) scrollbar = GetComponent<Scrollbar>();
        if (scrollbar != null && handleRect == null) handleRect = scrollbar.handleRect;
        if (scrollbar != null) scrollbarRect = scrollbar.GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        // run after layout so ScrollRect/Content sizes are valid
        StartCoroutine(InitNextFrame());
    }

    private void OnRectTransformDimensionsChange()
    {
        // Re-apply if layout/Resolution changes
        ApplyThumbSize();
    }

    private System.Collections.IEnumerator InitNextFrame()
    {
        // wait until end of the first rendered frame to ensure layout sizes are valid
        yield return new WaitForEndOfFrame();

        // if track width is still zero (layout not ready), poll a few frames
        int safety = 10;
        while (safety-- > 0)
        {
            if (scrollbarRect != null && scrollbarRect.rect.width > 0f) break;
            yield return null;
        }

        ApplyFromNormalized(initialSize);
    }

    private void ApplyThumbSize()
    {
        if (scrollbarRect == null || handleRect == null || scrollbar == null) return;
        ApplyFromNormalized(Mathf.Clamp01(initialSize));
    }

    // Exposed for Inspector events: set thumb size using normalized size (0..1)
    public void SetThumbSize(float normalizedSize)
    {
        ApplyFromNormalized(normalizedSize);
    }

    private void ApplyFromNormalized(float normalizedSize)
    {
        if (scrollbar == null) return;
        // Clamp and enforce a minimum so the handle never vanishes
        float size = Mathf.Clamp(normalizedSize, 0.01f, 1f);
        scrollbar.size = size; // let Scrollbar drive handle anchors/width itself

        // Ensure the value stays in range to avoid the handle moving off-track
        scrollbar.value = Mathf.Clamp01(scrollbar.value);
    }
}