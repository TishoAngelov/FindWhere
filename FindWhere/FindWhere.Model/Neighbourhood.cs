namespace FindWhere.Model
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Neighbourhood
    {
        private ICollection<Location> locations;

        public Neighbourhood()
        {
            this.locations = new HashSet<Location>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string Name { get; set; }

        // Relational
        public virtual int CityId { get; set; }

        public virtual City City { get; set; }

        public virtual ICollection<Location> Locations
        {
            get { return this.locations; }
            set { this.locations = value; }
        }
    }
}
