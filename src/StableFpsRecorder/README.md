# StableFPS Recorder - Raw Media Recording Demo

## Overview

This WPF application demonstrates how to use the `RawSource` class from the `MilestoneSystems.VideoOS.Platform.SDK.Core` package to record raw media data from a Milestone XProtect VMS. It supports both live and recorded (playback) modes and writes the received raw data directly to a file.
As an added bonus it writes the data in a format that is compatible with the StableFPS driver, so it can be used to feed a StableFPS virtual camera with raw data from the VMS.

## Features

- **Session Management**: Connect to a Milestone XProtect server using Windows Default, Windows, or Basic credentials
- **Device Browser**: Browse and select cameras, microphones, speakers, and metadata devices via an interactive tree dialog
- **Live and Playback Modes**:
  - Live: streams and records data in real time
  - Recorded: streams and records data from a specified UTC start time
- **Raw Data Output**: Writes received raw media data directly to a `.raw` output file
- **Bytes Written Counter**: Displays a running tally of how much data has been written

## Architecture

### Key Components

#### MainWindow
The main application window, responsible for:
- Login and session establishment using `Session` and `IMipCredentials`
- Device selection via `DevicePickerDialog`
- Starting and stopping a `RawSource` instance
- Writing received `RawData` to a `FileStream`
- Displaying status and progress to the user

#### DevicePickerDialog
A WPF dialog that lets the user browse the VMS device tree and select a device. It uses the `ConfigurationService` to load device groups (Camera, Microphone, Speaker, Metadata) and lazily loads children when the user expands a group node.

#### DeviceTreeNode
A view model class representing a node in the device tree. Supports lazy loading via a `Loader` delegate, which is invoked when the node is expanded for the first time.

#### RawSource
The central SDK class used to connect to a device stream. Configured with:
- The active `ISession`
- A `RawSourceMode` (Live or Playback)
- A device path (e.g. `camera/<guid>`)
- Optionally a `PlaybackStartTime` for recorded mode

Events:
- `DataReady`: raised when a new chunk of raw data is available
- `Error`: raised when a stream error occurs

## Operation Flow

1. **Login**: Enter the server URI and credentials, then click **Login** to establish a session
2. **Select Device**: Click **Browse…** to open the device picker and select a camera, microphone, speaker, or metadata device
3. **Choose Mode**: Select **Live** or **Recorded**; for recorded mode, enter a UTC start time
4. **Choose Output File**: Click **Browse…** to pick a destination `.raw` file
5. **Start Recording**: Click **Start** — the application opens the file, creates a `RawSource`, and begins receiving and writing data
6. **Stop Recording**: Click **Stop** to flush and close the output file and stop the stream

## Usage

1. Run the application
2. Enter the server URI (e.g. `http://localhost`) and select a login type
3. Click **Login**
4. Click **Browse…** next to Device and select a device from the tree
5. Select Live or Recorded mode (and enter a start time for Recorded)
6. Click **Browse…** next to Output File and choose a file path
7. Click **Start** to begin recording
8. Click **Stop** when done

## Error Handling

- Login errors are displayed inline with a red status message
- Stream errors raised by `RawSource` are displayed in the error text area
- Cleanup of the `RawSource` and output file is performed on both normal stop and error conditions

## Dependencies

- MilestoneSystems.VideoOS.Platform.SDK.Core
- Microsoft.Extensions.DependencyInjection

## This sample demonstrates:

- Establishing a session using `Session` and `IMipCredentials` (Windows Default, Windows, and Basic credential types)
- Using `RawSource` with `RawSourceMode.Live` and `RawSourceMode.Playback` to stream raw media data
- Browsing VMS configuration items (`CameraGroup`, `Camera`, `MicrophoneGroup`, `Microphone`, `SpeakerGroup`, `Speaker`, `MetadataGroup`, `Metadata`) using `ConfigurationService`
- Lazy-loading child nodes in a WPF `TreeView` using a delegate-based loader pattern
- Handling `DataReady` and `Error` events from `RawSource`
- Writing raw media bytes to a file stream and tracking progress
