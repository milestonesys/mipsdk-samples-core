using VideoOS.Platform.SDK.Core.Configuration;
using VideoOS.Platform.SDK.Core.Configuration.Items;

namespace sdk_core_crud.ItemOperations.Handlers
{
    internal class AlarmDefinitionHandler : BaseItemOperationHandler<AlarmDefinition>
    {
        public override string ItemTypeName => "AlarmDefinition";
        public override bool SupportsAddDelete => false;

        protected override string GetDisplayName(AlarmDefinition item) => item.Name ?? "Unnamed";
        protected override string GetEditablePropertyName() => "Name";
        protected override object GetEditablePropertyValue(AlarmDefinition item) => item.Name ?? "";
        protected override void SetEditablePropertyValue(AlarmDefinition item, object value) => item.Name = value?.ToString();
        protected override string GetFilterPropertyName() => "Name";
        protected override object GetFilterPropertyValue(AlarmDefinition item) => item.Name ?? "";
        protected override AlarmDefinition CreateNewItem(string name)
        {
            throw new NotImplementedException("Creating new AlarmDefinition items requires setting EventType, EventTypeGroup, and SourceList, which are not available to this simplified create method.");
        }
    }

    internal class BasicUserHandler : BaseItemOperationHandler<BasicUser>
    {
        public override string ItemTypeName => "BasicUser";
        public override bool SupportsAddDelete => true;

        protected override string GetDisplayName(BasicUser item) => item.Name ?? "Unnamed";
        protected override string GetEditablePropertyName() => "Description";
        protected override object GetEditablePropertyValue(BasicUser item) => item.Description ?? "";
        protected override void SetEditablePropertyValue(BasicUser item, object value) => item.Description = value?.ToString();
        protected override string GetFilterPropertyName() => "Name";
        protected override object GetFilterPropertyValue(BasicUser item) => item.Name ?? "";
        protected override BasicUser CreateNewItem(string name)
        {
            return new BasicUser { Name = name, Status = BasicUserStatus.Enabled, Password = "TestPassword123!" };
        }
    }

    internal class CameraHandler : BaseItemOperationHandler<Camera>
    {
        public override string ItemTypeName => "Camera";
        public override bool SupportsAddDelete => false; // Device type

        protected override string GetDisplayName(Camera item) => item.Name ?? "Unnamed";
        protected override string GetEditablePropertyName() => "Description";
        protected override object GetEditablePropertyValue(Camera item) => item.Description ?? "";
        protected override void SetEditablePropertyValue(Camera item, object value) => item.Description = value?.ToString();
        protected override string GetFilterPropertyName() => "Name";
        protected override object GetFilterPropertyValue(Camera item) => item.Name ?? "";
        protected override Camera CreateNewItem(string name) => throw new NotSupportedException();
    }

    internal class CameraGroupHandler : BaseItemOperationHandler<CameraGroup>
    {
        public override string ItemTypeName => "CameraGroup";
        public override bool SupportsAddDelete => true;

        protected override string GetDisplayName(CameraGroup item) => item.Name ?? "Unnamed";
        protected override string GetEditablePropertyName() => "Name";
        protected override object GetEditablePropertyValue(CameraGroup item) => item.Name ?? "";
        protected override void SetEditablePropertyValue(CameraGroup item, object value) => item.Name = value?.ToString();
        protected override string GetFilterPropertyName() => "Name";
        protected override object GetFilterPropertyValue(CameraGroup item) => item.Name ?? "";
        protected override CameraGroup CreateNewItem(string name)
        {
            return new CameraGroup { Name = name, Description = name };
        }
    }

    internal class HardwareHandler : BaseItemOperationHandler<Hardware>
    {
        public override string ItemTypeName => "Hardware";
        public override bool SupportsAddDelete => false; // Explicitly mentioned

        protected override string GetDisplayName(Hardware item) => item.Name ?? "Unnamed";
        protected override string GetEditablePropertyName() => "Description";
        protected override object GetEditablePropertyValue(Hardware item) => item.Description ?? "";
        protected override void SetEditablePropertyValue(Hardware item, object value) => item.Description = value?.ToString();
        protected override string GetFilterPropertyName() => "Name";
        protected override object GetFilterPropertyValue(Hardware item) => item.Name ?? "";
        protected override Hardware CreateNewItem(string name) => throw new NotSupportedException();
    }
}
