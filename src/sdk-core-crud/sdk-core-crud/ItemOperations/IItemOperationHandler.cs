using VideoOS.Platform.SDK.Core;

namespace sdk_core_crud.ItemOperations
{
    /// <summary>
    /// Interface for handling operations on configuration items
    /// </summary>
    internal interface IItemOperationHandler
    {
        /// <summary>
        /// Gets the name of the item type this handler manages
        /// </summary>
        string ItemTypeName { get; }

        /// <summary>
        /// Indicates whether this item type supports add/delete operations
        /// </summary>
        bool SupportsAddDelete { get; }

        /// <summary>
        /// Executes all operations for this item type
        /// </summary>
        Task ExecuteOperationsAsync(ISession session);
    }
}
