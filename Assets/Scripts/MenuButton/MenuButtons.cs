using UnityEngine;

public class MenuButtons : MonoBehaviour
{
    [SerializeField] private GameObject menuPanel; // Panel that must start active

    private void Start()
    {
        if (menuPanel == null) return;

        // Enable only menuPanel; disable other top-level children under this Canvas
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            if (child == null) continue;
            child.gameObject.SetActive(child.gameObject == menuPanel);
        }
    }

    // Turn OFF one panel and ON another (use via Button OnClick)
    public void SwitchPanel(GameObject fromPanel, GameObject toPanel)
    {
        if (fromPanel != null) fromPanel.SetActive(false);
        if (toPanel != null) toPanel.SetActive(true);
    }

    // Show a panel and hide the rest (single-parameter Button OnClick)
    public void ShowPanel(GameObject toPanel)
    {
        if (toPanel == null) return;

        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            if (child == null) continue;
            child.gameObject.SetActive(child.gameObject == toPanel);
        }
    }
}
