using System;
using System.Collections.Generic;
using System.Text;
using VideoOS.Platform.SDK.Core.Configuration.Items;

namespace sdk_core_crud.ItemOperations.Handlers
{
    internal class AlarmDefinitionHandler : BaseItemOperationHandler<AlarmDefinition>
    {
        public override bool SupportsAddDelete => false;

        protected override string GetEditablePropertyName() => "Name";
        protected override object GetEditablePropertyValue(AlarmDefinition item) => item.Name ?? "";
        protected override void SetEditablePropertyValue(AlarmDefinition item, object value) => item.Name = value?.ToString();
        protected override AlarmDefinition CreateNewItem(string name)
        {
            throw new NotImplementedException("Creating new AlarmDefinition items requires setting EventType, EventTypeGroup, and SourceList, which are not available to this simplified create method.");
        }
    }

    internal class BasicUserHandler : BaseItemOperationHandler<BasicUser>
    {
        public override bool SupportsAddDelete => true;

        protected override string GetEditablePropertyName() => "Description";
        protected override object GetEditablePropertyValue(BasicUser item) => item.Description ?? "";
        protected override void SetEditablePropertyValue(BasicUser item, object value) => item.Description = value?.ToString();
        protected override BasicUser CreateNewItem(string name)
        {
            //NOTE: In a real implementation, you would want to allow setting the password and status when creating a new user, but for this example we'll use default values.
            //Never hardcode passwords in production code. This is just for demonstration purposes.
            return new BasicUser { Name = name, Status = BasicUserStatus.Enabled, Password = "TestPassword123!" };
        }
    }

    internal class CameraHandler : BaseItemOperationHandler<Camera>
    {
        public override bool SupportsAddDelete => false; // Device type

        protected override string GetEditablePropertyName() => "Description";
        protected override object GetEditablePropertyValue(Camera item) => item.Description ?? "";
        protected override void SetEditablePropertyValue(Camera item, object value) => item.Description = value?.ToString();
        protected override Camera CreateNewItem(string name) => throw new NotSupportedException();
    }

    internal class CameraGroupHandler : BaseItemOperationHandler<CameraGroup>
    {
        public override bool SupportsAddDelete => true;

        protected override string GetEditablePropertyName() => "Name";
        protected override object GetEditablePropertyValue(CameraGroup item) => item.Name ?? "";
        protected override void SetEditablePropertyValue(CameraGroup item, object value) => item.Name = value?.ToString();
        protected override CameraGroup CreateNewItem(string name)
        {
            return new CameraGroup { Name = name, Description = name };
        }
    }

    internal class HardwareHandler : BaseItemOperationHandler<Hardware>
    {
        public override bool SupportsAddDelete => false;

        protected override string GetEditablePropertyName() => "Description";
        protected override object GetEditablePropertyValue(Hardware item) => item.Description ?? "";
        protected override void SetEditablePropertyValue(Hardware item, object value) => item.Description = value?.ToString();
        protected override Hardware CreateNewItem(string name) => throw new NotSupportedException();
    }

    internal class InputEventHandler : BaseItemOperationHandler<InputEvent>
    {
        public override bool SupportsAddDelete => false; // Device type

        protected override string GetEditablePropertyName() => "Description";
        protected override object GetEditablePropertyValue(InputEvent item) => item.Description ?? "";
        protected override void SetEditablePropertyValue(InputEvent item, object value) => item.Description = value?.ToString();
        protected override InputEvent CreateNewItem(string name) => throw new NotSupportedException();
    }

    internal class InputEventGroupHandler : BaseItemOperationHandler<InputEventGroup>
    {
        public override bool SupportsAddDelete => true;

        protected override string GetEditablePropertyName() => "Name";
        protected override object GetEditablePropertyValue(InputEventGroup item) => item.Name ?? "";
        protected override void SetEditablePropertyValue(InputEventGroup item, object value) => item.Name = value?.ToString();
        protected override InputEventGroup CreateNewItem(string name)
        {
            return new InputEventGroup { Name = name, Description = name };
        }
    }

    internal class MetadataHandler : BaseItemOperationHandler<Metadata>
    {
        public override bool SupportsAddDelete => false; // Device type

        protected override string GetEditablePropertyName() => "Description";
        protected override object GetEditablePropertyValue(Metadata item) => item.Description ?? "";
        protected override void SetEditablePropertyValue(Metadata item, object value) => item.Description = value?.ToString();
        protected override Metadata CreateNewItem(string name) => throw new NotSupportedException();
    }

    internal class MetadataGroupHandler : BaseItemOperationHandler<MetadataGroup>
    {
        public override bool SupportsAddDelete => true;

        protected override string GetEditablePropertyName() => "Name";
        protected override object GetEditablePropertyValue(MetadataGroup item) => item.Name ?? "";
        protected override void SetEditablePropertyValue(MetadataGroup item, object value) => item.Name = value?.ToString();
        protected override MetadataGroup CreateNewItem(string name)
        {
            return new MetadataGroup { Name = name, Description = name };
        }
    }

