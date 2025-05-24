using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Google;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using UnityEditor;
using UnityEditor.Localization;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;

namespace Editor.GoogleSheetsSync
{
	public class LocalizationGoogleSync
	{
		private const string CredentialsPath = "credentials.json";
		private static string spreadsheetId = "1MTkBRy4FQl0oj0msXwdDSnMUA13f6NREQOP6GCNYyCE";

		[MenuItem("Tools/Localization Sync/Push to Google Sheets")]
		private static void PushToGoogleSheets()
		{
			try
			{
				var service = GetSheetsService();
				var tables = GetAllStringTables(); 

				foreach (var table in tables)
				{
					var sheetName = SanitizeSheetName(table[0].TableCollectionName);
					Debug.Log($"Processing table: {table[0].TableCollectionName} -> Sheet: {sheetName}");

					CreateSheetIfNotExists(service, spreadsheetId, sheetName);
					UpdateSheet(service, spreadsheetId, sheetName, table);
				}

				Debug.Log("Push completed successfully!");
				AssetDatabase.Refresh();
			}
			catch (Exception e)
			{
				Debug.LogError($"Push failed: {e}");
			}
		}

		[MenuItem("Tools/Localization Sync/Pull from Google Sheets")]
		private static void PullFromGoogleSheets()
		{
			 try
			{
				bool anyChanges = false;
				var service = GetSheetsService();
				var stringTables = GetAllStringTables();

				foreach (var tables in stringTables)
				{
					var sheetName = SanitizeSheetName(tables[0].TableCollectionName);
					Debug.Log($"Processing table: {tables[0].TableCollectionName}");

					var response = service.Spreadsheets.Values.Get(spreadsheetId, $"{sheetName}!A:Z").Execute();

					if (response.Values == null || response.Values.Count < 2)
					{
						Debug.LogWarning($"No data found in sheet {sheetName}");
						continue;
					}

					// Получаем список локалей из заголовков (пропускаем Key Name и Key ID)
					var localeCodes = response.Values[0].Cast<string>().Skip(2).ToList();

					Undo.RecordObject(tables[0], "Pull from Google Sheets");

					for (int i = 1; i < response.Values.Count; i++)
					{
						var row = response.Values[i];
						if (row.Count < 2) continue;

						var keyName = row[0]?.ToString();
						var keyId = row[1]?.ToString();

						if (string.IsNullOrEmpty(keyName)) continue;

						foreach (var table in tables)
						{
							// Находим запись в таблице либо по ID, либо по имени
							StringTableEntry entry = null;
							
							if (long.TryParse(keyId, out var id)) entry = table.GetEntry(id);
							if (entry == null) entry = table.GetEntry(keyName);
							if (entry == null)
							{
								Debug.LogWarning($"Key not found: {keyName} (ID: {keyId})");
								continue;
							}

							// Обновляем значения для всех локалей
							for (int j = 2; j < row.Count && j - 2 < localeCodes.Count; j++)
							{
								var localeCode = localeCodes[j - 2];
								var locale = LocalizationSettings.AvailableLocales.GetLocale(localeCode);

								if (locale == null)
								{
									Debug.LogWarning($"Locale not found: {localeCode}");
									continue;
								}
								
								var value = row[j]?.ToString() ?? string.Empty;
							
								tables.Find(x => x.LocaleIdentifier == locale.Identifier).GetEntry(keyName).Value = value;
							}
							
							EditorUtility.SetDirty(table);
							anyChanges = true;
							Debug.Log($"Updated {table.TableCollectionName} with data from sheet {sheetName}");
						}
					}

					if (anyChanges)
					{
						AssetDatabase.SaveAssets();
						AssetDatabase.Refresh();
						
						// Обновление локализации через реинициализацию
						if (LocalizationSettings.Instance != null)
						{
							var initOp = LocalizationSettings.Instance.GetInitializationOperation();
							if (!initOp.IsDone) initOp.WaitForCompletion();
							Debug.Log("Localization settings reinitialized");
						}

						// Принудительное обновление редактора
						EditorApplication.delayCall += () => {
							var windows = Resources.FindObjectsOfTypeAll<EditorWindow>();
							foreach (var window in windows)
							{
								window.Repaint();
							}
							Debug.Log("Editor UI refreshed");
						};
					}
				}

				AssetDatabase.SaveAssets();
				Debug.Log("Pull completed successfully!");
			}
			 catch (Exception e)
			 {
				 Debug.LogError($"Pull failed: {e}");
			 }
		}
		
		

		private static ServiceAccountCredential GetCredential()
		{
			var jsonPath = $"{Application.dataPath}/Editor/GoogleSheetsSync/{CredentialsPath}";
			using var stream = new FileStream(jsonPath, FileMode.Open, FileAccess.Read);
			var credential = GoogleCredential.FromStream(stream)
			                                 .CreateScoped(SheetsService.Scope.Spreadsheets)
			                                 .UnderlyingCredential as ServiceAccountCredential;

			return credential ?? throw new Exception("Failed to create credentials");
		}

