﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DimensionsLoadConsole.Entities.DWHNorthwindOrders
{

    [Table("DimCustomers")]
    public partial class DimCustomer
    {
        [Key]
        public int CustomerKey { get; set; }

        public string? CustomerName { get; set; }

        public string? CustomerId { get; set; }

    }
}