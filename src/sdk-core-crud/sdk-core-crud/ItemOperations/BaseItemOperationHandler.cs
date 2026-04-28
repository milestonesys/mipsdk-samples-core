using VideoOS.Platform.SDK.Core;
using VideoOS.Platform.SDK.Core.Configuration;
using VideoOS.Platform.SDK.Core.Configuration.Filtering;

namespace sdk_core_crud.ItemOperations
{
    /// <summary>
    /// Base class for handling operations on configuration items
    /// </summary>
    /// <typeparam name="T">The type of configuration item</typeparam>
    internal abstract class BaseItemOperationHandler<T> : IItemOperationHandler where T : ConfigurationItemBase, new()
    {
        public abstract string ItemTypeName { get; }
        public abstract bool SupportsAddDelete { get; }

        protected abstract string GetDisplayName(T item);
        protected abstract string GetEditablePropertyName();
        protected abstract object GetEditablePropertyValue(T item);
        protected abstract void SetEditablePropertyValue(T item, object value);
        protected abstract string GetFilterPropertyName();
        protected abstract object GetFilterPropertyValue(T item);
        protected abstract T CreateNewItem(string name);

        public async Task ExecuteOperationsAsync(ISession session)
        {
            Console.WriteLine($"\n========== {ItemTypeName} Operations ==========\n");

            // Track original state for restoration
            List<Guid> originalItemIds = new List<Guid>();
            Dictionary<Guid, object> originalPropertyValues = new Dictionary<Guid, object>();
            Guid? addedItemId = null;

            try
            {
                // Step 1: Retrieve all items
                Console.WriteLine($"1. Retrieving all {ItemTypeName} items...");
                var allItems = await session.Configuration.Get<T>();
                var itemsList = allItems.ToList();
                originalItemIds = itemsList.Select(i => i.Id).ToList();
                
                Console.WriteLine($"   Found {itemsList.Count} {ItemTypeName} item(s)");
                foreach (var item in itemsList.Take(5))
                {
                    Console.WriteLine($"   - {GetDisplayName(item)} (ID: {item.Id})");
                }
                if (itemsList.Count > 5)
                {
                    Console.WriteLine($"   ... and {itemsList.Count - 5} more");
                }

                if (!itemsList.Any())
                {
                    Console.WriteLine($"   No {ItemTypeName} items found. Skipping remaining operations.");
                    return;
                }

                // Step 2: Retrieve single item by ID
                Console.WriteLine($"\n2. Retrieving single {ItemTypeName} by ID...");
                var firstItem = itemsList.First();
                var singleItem = await session.Configuration.Get<T>(firstItem.Id);
                if (singleItem != null)
                {
                    Console.WriteLine($"   Retrieved: {GetDisplayName(singleItem)} (ID: {singleItem.Id})");
                }

                // Step 3: Retrieve items by property filter
                Console.WriteLine($"\n3. Retrieving {ItemTypeName} items by property filter...");
                var filterValue = GetFilterPropertyValue(firstItem);
                var propertyFilter = new Filter 
                { 
                    Field = GetFilterPropertyName(), 
                    Value = filterValue,
                    Operator = FilterOperator.Equal
                };
                var filteredItems = await session.Configuration.Get<T>(new[] { propertyFilter });
                var filteredList = filteredItems.ToList();
                Console.WriteLine($"   Found {filteredList.Count} {ItemTypeName} item(s) with {GetFilterPropertyName()} = {filterValue}");
                foreach (var item in filteredList.Take(3))
                {
                    Console.WriteLine($"   - {GetDisplayName(item)}");
                }

                // Step 4: Edit property and save
                Console.WriteLine($"\n4. Editing property on {ItemTypeName} item...");
                var itemToEdit = itemsList.First();
                var originalValue = GetEditablePropertyValue(itemToEdit);
                originalPropertyValues[itemToEdit.Id] = originalValue;
                
                Console.WriteLine($"   Original {GetEditablePropertyName()}: {originalValue}");
                
                var newValue = GenerateNewValue(originalValue);
                SetEditablePropertyValue(itemToEdit, newValue);
                Console.WriteLine($"   New {GetEditablePropertyName()}: {newValue}");

                await itemToEdit.Save();
                Console.WriteLine($"   Saved successfully");

                // Verify the edit
                var verifyItem = await session.Configuration.Get<T>(itemToEdit.Id);
                if (verifyItem != null)
                {
                    var verifyValue = GetEditablePropertyValue(verifyItem);
                    if (!string.Equals(newValue?.ToString(), verifyValue?.ToString(), StringComparison.Ordinal))
                    {
                        Console.WriteLine($"Edited value is {verifyValue}, edit failed");
                    }
                    else
                    {
                        Console.WriteLine($"   Verified {GetEditablePropertyName()}: {verifyValue}");
                    }
                }

                // Steps 5 & 6: Add and Delete (if supported)
                if (SupportsAddDelete)
                {
                    // Step 5: Add new item
                    Console.WriteLine($"\n5. Adding new {ItemTypeName} item...");
                    var newItem = CreateNewItem($"Test_{ItemTypeName}_{DateTime.Now.Ticks}");
                    newItem = await session.Configuration.Create(newItem);
                    addedItemId = newItem.Id;
                    Console.WriteLine($"   Added: {GetDisplayName(newItem)} (ID: {newItem.Id})");

                    // Verify add
                    var verifyAdd = await session.Configuration.Get<T>(newItem.Id);
                    if (verifyAdd != null)
                    {
                        Console.WriteLine($"   Verified addition: {GetDisplayName(verifyAdd)}");
                    }

                    // Step 6: Delete the added item
                    Console.WriteLine($"\n6. Deleting {ItemTypeName} item...");
                    var result = await newItem.Delete();
                    Console.WriteLine($"   Deleted item with ID: {addedItemId}");

                    // Verify delete
                    //TODO: The documentation claims we return null if item is not found. We throw exception instead. MAKE UP YOUR MIND
                    var verifyDelete = result; // await session.Configuration.Get<T>(addedItemId.Value);
                    if (verifyDelete) // verifyDelete == null
                    {
                        Console.WriteLine($"   Verified deletion: Item no longer exists");
                        addedItemId = null; // Successfully deleted, no need to clean up
                    }
                }
                else
                {
                    Console.WriteLine($"\n5-6. Add/Delete operations not implemented for {ItemTypeName}");
                    Console.WriteLine("This can be due to the item type being a device type or because add/delete operations require additional properties that are not available in this simplified example.");
                }

                // Restoration: Restore edited property
                Console.WriteLine($"\n========== Restoring Original State ==========");
                Console.WriteLine($"Restoring edited properties...");
                foreach (var kvp in originalPropertyValues)
                {
                    var itemToRestore = await session.Configuration.Get<T>(kvp.Key);
                    if (itemToRestore != null)
                    {
                        SetEditablePropertyValue(itemToRestore, kvp.Value);
                        await itemToRestore.Save();
                        Console.WriteLine($"   Restored {GetEditablePropertyName()} for item {kvp.Key}");
                    }
                }

                Console.WriteLine($"State restoration complete. System returned to original state.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError during {ItemTypeName} operations: {ex.Message}");
                
                // Attempt cleanup on error
                try
                {
                    if (addedItemId.HasValue)
                    {
                        var itemToDelete = await session.Configuration.Get<T>(addedItemId.Value);
                        if (itemToDelete != null)
                        {
                            await itemToDelete.Delete();
                            Console.WriteLine($"Cleaned up added item {addedItemId}");
                        }
                    }

                    foreach (var kvp in originalPropertyValues)
                    {
                        var itemToRestore = await session.Configuration.Get<T>(kvp.Key);
                        if (itemToRestore != null)
                        {
                            SetEditablePropertyValue(itemToRestore, kvp.Value);
                            await itemToRestore.Save();
                        }
                    }
                }
                catch (Exception cleanupEx)
                {
                    Console.WriteLine($"Error during cleanup: {cleanupEx.Message}");
                }
            }

            Console.WriteLine($"\n========== {ItemTypeName} Operations Complete ==========\n");
        }

        private object GenerateNewValue(object originalValue)
        {
            if (originalValue is string strValue)
            {
                return strValue + "_Modified";
            }
            else if (originalValue is int intValue)
            {
                return intValue + 1;
            }
            else if (originalValue is double doubleValue)
            {
                return doubleValue + 1.0;
            }
            else if (originalValue is bool boolValue)
            {
                return !boolValue;
            }
            else if (originalValue is Guid guidValue)
            {
                return Guid.NewGuid();
            }
            
            return originalValue;
        }
    }
}
