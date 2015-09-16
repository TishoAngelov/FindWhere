namespace FindWhere.Model
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Category
    {
        private ICollection<ShoppingCenter> shoppingCenters;

        public Category()
        {
            this.shoppingCenters = new HashSet<ShoppingCenter>();
        }

        [Key]
        public int Id { get; set; }
        
        [MaxLength(100)]
        [Index(IsUnique = true)]
        public string Name { get; set; }

        // Relational
        public virtual ICollection<ShoppingCenter> ShoppingCenters
        {
            get { return this.shoppingCenters; }
            set { this.shoppingCenters = value; }
        }
    }
}
