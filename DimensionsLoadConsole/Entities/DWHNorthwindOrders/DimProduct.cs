﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DimensionsLoadConsole.Entities.DWHNorthwindOrders
{
    [Table("DimProducts")]
    public partial class DimProduct
    {
        [Key]
        public int ProductKey { get; set; }

        public string? ProductName { get; set; }

        public int ProductId { get; set; }


    }
}