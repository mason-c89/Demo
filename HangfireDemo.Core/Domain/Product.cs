using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PractiseForMason.Core.Domain;

namespace HangfireDemo.Core.Domain;

[Table("product")]
public class Product : IEntity
{
    [Column("id")]
    public Guid Id { get; set; }
    
    [Column("name")]
    public string Name { get; set; }
    
    [Column("price")]
    public decimal Price { get; set; }
}