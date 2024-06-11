﻿namespace Product.Domain.Entities.Base
{
    public class BaseEntity
    {
        public long Id { get; set; }

        public long CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; }

        public long? UpdatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
