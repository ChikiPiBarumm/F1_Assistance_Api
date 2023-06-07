using System;
namespace F1_Assistance_Bot.Models
{
    public class RootObject
    {
        public MRData MRData { get; set; }
    }

    public class MRData
    {
        public RaceTable RaceTable { get; set; }
        public SeasonTable SeasonTable { get; set; }
        public StandingsTable StandingsTable { get; set; }
        public CircuitTable CircuitTable { get; set; }
    }

    public class RaceTable
    {
        public List<Race> Races { get; set; }
    }

    public class Race
    {
        public string RaceName { get; set; }
        public Circuit Circuit { get; set; }
        public List<Result> Results { get; set; }
        public List<QualifyingResult> QualifyingResults { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string Season { get; set; }
        public string Round { get; set; }
        public List<Lap> Laps { get; set; }
    }

    

    public class Result
    {
        public string Position { get; set; }
        public string Points { get; set; }
        public Driver Driver { get; set; }
        public Constructor Constructor { get; set; }
    }

    public class Driver
    {
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        public string Nationality { get; set; }
    }

    public class Constructor
    {
        public string Name { get; set; }
        public string Nationality { get; set; }
    }

    public class ScoreboardEntry
    {
        public string RaceName { get; set; }
        public string CircuitName { get; set; }
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        public string ConstructorName { get; set; }
        public string Position { get; set; }
        public string Points { get; set; }
    }

    public class RaceScheduleEntry
    {
        public string RaceName { get; set; }
        public string CircuitName { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
    }

    public class SeasonTable
    {
        public List<Season> Seasons { get; set; }
    }

    public class Season
    {
        public string season { get; set; }
    }

    public class QualifyingResult
    {
        public string Position { get; set; }
        public string Q1 { get; set; }
        public string Q2 { get; set; }
        public string Q3 { get; set; }
        public Driver Driver { get; set; }
        public Constructor Constructor { get; set; }
    }

    public class QualifyingResultEntry
    {
        public string RaceName { get; set; }
        public string CircuitName { get; set; }
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        public string ConstructorName { get; set; }
        public string Position { get; set; }
        public string Q1Time { get; set; }
        public string Q2Time { get; set; }
        public string Q3Time { get; set; }
    }

    public class DriverStandingsEntry
    {
        public string Position { get; set; }
        public string Points { get; set; }
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        public string Nationality { get; set; }
        public string ConstructorName { get; set; }
    }

    public class ConstructorStandingsEntry
    {
        public string Position { get; set; }
        public string Points { get; set; }
        public string ConstructorName { get; set; }
        public string Nationality { get; set; }
    }

    public class StandingsTable
    {
        public List<StandingsList> StandingsLists { get; set; }
    }

    public class StandingsList
    {
        public List<DriverStanding> DriverStandings { get; set; }
        public List<ConstructorStanding> ConstructorStandings { get; set; }
    }

    public class DriverStanding
    {
        public string Position { get; set; }
        public string Points { get; set; }
        public Driver Driver { get; set; }
        public List<Constructor> Constructors { get; set; }
    }

    public class ConstructorStanding
    {
        public string Position { get; set; }
        public string Points { get; set; }
        public Constructor Constructor { get; set; }
    }

    public class CircuitTable
    {
        public List<Circuit> Circuits { get; set; }
    }

    public class Circuit
    {
        public string CircuitId { get; set; }
        public string CircuitName { get; set; }
        public Location Location { get; set; }
    }

    public class Location
    {
        public string Country { get; set; }
        public string Locality { get; set; }
    }

    public class Lap
    {
        public string Number { get; set; }
        public List<Timing> Timings { get; set; }
    }

    public class Timing
    {
        public string DriverId { get; set; }
        public string Time { get; set; }
    }
}

