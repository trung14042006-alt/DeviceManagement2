/****************/
/* Mã sinh viên: 202419003 */
/* Họ tên: Nguyễn Thành Trung */
/****************/

using DeviceManagement2;

namespace DeviceManagement
{
    public class AirConditionerRemote : TeachingDevice
    {
        public AirConditionerRemote(string deviceId, string deviceName, int yearOfManufacture, decimal purchasePrice, DeviceStatus status, int maxBorrowPeriods)
            : base(deviceId, deviceName, yearOfManufacture, purchasePrice, status, maxBorrowPeriods) { }

        public override decimal CalculateAnnualMaintenanceCost() => PurchasePrice * 0.03m;
    }

    public class Microphone : TeachingDevice
    {
        public Microphone(string deviceId, string deviceName, int yearOfManufacture, decimal purchasePrice, DeviceStatus status, int maxBorrowPeriods)
            : base(deviceId, deviceName, yearOfManufacture, purchasePrice, status, maxBorrowPeriods) { }

        public override decimal CalculateAnnualMaintenanceCost() => PurchasePrice * 0.05m;
    }

    public class ProjectorRemote : TeachingDevice
    {
        public ProjectorRemote(string deviceId, string deviceName, int yearOfManufacture, decimal purchasePrice, DeviceStatus status, int maxBorrowPeriods)
            : base(deviceId, deviceName, yearOfManufacture, purchasePrice, status, maxBorrowPeriods) { }

        public override decimal CalculateAnnualMaintenanceCost() => PurchasePrice * 0.04m;
    }
}