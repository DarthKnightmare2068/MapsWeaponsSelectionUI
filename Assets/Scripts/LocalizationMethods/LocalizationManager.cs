using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

// Centralized localization manager for all localization tables in the project
public static class LocalizationManager
{
	// Table names constants
	public const string TABLE_TITTLE_LABELS = "Tittle Labels";
	public const string TABLE_MAP_LABELS = "Map Labels";

	// Get localized string from any table using LocalizedString
	// Returns a LocalizedString object that can be subscribed to for auto-updates
	// Parameters: tableName - Name of the localization table, entryKey - Key of the localized entry
	// Returns: LocalizedString object
	public static LocalizedString GetLocalizedString(string tableName, string entryKey)
	{
		if (string.IsNullOrEmpty(tableName) || string.IsNullOrEmpty(entryKey))
		{
			Debug.LogWarning($"LocalizationManager: Invalid table name or entry key. Table: '{tableName}', Key: '{entryKey}'");
			return new LocalizedString(tableName ?? "", entryKey ?? "");
		}

		return new LocalizedString(tableName, entryKey);
	}

	// Get localized string from Tittle Labels table
	// Parameter: entryKey - Key of the localized entry
	// Returns: LocalizedString object
	public static LocalizedString GetTittleLabel(string entryKey)
	{
		return GetLocalizedString(TABLE_TITTLE_LABELS, entryKey);
	}

	// Get localized string from Map Labels table
	// Parameter: entryKey - Key of the localized entry
	// Returns: LocalizedString object
	public static LocalizedString GetMapLabel(string entryKey)
	{
		return GetLocalizedString(TABLE_MAP_LABELS, entryKey);
	}

	// Get localized string synchronously (one-time lookup, no subscription)
	// Use this for simple one-time lookups that don't need auto-updates
	// Parameters: tableName - Name of the localization table, entryKey - Key of the localized entry
	// Returns: Localized string or key if not found
	public static string GetLocalizedStringSync(string tableName, string entryKey)
	{
		if (string.IsNullOrEmpty(tableName) || string.IsNullOrEmpty(entryKey))
			return entryKey ?? "";

		try
		{
			return LocalizationSettings.StringDatabase.GetLocalizedString(tableName, entryKey);
		}
		catch
		{
			Debug.LogWarning($"LocalizationManager: Could not find key '{entryKey}' in table '{tableName}'");
			return entryKey; // Return key as fallback
		}
	}

	// Get localized string from Tittle Labels table synchronously
	// Parameter: entryKey - Key of the localized entry
	// Returns: Localized string or key if not found
	public static string GetTittleLabelSync(string entryKey)
	{
		return GetLocalizedStringSync(TABLE_TITTLE_LABELS, entryKey);
	}

	// Get localized string from Map Labels table synchronously
	// Parameter: entryKey - Key of the localized entry
	// Returns: Localized string or key if not found
	public static string GetMapLabelSync(string entryKey)
	{
		return GetLocalizedStringSync(TABLE_MAP_LABELS, entryKey);
	}

	// Change the current locale
	// Parameter: localeCode - Locale code (e.g., "en-US", "vi-VN")
	public static void SetLocale(string localeCode)
	{
		var locale = LocalizationSettings.AvailableLocales.GetLocale(localeCode);
		if (locale != null)
		{
			LocalizationSettings.SelectedLocale = locale;
			Debug.Log($"LocalizationManager: Locale changed to '{localeCode}'");
		}
		else
		{
			Debug.LogWarning($"LocalizationManager: Locale '{localeCode}' not found");
		}
	}

	// Get current locale code
	// Returns: Current locale code (e.g., "en-US", "vi-VN")
	public static string GetCurrentLocaleCode()
	{
		return LocalizationSettings.SelectedLocale != null
			? LocalizationSettings.SelectedLocale.Identifier.Code
			: "en-US";
	}

	// Check if a locale is available
	// Parameter: localeCode - Locale code to check
	// Returns: True if locale is available
	public static bool IsLocaleAvailable(string localeCode)
	{
		var locale = LocalizationSettings.AvailableLocales.GetLocale(localeCode);
		return locale != null;
	}

	// Get all available locale codes
	// Returns: Array of available locale codes
	public static string[] GetAvailableLocaleCodes()
	{
		var locales = LocalizationSettings.AvailableLocales.Locales;
		string[] codes = new string[locales.Count];
		for (int i = 0; i < locales.Count; i++)
		{
			codes[i] = locales[i].Identifier.Code;
		}
		return codes;
	}
}

