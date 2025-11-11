using UnityEngine;
using UnityEngine.Localization.Settings;
using System.Collections;

// Tiny helper for wiring Button OnClick to change language
// Attach this to a persistent object (e.g., Menu Canvas / SceneManager)
public class LocaleSetter : MonoBehaviour
{
    [SerializeField] private bool alwaysStartWithEnglish = true; // Always set English on scene start (ignores saved preference)
    private const string PlayerPrefsKey = "locale";
    private bool isInitialized = false;

    private void Start()
    {
        if (alwaysStartWithEnglish)
        {
            // Always start with English, regardless of saved preference
            StartCoroutine(InitializeAndSetEnglish());
        }
        else
        {
            // Load saved locale preference
            StartCoroutine(InitializeAndLoadSavedLocale());
        }
    }

    // Wait for LocalizationSettings to initialize, then always set English
    private IEnumerator InitializeAndSetEnglish()
    {
        // Wait for LocalizationSettings to be ready
        yield return LocalizationSettings.InitializationOperation;
        isInitialized = true;

        // Always set English on scene start (don't load saved preference)
        SetLocale("en-US");
    }

    // Wait for LocalizationSettings to initialize, then load saved locale
    private IEnumerator InitializeAndLoadSavedLocale()
    {
        // Wait for LocalizationSettings to be ready
        yield return LocalizationSettings.InitializationOperation;
        isInitialized = true;

        // Load saved locale if it exists, otherwise default to English
        if (PlayerPrefs.HasKey(PlayerPrefsKey))
        {
            string saved = PlayerPrefs.GetString(PlayerPrefsKey);
            if (!string.IsNullOrEmpty(saved))
            {
                SetLocale(saved);
            }
            else
            {
                SetLocale("en-US"); // Default to English if saved value is empty
            }
        }
        else
        {
            SetLocale("en-US"); // Default to English if no saved preference
        }
    }

    // Generic setter (use this in Button OnClick with a string parameter)
    public void SetLocale(string localeCode)
    {
        if (string.IsNullOrEmpty(localeCode)) return;

        // If not initialized yet, start coroutine to wait and then set
        if (!isInitialized)
        {
            StartCoroutine(SetLocaleAfterInit(localeCode));
            return;
        }

        var locale = LocalizationSettings.AvailableLocales.GetLocale(localeCode);
        if (locale != null)
        {
            LocalizationSettings.SelectedLocale = locale;
            PlayerPrefs.SetString(PlayerPrefsKey, localeCode);
            PlayerPrefs.Save();
            Debug.Log($"LocaleSetter: Locale changed to '{localeCode}'");
        }
        else
        {
            Debug.LogWarning($"LocaleSetter: Locale '{localeCode}' not found in AvailableLocales");
        }
    }

    // Helper coroutine to wait for initialization before setting locale
    private IEnumerator SetLocaleAfterInit(string localeCode)
    {
        yield return LocalizationSettings.InitializationOperation;
        isInitialized = true;
        SetLocale(localeCode); // Recursive call, but now isInitialized is true
    }

    // Convenience methods (optional) for Button OnClick without parameters
    public void SetEnglishUS() => SetLocale("en-US");
    public void SetVietnameseVN() => SetLocale("vi-VN");
}


