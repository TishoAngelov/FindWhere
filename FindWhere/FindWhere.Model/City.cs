namespace FindWhere.Model
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class City
    {
        private ICollection<Neighbourhood> neighbourhoods;

        public City()
        {
            this.neighbourhoods = new HashSet<Neighbourhood>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        
        // Relational
        public virtual int CountryId { get; set; }

        public virtual Country Country { get; set; }

        public virtual ICollection<Neighbourhood> Neighbourhoods
        {
            get { return this.neighbourhoods; }
            set { this.neighbourhoods = value; }
        }
    }
}
