namespace Wolfenstein.Common.Interfaces
{
    using Models;
    using System.Collections.Generic;

    /// <summary>
    /// Interface ILootable
    /// </summary>
    public interface ILootable
    {
        /// <summary>
        /// Returns the contents of the inventory of the ILootable object
        /// </summary>
        /// <returns>List of items</returns>
        List<Item> GetContent();
    }
}
