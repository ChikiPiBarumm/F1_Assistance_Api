using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using F1_Assistance_Bot.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace F1_Assistance_Bot.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ErgastContoller : Controller
    {
        private readonly HttpClient httpClient;

        
        public ErgastContoller()
        {
            httpClient = new HttpClient();
        }

        [HttpGet("latest-race-result")]
        public async Task<IActionResult> GetLatestRaceResult()
        {
            string apiUrl = "http://ergast.com/api/f1/current/last/results.json";

            try
            {
                HttpResponseMessage response = await httpClient.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();

                // Deserialize the JSON response into objects
                var result = JsonConvert.DeserializeObject<RootObject>(responseBody);

                // Extract the relevant information and create a scoreboard
                var scoreboard = new List<ScoreboardEntry>();

                foreach (var raceResult in result.MRData.RaceTable.Races)
                {
                    foreach (var driverResult in raceResult.Results)
                    {
                        var entry = new ScoreboardEntry
                        {
                            RaceName = raceResult.RaceName,
                            CircuitName = raceResult.Circuit.CircuitName,
                            GivenName = driverResult.Driver.GivenName,
                            FamilyName = driverResult.Driver.FamilyName,
                            ConstructorName = driverResult.Constructor.Name,
                            Position = driverResult.Position,
                            Points = driverResult.Points
                        };

                        scoreboard.Add(entry);
                    }
                }

                // Return the scoreboard
                return Ok(scoreboard);
            }
            catch (HttpRequestException ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpGet("race-schedule")]
        public async Task<IActionResult> GetRaceSchedule()
        {
            string apiUrl = "http://ergast.com/api/f1/current.json";

            try
            {
                HttpResponseMessage response = await httpClient.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();

                // Deserialize the JSON response into objects
                var result = JsonConvert.DeserializeObject<RootObject>(responseBody);

                // Extract the relevant information and create a race schedule list
                var raceSchedule = new List<RaceScheduleEntry>();

                foreach (var race in result.MRData.RaceTable.Races)
                {
                    var entry = new RaceScheduleEntry
                    {
                        RaceName = race.RaceName,
                        CircuitName = race.Circuit.CircuitName,
                        Date = race.Date,
                        Time = race.Time
                    };

                    raceSchedule.Add(entry);
                }

                // Return the race schedule
                return Ok(raceSchedule);
            }
            catch (HttpRequestException ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpGet("list-of-seasons")]
        public async Task<IActionResult> GetSeasonList()
        {
            string apiUrl = "http://ergast.com/api/f1/seasons.json";

            try
            {
                HttpResponseMessage response = await httpClient.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();

                // Deserialize the JSON response into objects
                var result = JsonConvert.DeserializeObject<RootObject>(responseBody);

                // Extract the relevant information and create a season list
                var seasonList = new List<string>();

                foreach (var season in result.MRData.SeasonTable.Seasons)
                {
                    seasonList.Add(season.season);
                }

                // Return the season list
                return Ok(seasonList);
            }
            catch (HttpRequestException ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpGet("qualifying-results")]
        public async Task<IActionResult> GetQualifyingResults()
        {
            string apiUrl = "http://ergast.com/api/f1/current/last/qualifying.json";

            try
            {
                HttpResponseMessage response = await httpClient.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();

                // Deserialize the JSON response into objects
                var result = JsonConvert.DeserializeObject<RootObject>(responseBody);

                // Extract the relevant information and create a list of qualifying results
                var qualifyingResults = new List<QualifyingResultEntry>();

                foreach (var raceResult in result.MRData.RaceTable.Races)
                {
                    foreach (var driverResult in raceResult.QualifyingResults)
                    {
                        var entry = new QualifyingResultEntry
                        {
                            RaceName = raceResult.RaceName,
                            CircuitName = raceResult.Circuit.CircuitName,
                            GivenName = driverResult.Driver.GivenName,
                            FamilyName = driverResult.Driver.FamilyName,
                            ConstructorName = driverResult.Constructor.Name,
                            Position = driverResult.Position,
                            Q1Time = driverResult.Q1,
                            Q2Time = driverResult.Q2,
                            Q3Time = driverResult.Q3
                        };

                        qualifyingResults.Add(entry);
                    }
                }

                // Return the list of qualifying results
                return Ok(qualifyingResults);
            }
            catch (HttpRequestException ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpGet("driver-standings")]
        public async Task<IActionResult> GetDriverStandings()
        {
            string apiUrl = "http://ergast.com/api/f1/current/last/driverStandings.json";

            try
            {
                HttpResponseMessage response = await httpClient.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();

                // Deserialize the JSON response into objects
                var result = JsonConvert.DeserializeObject<RootObject>(responseBody);

                // Extract the relevant information and create a driver standings list
                var driverStandings = new List<DriverStandingsEntry>();

                foreach (var driverStanding in result.MRData.StandingsTable.StandingsLists[0].DriverStandings)
                {
                    var entry = new DriverStandingsEntry
                    {
                        Position = driverStanding.Position,
                        Points = driverStanding.Points,
                        GivenName = driverStanding.Driver.GivenName,
                        FamilyName = driverStanding.Driver.FamilyName,
                        Nationality = driverStanding.Driver.Nationality,
                        ConstructorName = driverStanding.Constructors[0].Name
                    };

                    driverStandings.Add(entry);
                }

                // Return the driver standings
                return Ok(driverStandings);
            }
            catch (HttpRequestException ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpGet("constructor-standings")]
        public async Task<IActionResult> GetConstructorStandings()
        {
            string apiUrl = "http://ergast.com/api/f1/current/last/constructorStandings.json";

            try
            {
                HttpResponseMessage response = await httpClient.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();

                // Deserialize the JSON response into objects
                var result = JsonConvert.DeserializeObject<RootObject>(responseBody);

                // Extract the relevant information and create a constructor standings list
                var constructorStandings = new List<ConstructorStandingsEntry>();

                foreach (var constructorStanding in result.MRData.StandingsTable.StandingsLists[0].ConstructorStandings)
                {
                    var entry = new ConstructorStandingsEntry
                    {
                        Position = constructorStanding.Position,
                        Points = constructorStanding.Points,
                        ConstructorName = constructorStanding.Constructor.Name,
                        Nationality = constructorStanding.Constructor.Nationality
                    };

                    constructorStandings.Add(entry);
                }

                // Return the constructor standings
                return Ok(constructorStandings);
            }
            catch (HttpRequestException ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpGet("circuits")]
        public async Task<IActionResult> GetCircuits()
        {
            string apiUrl = "http://ergast.com/api/f1/circuits.json";

            try
            {
                HttpResponseMessage response = await httpClient.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();

                // Deserialize the JSON response into objects
                var result = JsonConvert.DeserializeObject<RootObject>(responseBody);

                // Extract the relevant information and create a circuit list
                var circuits = new List<Circuit>();

                foreach (var circuit in result.MRData.CircuitTable.Circuits)
                {
                    var entry = new Circuit
                    {
                        CircuitId = circuit.CircuitId,
                        CircuitName = circuit.CircuitName,
                        Location = new Location
                        {
                            Country = circuit.Location.Country,
                            Locality = circuit.Location.Locality
                        }
                    };

                    circuits.Add(entry);
                }

                // Return the circuit list
                return Ok(circuits);
            }
            catch (HttpRequestException ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }


        [HttpGet("lap-times")]
        public async Task<IActionResult> GetLapTimes()
        {
            string apiUrl = "http://ergast.com/api/f1/2022/5/laps/1.json";

            try
            {
                HttpResponseMessage response = await httpClient.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();

                // Deserialize the JSON response into objects
                var result = JsonConvert.DeserializeObject<RootObject>(responseBody);

                // Extract the relevant information and create a lap times list
                var lapTimes = new List<Lap>();

                foreach (var race in result.MRData.RaceTable.Races)
                {
                    foreach (var lap in race.Laps)
                    {
                        var entry = new Lap
                        {
                            Number = lap.Number,
                            Timings = lap.Timings
                        };

                        lapTimes.Add(entry);
                    }
                }

                // Return the lap times list
                return Ok(lapTimes);
            }
            catch (HttpRequestException ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

    }
}