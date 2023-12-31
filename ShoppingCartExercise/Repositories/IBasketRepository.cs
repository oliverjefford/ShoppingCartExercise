﻿using ShoppingCartExercise.Models.DatabaseModels;

namespace ShoppingCartExercise.Repositories
{
    public interface IBasketRepository
    {
        void AddItemToBasket(int basketId, string barcode);
        int CalculateTotal(int basketId);
        List<Basket> GetBasket(int basketId);
        List<Basket> GetBaskets();
        void RemoveItemFromBasket(int basketId, string barcode);
    }
}