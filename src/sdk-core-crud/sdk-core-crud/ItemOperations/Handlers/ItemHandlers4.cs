using VideoOS.Platform.SDK.Core.Configuration.Items;

namespace sdk_core_crud.ItemOperations.Handlers
{
    internal class RoleHandler : BaseItemOperationHandler<Role>
    {
        public override string ItemTypeName => "Role";
        public override bool SupportsAddDelete => true;

        protected override string GetDisplayName(Role item) => item.Name ?? "Unnamed";
        protected override string GetEditablePropertyName() => "Name";
        protected override object GetEditablePropertyValue(Role item) => item.Name ?? "";
        protected override void SetEditablePropertyValue(Role item, object value) => item.Name = value?.ToString();
        protected override string GetFilterPropertyName() => "Name";
        protected override object GetFilterPropertyValue(Role item) => item.Name ?? "";
        protected override Role CreateNewItem(string name)
        {
            return new Role { Name = name };
        }
    }

    internal class SiteHandler : BaseItemOperationHandler<Site>
    {
        public override string ItemTypeName => "Site";
        public override bool SupportsAddDelete => false;

        protected override string GetDisplayName(Site item) => item.Name ?? "Unnamed";
        protected override string GetEditablePropertyName() => "Name";
        protected override object GetEditablePropertyValue(Site item) => item.Name ?? "";
        protected override void SetEditablePropertyValue(Site item, object value) => item.Name = value?.ToString();
        protected override string GetFilterPropertyName() => "Name";
        protected override object GetFilterPropertyValue(Site item) => item.Name ?? "";
        protected override Site CreateNewItem(string name)
        {
            return new Site { Name = name };
        }
    }

    internal class SpeakerHandler : BaseItemOperationHandler<Speaker>
    {
        public override string ItemTypeName => "Speaker";
        public override bool SupportsAddDelete => false; // Device type

        protected override string GetDisplayName(Speaker item) => item.Name ?? "Unnamed";
        protected override string GetEditablePropertyName() => "Description";
        protected override object GetEditablePropertyValue(Speaker item) => item.Description ?? "";
        protected override void SetEditablePropertyValue(Speaker item, object value) => item.Description = value?.ToString();
        protected override string GetFilterPropertyName() => "Name";
        protected override object GetFilterPropertyValue(Speaker item) => item.Name ?? "";
        protected override Speaker CreateNewItem(string name) => throw new NotSupportedException();
    }

    internal class SpeakerGroupHandler : BaseItemOperationHandler<SpeakerGroup>
    {
        public override string ItemTypeName => "SpeakerGroup";
        public override bool SupportsAddDelete => true;

        protected override string GetDisplayName(SpeakerGroup item) => item.Name ?? "Unnamed";
        protected override string GetEditablePropertyName() => "Name";
        protected override object GetEditablePropertyValue(SpeakerGroup item) => item.Name ?? "";
        protected override void SetEditablePropertyValue(SpeakerGroup item, object value) => item.Name = value?.ToString();
        protected override string GetFilterPropertyName() => "Name";
        protected override object GetFilterPropertyValue(SpeakerGroup item) => item.Name ?? "";
        protected override SpeakerGroup CreateNewItem(string name)
        {
            return new SpeakerGroup { Name = name, Description = name };
        }
    }

    internal class UserDefinedEventHandler : BaseItemOperationHandler<UserDefinedEvent>
    {
        public override string ItemTypeName => "UserDefinedEvent";
        public override bool SupportsAddDelete => true;

        protected override string GetDisplayName(UserDefinedEvent item) => item.Name ?? "Unnamed";
        protected override string GetEditablePropertyName() => "Name";
        protected override object GetEditablePropertyValue(UserDefinedEvent item) => item.Name ?? "";
        protected override void SetEditablePropertyValue(UserDefinedEvent item, object value) => item.Name = value?.ToString();
        protected override string GetFilterPropertyName() => "Name";
        protected override object GetFilterPropertyValue(UserDefinedEvent item) => item.Name ?? "";
        protected override UserDefinedEvent CreateNewItem(string name)
        {
            return new UserDefinedEvent { Name = name };
        }
    }
}
