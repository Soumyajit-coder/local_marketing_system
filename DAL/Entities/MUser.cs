using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace localMarketingSystem.DAL.Entities;

[Table("m_users", Schema = "admin")]
public partial class MUser
{
    [Column("id")]
    public long Id { get; set; }

    [Key]
    [Column("user_id")]
    public int UserId { get; set; }

    [Column("user_name")]
    [StringLength(200)]
    public string? UserName { get; set; }

    [Column("email")]
    [StringLength(500)]
    public string? Email { get; set; }

    [Column("mobile_no")]
    [StringLength(10)]
    public string? MobileNo { get; set; }

    [Column("role")]
    [StringLength(20)]
    public string? Role { get; set; }

    [Column("status")]
    public bool? Status { get; set; }

    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_at", TypeName = "timestamp without time zone")]
    public DateTime? UpdatedAt { get; set; }

    [Column("deleted_at", TypeName = "timestamp without time zone")]
    public DateTime? DeletedAt { get; set; }

    [Column("password_hash")]
    public byte[]? PasswordHash { get; set; }

    [Column("password_salt")]
    public byte[]? PasswordSalt { get; set; }

    [Column("password_hash2")]
    public List<byte[]>? PasswordHash2 { get; set; }
}
