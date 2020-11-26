using System;

namespace XF40Demo.Models
{
    public enum StandingChange
    {
        up,
        down,
        none
    }

    public class PowerStanding
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Position { get; set; }
        public StandingChange Change { get; set; }
        public bool Turmoil { get; set; }
        public string Allegiance { get; set; }
        public string ShortName { get; set; }
        public string Cycle { get; set; }
        public DateTime LastUpdated { get; set; }

        public string ChangeString
        {
            get
            {
                switch (Change)
                {
                    case StandingChange.up:
                        return "Up";
                    case StandingChange.down:
                        return "Down";
                    case StandingChange.none:
                        return "No Change";
                    default:
                        return String.Empty;
                }
            }
        }

        public PowerStanding()
        {
            // required for NewtonsoftJson
        }

        public PowerStanding(int id, string name, int position, StandingChange change, bool turmoil, string allegiance, string shortname, DateTime updated)
        {
            Id = id;
            Name = name;
            Position = position;
            Change = change;
            Turmoil = turmoil;
            Allegiance = allegiance;
            ShortName = shortname;
            LastUpdated = updated;
        }

        public override string ToString()
        {
            return Turmoil
                ? $"{Position:00} {ChangeString} - {Name} ({Allegiance}) TURMOIL"
                : $"{Position:00} {ChangeString} - {Name} ({Allegiance})";
        }
    }
}
