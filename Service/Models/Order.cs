namespace SharedLibrary.Models
{
    /// <summary>
    /// Represents a model for a order.
    /// </summary>
    public class Order
    {
        /// <summary>
        /// Order ID
        /// </summary>
        public Guid Id            { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Product name
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// Product quantity
        /// </summary>
        public int Quantity       { get; set; }

        /// <summary>
        /// Product price
        /// </summary>
        public decimal Price      { get; set; }
    }
}
