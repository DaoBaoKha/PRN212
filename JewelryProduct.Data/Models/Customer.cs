using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JewelryProduct.Data.Models;

public partial class Customer
{
    [Key]
    public int Id { get; set; }

    public string CustomerName { get; set; }

    public string CustomerPhone { get; set; }

    public string CustomerAddress { get; set; }

    public virtual ICollection<Request> Requests { get; set; } = new List<Request>();
}