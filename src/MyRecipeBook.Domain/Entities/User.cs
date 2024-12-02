﻿namespace MyRecipeBook.Domain.Entities
{
    public class User : EntityBase
    {
        public bool Active { get; set; } = true;

        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public DateTime BirthDate { get; set; }

        public Guid UserIdentifier { get; set; } = Guid.NewGuid();
    }
}
