using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

public static class GaiaDataFetcher
{
    private static readonly HttpClient client = new HttpClient();

    public static async Task<NativeArray<float3>> FetchAndFilterData(string apiUrl, Func<JObject, bool> filter)
    {
        var response = await client.GetStringAsync(apiUrl);
        var jsonData = JArray.Parse(response);
        var filteredData = jsonData.Where(filter).Select(item => new float3(
            (float)item["x"],
            (float)item["y"],
            (float)item["z"]
        )).ToArray();

        var positions = new NativeArray<float3>(filteredData.Length, Allocator.TempJob);
        for (int i = 0; i < filteredData.Length; i++)
        {
            positions[i] = filteredData[i];
        }

        return positions;
    }
}