		private static List<List<StringTable>> GetAllStringTables()
		{
			var allTables = Resources.FindObjectsOfTypeAll<StringTable>()
			                         .Where(t => t != null && t.SharedData != null) // Проверяем что таблица инициализирована
			                         .ToList();

			var names = new List<string>();

			foreach (var table in allTables)
				if (names.Find(x => x == table.TableCollectionName) == null)
					names.Add(table.TableCollectionName);

			var result = new List<List<StringTable>>();

			foreach (var name in names) result.Add(allTables.Where(x => x.TableCollectionName == name).ToList());

			return result;
		}

		private static SheetsService GetSheetsService()
		{
			var credential = GetCredential();
			return new SheetsService(new BaseClientService.Initializer()
			{
				HttpClientInitializer = credential,
				ApplicationName = "Unity Localization Sync"
			});
		}

		private static string SanitizeSheetName(string name)
		{
			var cleanName = name?
			                .Replace('/', '-')
			                .Replace('\\', '-')
			                .Replace('?', '-')
			                .Replace('*', '-')
			                .Replace('[', '-')
			                .Replace(']', '-')
			                .Trim() ?? "Untitled";

			return cleanName.Length > 100 ? cleanName.Substring(0, 100) : cleanName;
		}

		private static void CreateSheetIfNotExists(SheetsService service, string spreadsheetId, string sheetName)
		{
			try
			{
				var spreadsheet = service.Spreadsheets.Get(spreadsheetId).Execute();
				if (spreadsheet.Sheets.Any(s => s.Properties.Title == sheetName))
				{
					Debug.Log($"Sheet '{sheetName}' exists");
					return;
				}

				var addSheetRequest = new Request
				{
					AddSheet = new AddSheetRequest
					{
						Properties = new SheetProperties
						{
							Title = sheetName,
							GridProperties = new GridProperties { RowCount = 1000, ColumnCount = 20 }
						}
					}
				};

				service.Spreadsheets.BatchUpdate(new BatchUpdateSpreadsheetRequest
				{
					Requests = new List<Request> { addSheetRequest }
				}, spreadsheetId).Execute();
			}
			catch (GoogleApiException e)
			{
				Debug.LogError($"Google API Error: {e.Message}");
				throw;
			}
		}

		private static void UpdateSheet(SheetsService service, string spreadsheetId, string sheetName, List<StringTable> tables)
		{
			try
			{
				// 1. Получаем все доступные локали
				var locales = LocalizationSettings.AvailableLocales.Locales;
				if (locales.Count == 0)
				{
					Debug.LogError("No locales configured!");
					return;
				}

				// 2. Получаем SharedData для имен ключей
				var sharedData = tables[0].SharedData;
				if (sharedData == null)
				{
					Debug.LogError("Table has no SharedData!");
					return;
				}

				// 3. Подготавливаем данные
				var range = $"{sheetName}!A1:Z1000";
				var valueRange = new ValueRange { Values = new List<IList<object>>() };

				// Заголовки
				var headerRow = new List<object> { "Key Name", "Key ID" };
				headerRow.AddRange(locales.Select(l => l.Identifier.Code));
				valueRange.Values.Add(headerRow);

				// Данные
				foreach (var keyEntry in sharedData.Entries)
				{
					var keyName = keyEntry.Key;
					var keyId = keyEntry.Id;

					var dataRow = new List<object> { keyName, keyId };

					foreach (var locale in locales)
					{
						try
						{
							var tableEntry = tables.Find(x => x.LocaleIdentifier == locale.Identifier).GetEntry(keyEntry.Id);
							if (tableEntry != null)
							{
								var localizedValue = tableEntry.GetLocalizedString(locale.Identifier);
								dataRow.Add(localizedValue ?? "");
							}
							else
							{
								dataRow.Add("");
							}
						}
						catch (Exception e)
						{
							Debug.LogWarning($"Error getting '{keyName}' for {locale.Identifier}: {e.Message}");
							dataRow.Add("");
						}
					}

					valueRange.Values.Add(dataRow);
				}

				// 4. Отправляем данные
				service.Spreadsheets.Values.Clear(new ClearValuesRequest(), spreadsheetId, range).Execute();

				var updateRequest = service.Spreadsheets.Values.Update(valueRange, spreadsheetId, range);
				updateRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;
				var result = updateRequest.Execute();

				Debug.Log($"Updated sheet '{sheetName}': {result.UpdatedCells} cells");
			}
			catch (Exception e)
			{
				Debug.LogError($"Sheet update failed: {e}");
			}
		}

		[MenuItem("Tools/Localization Sync/List Sheets")]
		private static void ListSheets()
		{
			try
			{
				var service = GetSheetsService();
				var spreadsheet = service.Spreadsheets.Get(spreadsheetId).Execute();

				Debug.Log($"Sheets in {spreadsheetId}:");
				foreach (var sheet in spreadsheet.Sheets)
				{
					Debug.Log($"- {sheet.Properties.Title} (ID: {sheet.Properties.SheetId})");
				}
			}
			catch (Exception e)
			{
				Debug.LogError($"List sheets failed: {e}");
			}
		}
	}
}