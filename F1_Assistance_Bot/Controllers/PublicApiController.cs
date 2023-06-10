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
    public class ErgastController : Controller
    {
        private readonly HttpClient httpClient;

        
        public ErgastController()
        {
            httpClient = new HttpClient();
        }

        

        [HttpGet("race-result/{year}/{round}/results")]
        public async Task<IActionResult> GetLatestRaceResult([FromRoute] string year, string round)
        {
            string apiUrl = "http://ergast.com/api/f1/"+year+"/"+round+"/results.json";

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

        [HttpGet("race-schedule/{year}")]
        public async Task<IActionResult> GetRaceSchedule([FromRoute] string year)
        {
            string apiUrl = "http://ergast.com/api/f1/"+year+".json";

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
                        Round = race.Round,
                        RaceName = race.RaceName,
                        CircuitName = race.Circuit.CircuitName,
                        Url = race.Url,
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
            string apiUrl = "http://ergast.com/api/f1/seasons.json?limit=100&offset=50";

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

        [HttpGet("driver-standings/{year}/{round}/driverStandings")]
        public async Task<IActionResult> GetDriverStandings([FromRoute] string year, string round)
        {
            string apiUrl = "http://ergast.com/api/f1/"+year+"/"+round+"/driverStandings.json";

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

        [HttpGet("constructor-standings/{year}/{round}/constructorStandings")]
        public async Task<IActionResult> GetConstructorStandings([FromRoute] string year, string round)
        {
            string apiUrl = "http://ergast.com/api/f1/"+year+"/"+round+"/constructorStandings.json";

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

        [HttpGet("circuits-info/{year}/{round}/circuits")]
        public async Task<IActionResult> GetCircuits([FromRoute] string year, string round)
        {
            string apiUrl = "http://ergast.com/api/f1/"+year+"/"+round+"/circuits.json";

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
                        Url = circuit.Url,
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
        
        [HttpGet("qualifying-results/{year}/{round}/qualifying")]
        public async Task<IActionResult> GetQualifyingResults([FromRoute] string year, string round)
        {
            string apiUrl = "http://ergast.com/api/f1/"+year+"/"+round+"/qualifying.json";

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

        [HttpGet("driver-info/{year}/{round}")]
        public async Task<IActionResult> GetDriverInfo([FromRoute] string year, [FromRoute] string round)
        {
            string apiUrl = $"http://ergast.com/api/f1/{year}/{round}/drivers.json";

            try
            {
                HttpResponseMessage response = await httpClient.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();

                // Deserialize the JSON response into objects
                var result = JsonConvert.DeserializeObject<RootObject>(responseBody);

                // Extract the relevant driver information
                var driverInfoList = new List<DriverInfo>();

                if (result?.MRData?.DriverTable?.Drivers != null)
                {
                    foreach (var driver in result.MRData.DriverTable.Drivers)
                    {
                        var driverInfo = new DriverInfo
                        {
                            DriverId = driver.DriverId,
                            Url = driver.Url,
                            GivenName = driver.GivenName,
                            FamilyName = driver.FamilyName,
                            Nationality = driver.Nationality,
                        };

                        driverInfoList.Add(driverInfo);
                    }

                    // Return the driver information
                    return Ok(driverInfoList);
                }
                else
                {
                    // Handle the case when the necessary objects are null
                    return BadRequest("Invalid response from the API.");
                }
            }
            catch (HttpRequestException ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }
        
        [HttpGet("constructor-info/{year}/{round}")]
        public async Task<IActionResult> GetConstructorInfo([FromRoute] string year, [FromRoute] string round)
        {
            string apiUrl = $"http://ergast.com/api/f1/{year}/{round}/constructors.json";

            try
            {
                HttpResponseMessage response = await httpClient.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();

                // Deserialize the JSON response into objects
                var result = JsonConvert.DeserializeObject<RootObject>(responseBody);

                // Extract the relevant constructor information
                var constructorInfoList = new List<ConstructorInfo>();

                if (result?.MRData?.ConstructorTable?.Constructors != null)
                {
                    foreach (var constructor in result.MRData.ConstructorTable.Constructors)
                    {
                        var constructorInfo = new ConstructorInfo
                        {
                            ConstructorId = constructor.ConstructorId,
                            Url = constructor.Url,
                            Name = constructor.Name,
                            Nationality = constructor.Nationality
                        };

                        constructorInfoList.Add(constructorInfo);
                    }

                    // Return the constructor information
                    return Ok(constructorInfoList);
                }
                else
                {
                    // Handle the case when the necessary objects are null
                    return BadRequest("Invalid response from the API.");
                }
            }
            catch (HttpRequestException ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }



    }
}