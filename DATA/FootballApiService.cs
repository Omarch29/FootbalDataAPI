using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using FootbalDataAPI.DTOs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FootbalDataAPI.DATA
{
    public class FootballApiService : IFootBallApiService
    {
        private const string API_URL = "https://api.football-data.org/v2/";
        private const string TOKEN = "edb06c56816c454ea93bab142355eb57";

        private string GetAsync(string path)
        {
            HttpClient httpClient = new HttpClient();
            var headers = httpClient.DefaultRequestHeaders;
            headers.Remove("X-Auth-Token");
            headers.Add("X-Auth-Token", TOKEN);
            Uri requestUri = new Uri(API_URL + path);
            HttpResponseMessage httpResponse;
            httpResponse = httpClient.GetAsync(requestUri).GetAwaiter().GetResult();
            httpResponse.EnsureSuccessStatusCode();
            string httpResponseBody = httpResponse.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var responseHeaders = httpResponse.Headers;
            IEnumerable<string> values;
            int AvailableRequests = 0;
            int RequestCounter = 0;
            if (responseHeaders.TryGetValues("X-Requests-Available-Minute", out values))
            {
                string AvailableMinute = values.First();
                AvailableRequests = int.Parse(AvailableMinute);
            }
            if (responseHeaders.TryGetValues("X-RequestCounter-Reset", out values))
            {
                string RequestCounterReset = values.First();
                RequestCounter = int.Parse(RequestCounterReset);
            }
            if (AvailableRequests == 0)
            {
                if (RequestCounter > 0)
                {
                    System.Threading.Thread.Sleep(RequestCounter * 1000);
                }
            }
            httpClient.Dispose();
            return httpResponseBody;
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