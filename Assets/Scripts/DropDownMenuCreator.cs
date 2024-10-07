using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class DropDownMenuCreator : MonoBehaviour
{
    public TMP_Dropdown dropdown;

    private string apiUrl = "https://exoplanetarchive.ipac.caltech.edu/TAP/sync?";
    public List<string> exoplanetNames = new List<string>();

    async void Start()
    {
        if (dropdown == null)
        {
            Debug.LogError("TMP_Dropdown is not assigned in the Inspector!");
            return;
        }

        string query = @"
        SELECT pl_name
        FROM ps
        WHERE pl_rade < 2";

        string jsonData = await FetchExoplanetDataAsync(query);

        if (!string.IsNullOrEmpty(jsonData))
        {
            ProcessData(jsonData);
            PopulateDropdown();
        }
    }

    async Task<string> FetchExoplanetDataAsync(string query)
    {
        string fullUrl = apiUrl + "query=" + UnityEngine.Networking.UnityWebRequest.EscapeURL(query) + "&format=json";

        using (var webRequest = UnityEngine.Networking.UnityWebRequest.Get(fullUrl))
        {
            var operation = webRequest.SendWebRequest();

            while (!operation.isDone)
                await Task.Yield();

            if (webRequest.result == UnityEngine.Networking.UnityWebRequest.Result.ConnectionError || webRequest.result == UnityEngine.Networking.UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + webRequest.error);
                return null;
            }
            else
            {
                Debug.Log("Fetched JSON Data: " + webRequest.downloadHandler.text);
                return webRequest.downloadHandler.text;
            }
        }
    }

    void ProcessData(string jsonData)
    {
        // Parse the JSON data using Newtonsoft.Json
        var exoplanetList = JsonConvert.DeserializeObject<List<Exoplanet>>(jsonData);

        if (exoplanetList != null && exoplanetList.Count > 0)
        {
            foreach (var exo in exoplanetList)
            {
                exoplanetNames.Add(exo.pl_name);
            }
            Debug.Log("Number of exoplanet names processed: " + exoplanetNames.Count);
        }
        else
        {
            Debug.LogError("Failed to parse exoplanet data or no data found.");
        }
    }

    void PopulateDropdown()
    {
        if (dropdown == null)
        {
            Debug.LogError("TMP_Dropdown is not assigned in the Inspector!");
            return;
        }

        if (exoplanetNames == null || exoplanetNames.Count == 0)
        {
            Debug.LogError("No exoplanet names available to populate the dropdown.");
            return;
        }

        dropdown.ClearOptions();
        dropdown.AddOptions(exoplanetNames);
        dropdown.RefreshShownValue();
        Debug.Log("Dropdown populated with exoplanet names.");
    }

    [System.Serializable]
    public class Exoplanet
    {
        public string pl_name;
    }
}