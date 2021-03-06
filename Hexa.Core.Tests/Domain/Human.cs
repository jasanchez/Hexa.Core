﻿using System.ComponentModel.DataAnnotations;

using Hexa.Core.Domain;

namespace Hexa.Core.Tests.Domain
{
    public class Human : AuditableRootEntity<Human>
    {
        [Required]
        public virtual string Name { get; set; }

        [Required]
        public virtual bool isMale { get; set; }
    }
}
