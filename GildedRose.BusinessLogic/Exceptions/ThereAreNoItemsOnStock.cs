using System;

namespace GildedRose.BusinessLogic.Exceptions
{
    [Serializable]
    public class ThereAreNoItemsOnStockException : Exception
    {
        public ThereAreNoItemsOnStockException()
        {

        }

        public ThereAreNoItemsOnStockException(string message) : base(message)
        {

        }
    }
}
