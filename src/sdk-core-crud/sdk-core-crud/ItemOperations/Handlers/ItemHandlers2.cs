using System.Web;
using VideoOS.Platform.SDK.Core.Configuration;
using VideoOS.Platform.SDK.Core.Configuration.Items;

namespace sdk_core_crud.ItemOperations.Handlers
{
    internal class InputEventHandler : BaseItemOperationHandler<InputEvent>
    {
        public override string ItemTypeName => "InputEvent";
        public override bool SupportsAddDelete => false; // Device type

        protected override string GetDisplayName(InputEvent item) => item.Name ?? "Unnamed";
        protected override string GetEditablePropertyName() => "Description";
        protected override object GetEditablePropertyValue(InputEvent item) => item.Description ?? "";
        protected override void SetEditablePropertyValue(InputEvent item, object value) => item.Description = value?.ToString();
        protected override string GetFilterPropertyName() => "Name";
        protected override object GetFilterPropertyValue(InputEvent item) => item.Name ?? "";
        protected override InputEvent CreateNewItem(string name) => throw new NotSupportedException();
    }

    internal class InputEventGroupHandler : BaseItemOperationHandler<InputEventGroup>
    {
        public override string ItemTypeName => "InputEventGroup";
        public override bool SupportsAddDelete => true;

        protected override string GetDisplayName(InputEventGroup item) => item.Name ?? "Unnamed";
        protected override string GetEditablePropertyName() => "Name";
        protected override object GetEditablePropertyValue(InputEventGroup item) => item.Name ?? "";
        protected override void SetEditablePropertyValue(InputEventGroup item, object value) => item.Name = value?.ToString();
        protected override string GetFilterPropertyName() => "Name";
        protected override object GetFilterPropertyValue(InputEventGroup item) => item.Name ?? "";
        protected override InputEventGroup CreateNewItem(string name)
        {
            return new InputEventGroup { Name = name, Description = name };
        }
    }

    internal class MetadataHandler : BaseItemOperationHandler<Metadata>
    {
        public override string ItemTypeName => "Metadata";
        public override bool SupportsAddDelete => false; // Device type

        protected override string GetDisplayName(Metadata item) => item.Name ?? "Unnamed";
        protected override string GetEditablePropertyName() => "Description";
        protected override object GetEditablePropertyValue(Metadata item) => item.Description ?? "";
        protected override void SetEditablePropertyValue(Metadata item, object value) => item.Description = value?.ToString();
        protected override string GetFilterPropertyName() => "Name";
        protected override object GetFilterPropertyValue(Metadata item) => HttpUtility.UrlEncode(item.Name ?? "");
        protected override Metadata CreateNewItem(string name) => throw new NotSupportedException();
    }

    internal class MetadataGroupHandler : BaseItemOperationHandler<MetadataGroup>
    {
        public override string ItemTypeName => "MetadataGroup";
        public override bool SupportsAddDelete => true;

        protected override string GetDisplayName(MetadataGroup item) => item.Name ?? "Unnamed";
        protected override string GetEditablePropertyName() => "Name";
        protected override object GetEditablePropertyValue(MetadataGroup item) => item.Name ?? "";
        protected override void SetEditablePropertyValue(MetadataGroup item, object value) => item.Name = value?.ToString();
        protected override string GetFilterPropertyName() => "Name";
        protected override object GetFilterPropertyValue(MetadataGroup item) => item.Name ?? "";
        protected override MetadataGroup CreateNewItem(string name)
        {
            return new MetadataGroup { Name = name, Description = name };
        }
    }

    internal class MicrophoneHandler : BaseItemOperationHandler<Microphone>
    {
        public override string ItemTypeName => "Microphone";
        public override bool SupportsAddDelete => false; // Device type

        protected override string GetDisplayName(Microphone item) => item.Name ?? "Unnamed";
        protected override string GetEditablePropertyName() => "Description";
        protected override object GetEditablePropertyValue(Microphone item) => item.Description ?? "";
        protected override void SetEditablePropertyValue(Microphone item, object value) => item.Description = value?.ToString();
        protected override string GetFilterPropertyName() => "Name";
        protected override object GetFilterPropertyValue(Microphone item) => item.Name ?? "";
        protected override Microphone CreateNewItem(string name) => throw new NotSupportedException();
    }

    internal class MicrophoneGroupHandler : BaseItemOperationHandler<MicrophoneGroup>
    {
        public override string ItemTypeName => "MicrophoneGroup";
        public override bool SupportsAddDelete => true;

        protected override string GetDisplayName(MicrophoneGroup item) => item.Name ?? "Unnamed";
        protected override string GetEditablePropertyName() => "Name";
        protected override object GetEditablePropertyValue(MicrophoneGroup item) => item.Name ?? "";
        protected override void SetEditablePropertyValue(MicrophoneGroup item, object value) => item.Name = value?.ToString();
        protected override string GetFilterPropertyName() => "Name";
        protected override object GetFilterPropertyValue(MicrophoneGroup item) => item.Name ?? "";
        protected override MicrophoneGroup CreateNewItem(string name)
        {
            return new MicrophoneGroup { Name = name, Description = name };
        }
    }
}
