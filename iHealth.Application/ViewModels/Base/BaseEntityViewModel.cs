﻿using System.ComponentModel.DataAnnotations;

namespace iHealth.Core.Application.ViewModels.Base
{
    public class BaseEntityViewModel
    {
        public int Id { get; set; }

        //Auditable

        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public bool? Deleted { get; set; } = false;
        public string? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
    }
}
