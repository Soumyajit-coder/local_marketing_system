using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace localMarketingSystem.DAL.Entities;

[Table("m_category_list", Schema = "admin")]
public partial class MCategoryList
{
    [Column("id")]
    public long Id { get; set; }

    [Key]
    [Column("category_id")]
    public int CategoryId { get; set; }

    [Column("category_name")]
    [StringLength(250)]
    public string? CategoryName { get; set; }

    [Column("category_desc")]
    public string? CategoryDesc { get; set; }

    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_at", TypeName = "timestamp without time zone")]
    public DateTime? UpdatedAt { get; set; }

    [Column("deleted_at", TypeName = "timestamp without time zone")]
    public DateTime? DeletedAt { get; set; }

    [Column("product_id")]
    public int? ProductId { get; set; }

    [InverseProperty("Category")]
    public virtual ICollection<MSubCategoryList> MSubCategoryLists { get; set; } = new List<MSubCategoryList>();

    [ForeignKey("ProductId")]
    [InverseProperty("MCategoryLists")]
    public virtual MProductList? Product { get; set; }
}
