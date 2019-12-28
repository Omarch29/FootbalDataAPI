using System;
using System.Collections.Generic;
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
            //var result = JsonConvert.DeserializeObject<T>(httpResponseBody);
            httpClient.Dispose();


            return httpResponseBody;
        }

        public CompetitionDTO GetCompetition(int Id) {
            var result = new CompetitionDTO();
            string jsonString = GetAsync($"/competitions/{ Id }");
            result = JsonConvert.DeserializeObject<CompetitionDTO>(jsonString);
            return result;
        }

        public CompetitionAndTeamsDTO GetCompetitionAndTeams(int CompetitionId){
            var result = new CompetitionAndTeamsDTO();
            string jsonString = GetAsync($"/competitions/{ CompetitionId }/teams");
            var json = JObject.Parse(jsonString);
            var teamsJson = json["teams"].ToString();
            var competitionJson = json["competition"].ToString();
            result.Teams = JsonConvert.DeserializeObject<List<TeamDTO>>(teamsJson);
            result.Competition =  JsonConvert.DeserializeObject<CompetitionDTO>(competitionJson);
            return result;
        }

        public List<TeamDTO> GetTeams(int CompetitionId) {
            string jsonString = GetAsync($"/competitions/{ CompetitionId }/teams");
            var json = JObject.Parse(jsonString);
            var teamsJson = json["teams"].ToString();
            var result = JsonConvert.DeserializeObject<List<TeamDTO>>(teamsJson);
            return result;
        }

        public CompleteTeamDTO GetCompleteTeamInfo(int TeamId) {
            var result = new CompleteTeamDTO();
            string jsonString = GetAsync($"/teams/{ TeamId }");
            var json = JObject.Parse(jsonString);
            var squadJson = json["squad"].ToString();
            result.Squad = JsonConvert.DeserializeObject<List<PlayerDTO>>(squadJson);
            var competitionsJson = json["activeCompetitions"].ToString();
            result.ActiveCompetitions = JsonConvert.DeserializeObject<List<CompetitionDTO>>(competitionsJson);
            return result;
        }

    }
}