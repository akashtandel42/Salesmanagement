using System.ComponentModel.DataAnnotations;

namespace Salesmanagement.Models
{
    public class Sale
    {
        [Key]
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public decimal Amount { get; set; }
        public DateTime SaleDate { get; set; }
        public int ProductId { get; set; }
        public int RegionId { get; set; }

    }
}
