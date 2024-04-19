using Microsoft.Win32;
using System.Management;
using System.Runtime.Versioning;

public static class SerialPortSearcher
{

    [SupportedOSPlatform("windows")]
    public static IEnumerable<ISerialPortInfo> Search()
    {
        using var entity = new ManagementClass("Win32_PnPEntity");
        foreach (var instance in entity.GetInstances().Cast<ManagementObject>())
        {
            var classGuid = instance.GetPropertyValue("ClassGuid");
            // Skip all devices except device class "PORTS"
            if (classGuid?.ToString()?.ToUpper() == "{4D36E978-E325-11CE-BFC1-08002BE10318}")
            {
                yield return new SerialPortInfo(instance);
            }
        }
    }

    [SupportedOSPlatform("windows")]
    private class SerialPortInfo : ISerialPortInfo
    {
        public SerialPortInfo(ManagementObject obj)
        {
            this.Availability = obj.GetPropertyValue("Availability") as int? ?? 0;
            this.Caption = obj.GetPropertyValue("Caption") as string ?? string.Empty;
            this.ClassGuid = obj.GetPropertyValue("ClassGuid") as string ?? string.Empty;
            this.CompatibleID = obj.GetPropertyValue("CompatibleID") as string[] ?? new string[] { };
            this.ConfigManagerErrorCode = obj.GetPropertyValue("ConfigManagerErrorCode") as int? ?? 0;
            this.ConfigManagerUserConfig = obj.GetPropertyValue("ConfigManagerUserConfig") as bool? ?? false;
            this.CreationClassName = obj.GetPropertyValue("CreationClassName") as string ?? string.Empty;
            this.Description = obj.GetPropertyValue("Description") as string ?? string.Empty;
            this.DeviceID = obj.GetPropertyValue("DeviceID") as string ?? string.Empty;
            this.ErrorCleared = obj.GetPropertyValue("ErrorCleared") as bool? ?? false;
            this.ErrorDescription = obj.GetPropertyValue("ErrorDescription") as string ?? string.Empty;
            this.HardwareID = obj.GetPropertyValue("HardwareID") as string[] ?? new string[] { };
            this.InstallDate = obj.GetPropertyValue("InstallDate") as DateTime? ?? DateTime.MinValue;
            this.LastErrorCode = obj.GetPropertyValue("LastErrorCode") as int? ?? 0;
            this.Manufacturer = obj.GetPropertyValue("Manufacturer") as string ?? string.Empty;
            this.Name = obj.GetPropertyValue("Name") as string ?? string.Empty;
            this.PNPClass = obj.GetPropertyValue("PNPClass") as string ?? string.Empty;
            this.PNPDeviceID = obj.GetPropertyValue("PnpDeviceID") as string ?? string.Empty;
            this.PowerManagementCapabilities = obj.GetPropertyValue("PowerManagementCapabilities") as int[] ?? new int[] { };
            this.PowerManagementSupported = obj.GetPropertyValue("PowerManagementSupported") as bool? ?? false;
            this.Present = obj.GetPropertyValue("Present") as bool? ?? false;
            this.Service = obj.GetPropertyValue("Service") as string ?? string.Empty;
            this.Status = obj.GetPropertyValue("Status") as string ?? string.Empty;
            this.StatusInfo = obj.GetPropertyValue("StatusInfo") as int? ?? 0;
            this.SystemCreationClassName = obj.GetPropertyValue("SystemCreationClassName") as string ?? string.Empty;
            this.SystemName = obj.GetPropertyValue("SystemName") as string ?? string.Empty;

            var regPath = "HKEY_LOCAL_MACHINE\\System\\CurrentControlSet\\Enum\\" + PNPDeviceID + "\\Device Parameters";
            this.PortName = Registry.GetValue(regPath, "PortName", "")?.ToString();

            //int i = Caption.IndexOf(" (COM");
            //if (i > 0) // remove COM port from description
            //    Caption = Caption.Substring(0, i);

        }

        public int Availability { get; }
        public string Caption { get; }
        public string ClassGuid { get; }
        public string[] CompatibleID { get; }
        public int ConfigManagerErrorCode { get; }
        public bool ConfigManagerUserConfig { get; }
        public string CreationClassName { get; }
        public string Description { get; }
        public string DeviceID { get; }
        public bool ErrorCleared { get; }
        public string ErrorDescription { get; }
        public string[] HardwareID { get; }
        public DateTime InstallDate { get; }
        public int LastErrorCode { get; }
        public string Manufacturer { get; }
        public string Name { get; }
        public string PNPClass { get; }
        public string PNPDeviceID { get; }
        public int[] PowerManagementCapabilities { get; }
        public bool PowerManagementSupported { get; }
        public bool Present { get; }
        public string Service { get; }
        public string Status { get; }
        public int StatusInfo { get; }
        public string SystemCreationClassName { get; }
        public string SystemName { get; }
        public string? PortName { get; }
    }
}

public interface ISerialPortInfo
{
    int Availability { get; }
    string Caption { get; }
    string ClassGuid { get; }
    string[] CompatibleID { get; }
    int ConfigManagerErrorCode { get; }
    bool ConfigManagerUserConfig { get; }
    string CreationClassName { get; }
    string Description { get; }
    string DeviceID { get; }
    bool ErrorCleared { get; }
    string ErrorDescription { get; }
    string[] HardwareID { get; }
    DateTime InstallDate { get; }
    int LastErrorCode { get; }
    string Manufacturer { get; }
    string Name { get; }
    string PNPClass { get; }
    string PNPDeviceID { get; }
    string? PortName { get; }
    int[] PowerManagementCapabilities { get; }
    bool PowerManagementSupported { get; }
    bool Present { get; }
    string Service { get; }
    string Status { get; }
    int StatusInfo { get; }
    string SystemCreationClassName { get; }
    string SystemName { get; }
}
