using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using AutoMapper.Configuration;
using FootbalDataAPI.DTOs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FootbalDataAPI.DATA
{
    public class FootballApiService : IFootBallApiService
    {
        private const string API_URL = "https://api.football-data.org/v2/";
        private readonly string TOKEN;

        public FootballApiService(string token)
        {
            TOKEN = token;
        }

        private string GetAsync(string path)
        {
            string response = "";
            try
            {
                // Create instance of HttpClient
                using (HttpClient httpClient = new HttpClient())
                {

                    // Add Auth Headers
                    var headers = httpClient.DefaultRequestHeaders;
                    headers.Remove("X-Auth-Token");
                    headers.Add("X-Auth-Token", TOKEN);

                    // Execute the Request
                    Uri requestUri = new Uri(API_URL + path);
                    HttpResponseMessage httpResponse;
                    httpResponse = httpClient.GetAsync(requestUri).GetAwaiter().GetResult();
                    httpResponse.EnsureSuccessStatusCode();
                    // get the response
                    string httpResponseBody = httpResponse.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    // get the Response Headers
                    var responseHeaders = httpResponse.Headers;
                    IEnumerable<string> values;
                    int AvailableRequests = 0;
                    int RequestCounter = 0;
                    // Get the Avaible Requests in a minute from the Response Header
                    if (responseHeaders.TryGetValues("X-Requests-Available-Minute", out values))
                    {
                        string AvailableMinute = values.First();
                        AvailableRequests = int.Parse(AvailableMinute);
                    }
                    // Get the Request Counter (in seconds) to make a new request
                    if (responseHeaders.TryGetValues("X-RequestCounter-Reset", out values))
                    {
                        string RequestCounterReset = values.First();
                        RequestCounter = int.Parse(RequestCounterReset);
                    }
                    // If there's not more Avaible Request the thread Wait to complete a minute from the first request done.
                    if (AvailableRequests == 0)
                    {
                        if (RequestCounter > 0)
                        {
                            System.Threading.Thread.Sleep((RequestCounter + 1) * 1000);
                        }
                    }
                    // Return the JSON response as string
                    response = httpResponseBody;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return response;
            }
            return response;
        }

        public CompetitionDTO GetCompetition(int Id)
        {
            var result = new CompetitionDTO();
            string jsonString = GetAsync($"/competitions/{ Id }");
            result = JsonConvert.DeserializeObject<CompetitionDTO>(jsonString);
            return result;
        }

        public CompetitionAndTeamsDTO GetCompetitionAndTeams(string legueCode)
        {
            var result = new CompetitionAndTeamsDTO();
            string jsonString = GetAsync($"/competitions/{ legueCode }/teams");
            var json = JObject.Parse(jsonString);
            if (json != null)
            {
                var teamsJson = json["teams"].ToString();
                var competitionJson = json["competition"].ToString();
                result.Teams = JsonConvert.DeserializeObject<List<TeamDTO>>(teamsJson);
                result.Competition = JsonConvert.DeserializeObject<CompetitionDTO>(competitionJson);
            }
            return result;
        }

        public List<TeamDTO> GetTeams(int CompetitionId)
        {
            string jsonString = GetAsync($"/competitions/{ CompetitionId }/teams");
            var json = JObject.Parse(jsonString);
            var teamsJson = json["teams"].ToString();
            var result = JsonConvert.DeserializeObject<List<TeamDTO>>(teamsJson);
            return result;
        }

        public CompleteTeamDTO GetCompleteTeamInfo(int TeamId)
        {
            var result = new CompleteTeamDTO();
            string jsonString = GetAsync($"/teams/{ TeamId }");
            var json = JObject.Parse(jsonString);
            if (json != null)
            {
                var squadJson = json["squad"].ToString();
                result.Squad = JsonConvert.DeserializeObject<List<PlayerDTO>>(squadJson);
                var competitionsJson = json["activeCompetitions"].ToString();
                result.ActiveCompetitions = JsonConvert.DeserializeObject<List<CompetitionDTO>>(competitionsJson);
            }
            return result;
        }

    }
}