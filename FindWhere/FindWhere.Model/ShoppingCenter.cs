namespace FindWhere.Model
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class ShoppingCenter
    {
        public ShoppingCenter()
        {
            this.Id = Guid.NewGuid();
        }

        // One to One relation
        [Key, ForeignKey("Location")]

        public Guid Id { get; set; }

        [MaxLength(13)]
        public string WorkTime { get; set; }    // Example: 08:00 - 17:00

        public bool IsClosed { get; set; }

        [MaxLength(500)]
        public string Details { get; set; }

        // TODO: Images.

        // Relational
        public virtual int CategoryId { get; set; }

        public virtual Category Category { get; set; }

        public virtual Location Location { get; set; }
    }
}
