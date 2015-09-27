namespace FindWhere.Model
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Location
    {
        // Note: We can't make a relation many to many between Location and Shopping center
        // with Uniquie(lat, lng) because if there is already added location and it is approved,
        // another user can add new shopping center to the location but it will be approved automatically.
        // He might not have the permission to add new locations and respectively shopping centers wihtout
        // approval.
        // That is why we will keep the relation one to one.
        public Location()
        {
            this.Id = Guid.NewGuid();

            this.AddedOn = DateTime.Now;
        }

        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "The selected location is incorrect!")]
        public double Latitude { get; set; }

        public double Longitude { get; set; }

        [Required]
        public string FullAddress { get; set; }

        public string PlaceId { get; set; }     // From Google maps API

        public bool IsApproved { get; set; }

        // TODO: Rating.

        public DateTime AddedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        // Relational
        public virtual string UserId { get; set; }

        public virtual User User { get; set; }

        public virtual int NeighbourhoodId { get; set; }

        public virtual Neighbourhood Neighbourhood { get; set; }

        public virtual ShoppingCenter ShoppingCenter { get; set; }
    }
}