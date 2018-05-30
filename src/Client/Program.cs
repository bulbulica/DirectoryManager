// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using IdentityModel.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Client
{
    public class Program
    {
        public static void Main(string[] args) => MainAsync().GetAwaiter().GetResult();
        
        private static async Task MainAsync()
        {
            // discover endpoints from metadata
            var disco = await DiscoveryClient.GetAsync("http://localhost:5000/API/GetEmployees");
            if (disco.IsError)
            {
                Console.WriteLine(disco.Error);
                Console.ReadKey();
                return;
            }

            // request token
            var tokenClient = new TokenClient(disco.TokenEndpoint, "evaluationManagerClient", "Pass123$");
            var tokenResponse = await tokenClient.RequestClientCredentialsAsync("GetEmployees");

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                Console.ReadLine();
                return;
            }

            Console.WriteLine(tokenResponse.Json);
            Console.WriteLine("\n\n");

            // call api
            var client = new HttpClient();
            client.SetBearerToken(tokenResponse.AccessToken);

            var response = await client.GetAsync("http://localhost:5000/API/GetEmployees");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(JArray.Parse(content));
            }
            Console.ReadKey();
        }
    }
}