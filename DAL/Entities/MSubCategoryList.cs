using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace localMarketingSystem.DAL.Entities;

[Table("m_sub_category_list", Schema = "admin")]
public partial class MSubCategoryList
{
    [Column("id")]
    public long Id { get; set; }

    [Key]
    [Column("sub_category_id")]
    public int SubCategoryId { get; set; }

    [Column("sub_category_name")]
    [StringLength(250)]
    public string? SubCategoryName { get; set; }

    [Column("sub_category_desc")]
    public string? SubCategoryDesc { get; set; }

    [Column("category_id")]
    public int? CategoryId { get; set; }

    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_at", TypeName = "timestamp without time zone")]
    public DateTime? UpdatedAt { get; set; }

    [Column("deleted_at", TypeName = "timestamp without time zone")]
    public DateTime? DeletedAt { get; set; }

    [Column("product_id")]
    public int? ProductId { get; set; }

    [ForeignKey("CategoryId")]
    [InverseProperty("MSubCategoryLists")]
    public virtual MCategoryList? Category { get; set; }

    [ForeignKey("ProductId")]
    [InverseProperty("MSubCategoryLists")]
    public virtual MProductList? Product { get; set; }
}