    internal class MicrophoneHandler : BaseItemOperationHandler<Microphone>
    {
        public override bool SupportsAddDelete => false; // Device type

        protected override string GetEditablePropertyName() => "Description";
        protected override object GetEditablePropertyValue(Microphone item) => item.Description ?? "";
        protected override void SetEditablePropertyValue(Microphone item, object value) => item.Description = value?.ToString();
        protected override Microphone CreateNewItem(string name) => throw new NotSupportedException();
    }

    internal class MicrophoneGroupHandler : BaseItemOperationHandler<MicrophoneGroup>
    {
        public override bool SupportsAddDelete => true;

        protected override string GetEditablePropertyName() => "Name";
        protected override object GetEditablePropertyValue(MicrophoneGroup item) => item.Name ?? "";
        protected override void SetEditablePropertyValue(MicrophoneGroup item, object value) => item.Name = value?.ToString();
        protected override MicrophoneGroup CreateNewItem(string name)
        {
            return new MicrophoneGroup { Name = name, Description = name };
        }
    }

    internal class OutputHandler : BaseItemOperationHandler<Output>
    {
        public override bool SupportsAddDelete => false; // Device type

        protected override string GetEditablePropertyName() => "Description";
        protected override object GetEditablePropertyValue(Output item) => item.Description ?? "";
        protected override void SetEditablePropertyValue(Output item, object value) => item.Description = value?.ToString();
        protected override Output CreateNewItem(string name) => throw new NotSupportedException();
    }

    internal class OutputGroupHandler : BaseItemOperationHandler<OutputGroup>
    {
        public override bool SupportsAddDelete => true;

        protected override string GetEditablePropertyName() => "Name";
        protected override object GetEditablePropertyValue(OutputGroup item) => item.Name ?? "";
        protected override void SetEditablePropertyValue(OutputGroup item, object value) => item.Name = value?.ToString();
        protected override OutputGroup CreateNewItem(string name)
        {
            return new OutputGroup { Name = name, Description = name };
        }
    }

    internal class RecordingServerHandler : BaseItemOperationHandler<RecordingServer>
    {
        public override bool SupportsAddDelete => false;

        protected override string GetEditablePropertyName() => "Name";
        protected override object GetEditablePropertyValue(RecordingServer item) => item.Name ?? "";
        protected override void SetEditablePropertyValue(RecordingServer item, object value) => item.Name = value?.ToString();
        protected override RecordingServer CreateNewItem(string name) => throw new NotSupportedException();
    }

    internal class RoleHandler : BaseItemOperationHandler<Role>
    {
        public override bool SupportsAddDelete => true;

        protected override string GetEditablePropertyName() => "Name";
        protected override object GetEditablePropertyValue(Role item) => item.Name ?? "";
        protected override void SetEditablePropertyValue(Role item, object value) => item.Name = value?.ToString();
        protected override Role CreateNewItem(string name)
        {
            return new Role { Name = name };
        }
    }

    internal class SiteHandler : BaseItemOperationHandler<Site>
    {
        public override bool SupportsAddDelete => false;

        protected override string GetEditablePropertyName() => "Name";
        protected override object GetEditablePropertyValue(Site item) => item.Name ?? "";
        protected override void SetEditablePropertyValue(Site item, object value) => item.Name = value?.ToString();
        protected override Site CreateNewItem(string name) => throw new NotSupportedException();
    }

    internal class SpeakerHandler : BaseItemOperationHandler<Speaker>
    {
        public override bool SupportsAddDelete => false; // Device type

        protected override string GetEditablePropertyName() => "Description";
        protected override object GetEditablePropertyValue(Speaker item) => item.Description ?? "";
        protected override void SetEditablePropertyValue(Speaker item, object value) => item.Description = value?.ToString();
        protected override Speaker CreateNewItem(string name) => throw new NotSupportedException();
    }

    internal class SpeakerGroupHandler : BaseItemOperationHandler<SpeakerGroup>
    {
        public override bool SupportsAddDelete => true;

        protected override string GetEditablePropertyName() => "Name";
        protected override object GetEditablePropertyValue(SpeakerGroup item) => item.Name ?? "";
        protected override void SetEditablePropertyValue(SpeakerGroup item, object value) => item.Name = value?.ToString();
        protected override SpeakerGroup CreateNewItem(string name)
        {
            return new SpeakerGroup { Name = name, Description = name };
        }
    }

    internal class UserDefinedEventHandler : BaseItemOperationHandler<UserDefinedEvent>
    {
        public override bool SupportsAddDelete => true;

        protected override string GetEditablePropertyName() => "Name";
        protected override object GetEditablePropertyValue(UserDefinedEvent item) => item.Name ?? "";
        protected override void SetEditablePropertyValue(UserDefinedEvent item, object value) => item.Name = value?.ToString();
        protected override UserDefinedEvent CreateNewItem(string name)
        {
            return new UserDefinedEvent { Name = name };
        }
    }
}
