using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Unity.Collections;
using Unity.Mathematics;

public static class GaiaDataFetcher
{
    public static async Task<NativeArray<float3>> FetchAndFilterData(string apiUrl, Func<JToken, bool> filter)
    {
        using (HttpClient client = new HttpClient())
        {
            var response = await client.GetStringAsync(apiUrl);
            var data = JArray.Parse(response);
            List<float3> positions = new List<float3>();

            foreach (var item in data)
            {
                if (filter(item))
                {
                    positions.Add(new float3((float)item["x"], (float)item["y"], (float)item["z"]));
                }
            }

            NativeArray<float3> nativePositions = new NativeArray<float3>(positions.ToArray(), Allocator.Persistent);
            return nativePositions;
        }
    }
}