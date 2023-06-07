using System;
namespace F1_Assistance_Bot.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string ChatId { get; set; }
        public string DriverId { get; set; }
        public string TeamId { get; set; }
    }
}

