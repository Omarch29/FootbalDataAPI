using System;
using System.Collections.Generic;

namespace FootbalDataAPI.models
{
    public class Player
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string CountryOfBirth { get; set; }
        public string Nationality { get; set; }
        public IList<TeamPlayer> Teams { get; set; }
    }
}