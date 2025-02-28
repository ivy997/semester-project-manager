﻿namespace SemesterProjectManager.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public abstract class BaseModel<TKey> 
    {
        [Key]
        public TKey Id { get; set; }
    }
}
