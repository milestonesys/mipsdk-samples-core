using VideoOS.Platform.SDK.Core.Configuration;
using VideoOS.Platform.SDK.Core.Configuration.Items;

namespace sdk_core_crud.ItemOperations.Handlers
{
    internal class OutputHandler : BaseItemOperationHandler<Output>
    {
        public override string ItemTypeName => "Output";
        public override bool SupportsAddDelete => false; // Device type

        protected override string GetDisplayName(Output item) => item.Name ?? "Unnamed";
        protected override string GetEditablePropertyName() => "Description";
        protected override object GetEditablePropertyValue(Output item) => item.Description ?? "";
        protected override void SetEditablePropertyValue(Output item, object value) => item.Description = value?.ToString();
        protected override string GetFilterPropertyName() => "Name";
        protected override object GetFilterPropertyValue(Output item) => item.Name ?? "";
        protected override Output CreateNewItem(string name) => throw new NotSupportedException();
    }

    internal class OutputGroupHandler : BaseItemOperationHandler<OutputGroup>
    {
        public override string ItemTypeName => "OutputGroup";
        public override bool SupportsAddDelete => true;

        protected override string GetDisplayName(OutputGroup item) => item.Name ?? "Unnamed";
        protected override string GetEditablePropertyName() => "Name";
        protected override object GetEditablePropertyValue(OutputGroup item) => item.Name ?? "";
        protected override void SetEditablePropertyValue(OutputGroup item, object value) => item.Name = value?.ToString();
        protected override string GetFilterPropertyName() => "Name";
        protected override object GetFilterPropertyValue(OutputGroup item) => item.Name ?? "";
        protected override OutputGroup CreateNewItem(string name)
        {
            return new OutputGroup { Name = name, Description = name };
        }
    }

    internal class RecordingServerHandler : BaseItemOperationHandler<RecordingServer>
    {
        public override string ItemTypeName => "RecordingServer";
        public override bool SupportsAddDelete => false;

        protected override string GetDisplayName(RecordingServer item) => item.Name ?? "Unnamed";
        protected override string GetEditablePropertyName() => "Name";
        protected override object GetEditablePropertyValue(RecordingServer item) => item.Name ?? "";
        protected override void SetEditablePropertyValue(RecordingServer item, object value) => item.Name = value?.ToString();
        protected override string GetFilterPropertyName() => "Name";
        protected override object GetFilterPropertyValue(RecordingServer item) => item.Name ?? "";
        protected override RecordingServer CreateNewItem(string name)
        {
            return new RecordingServer { Name = name };
        }
    }
}
