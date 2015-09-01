using System;

namespace FindWhere.Model
{
    public class Location
    {
        public Location()
        {
            this.Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }

        public string PlaceId { get; set; }

        public double Latitude { get; set; }
    }
}
