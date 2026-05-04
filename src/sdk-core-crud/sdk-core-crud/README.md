# ConsoleAppCrud - Configuration Item CRUD Operations Demo

## Overview

This console application demonstrates comprehensive CRUD (Create, Read, Update, Delete) operations on 19 configuration item types from the `MilestoneSystems.VideoOS.Platform.SDK.Core` package.

## Features

- **Interactive Menu**: Select from 19 configuration item types to perform operations
- **Comprehensive Operations**:
  - Retrieve all items of a type
  - Retrieve single item by ID
  - Retrieve items filtered by property
  - Edit and save item properties
  - Add new items (where supported)
  - Delete items (where supported)
- **State Restoration**: All operations automatically restore the system to its original state after completion
- **Device Type Handling**: Properly handles device types (Camera, Microphone, Speaker, Metadata, InputEvent, Output) and Hardware that don't support add/delete operations

## Supported Item Types

### Types with Full CRUD Support (Add/Delete enabled):
1. BasicUser
2. CameraGroup
3. InputEventGroup
4. MetadataGroup
5. MicrophoneGroup
6. OutputGroup
7. Role
8. SpeakerGroup
9. UserDefinedEvent

### Types with Read/Update Only
1. AlarmDefinition (explicitly excluded from add/delete)
2. Camera (inherits from StreamingDevice)
3. Hardware (explicitly excluded from add/delete)
4. InputEvent (inherits from Device)
5. Metadata (inherits from Device)
6. Microphone (inherits from Device)
7. Output (inherits from Device)
8. RecordingServer (explicitly excluded from add/delete)
9. Site (explicitly excluded from add/delete)
10. Speaker (inherits from Device)

## Architecture

### Key Components

#### IItemOperationHandler
Interface defining the contract for all item operation handlers:
- `ItemTypeName`: Name of the configuration item type
- `SupportsAddDelete`: Indicates if add/delete operations are supported
- `ExecuteOperationsAsync`: Main method to execute all operations

#### BaseItemOperationHandler<T>
Abstract base class providing common CRUD logic for all handlers:
- Retrieve operations (all, by ID, by filter)
- Edit and save operations
- Add/delete operations (when supported)
- Automatic state restoration
- Error handling and cleanup

#### Concrete Handlers
19 specific handler implementations (in ItemHandlers1.cs through ItemHandlers4.cs), each configured for its specific item type.

Some item types are excluded from this sample, such as ArchiveStorage and HardwareDriver which require looking them up through the RecordingServer item.

## Operation Flow

For each selected item type, the application performs the following sequence:

1. **Retrieve All Items**: Lists all items of the selected type (shows first 5)
2. **Retrieve by ID**: Fetches a single item using its unique identifier
3. **Retrieve by Filter**: Queries items based on a property filter (e.g., Name contains specific value)
4. **Edit Property**: Modifies a property on an item and saves the change
5. **Add Item** (if supported): Creates a new item with a timestamped name
6. **Delete Item** (if supported): Removes the newly created item
7. **Restore State**: Reverts all property changes to restore original values

### State Restoration

The application ensures that after all operations complete:
- Any property modifications are reverted to original values
- Any items added during the demo are deleted
- The system state is exactly as it was before operations began

This is achieved by:
- Tracking original property values before modification
- Tracking IDs of added items
- Executing restoration logic in a finally block
- Providing cleanup on error conditions

## Usage

1. Run the application and establish a session using SessionHelper
2. Select a configuration item type from the numbered menu
3. Watch as the application performs all operations automatically
4. Press any key to return to the menu
5. Select another type or choose Exit

## Error Handling

- Comprehensive try-catch blocks around all operations
- Automatic cleanup of added items on error
- Restoration of modified properties on error
- Detailed error messages with stack traces
- Graceful handling of missing items (skips remaining operations)

## Dependencies

- MilestoneSystems.VideoOS.Platform.SDK.Core
- Microsoft.Extensions.DependencyInjection

## This sample demonstrates:

- Establishing a session, and using the AddMipServices extension method
- Device types (Camera, Microphone, Speaker, Metadata, InputEvent, Output) are hardware-backed and cannot be arbitrarily created or deleted
- Hardware items represent physical hardware and similarly cannot be added/deleted through the API
- Most other types support full CRUD operations, but RecordingServer and Site do not, while AlarmDefinition is excluded because it requires extra information to perform a create
- Property filtering uses the `Filter` class with `FilterOperator.Contains`
