using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace localMarketingSystem.DAL.Entities;

[Table("m_product_list", Schema = "admin")]
public partial class MProductList
{
    [Column("id")]
    public long Id { get; set; }

    [Key]
    [Column("product_id")]
    public int ProductId { get; set; }

    [Column("produc_name")]
    [StringLength(100)]
    public string? ProducName { get; set; }

    [Column("product_desc")]
    [StringLength(500)]
    public string? ProductDesc { get; set; }

    [Column("product_prize")]
    public int? ProductPrize { get; set; }

    [InverseProperty("Product")]
    public virtual ICollection<MCategoryList> MCategoryLists { get; set; } = new List<MCategoryList>();

    [InverseProperty("Product")]
    public virtual ICollection<MSubCategoryList> MSubCategoryLists { get; set; } = new List<MSubCategoryList>();
}
