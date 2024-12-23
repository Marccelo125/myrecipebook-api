﻿using MyRecipeBook.Domain.Enums;

namespace MyRecipeBook.Communication.Requests
{
    public class RequestRecipe
    {
        public string Title { get; set; } = string.Empty;
        public CookingTime? CookingTime { get; set; }
        public Difficulty? Difficulty { get; set; }
        public IList<string> Ingredients { get; set; } = [];
        public IList<RequestInstruction> Instructions { get; set; } = [];
        public IList<DishType> DishTypes { get; set; } = [];    
    }
}
