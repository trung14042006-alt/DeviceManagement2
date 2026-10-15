/****************/
/* Mã sinh viên: 202419003 */
/* Họ tên: Nguyễn Thành Trung */
/****************/

using System;

namespace DeviceManagement2
{
    public abstract class TeachingDevice
    {
        public string DeviceId { get; }
        public string DeviceName { get; set; }
        public int YearOfManufacture { get; set; }
        public decimal PurchasePrice { get; set; }
        public DeviceStatus Status { get; set; }
        public int MaxBorrowPeriods { get; set; }

        protected TeachingDevice(string deviceId, string deviceName, int yearOfManufacture, decimal purchasePrice, DeviceStatus status, int maxBorrowPeriods)
        {
            if (string.IsNullOrWhiteSpace(deviceId))
                throw new ArgumentException("Mã thiết bị không được để trống.", nameof(deviceId));

            if (purchasePrice <= 0)
                throw new ArgumentException("Giá mua phải lớn hơn 0.", nameof(purchasePrice));

            int currentYear = DateTime.Now.Year;
            if (yearOfManufacture > currentYear)
                throw new ArgumentException($"Năm sản xuất không được lớn hơn năm hiện tại ({currentYear}).", nameof(yearOfManufacture));

            DeviceId = deviceId;
            DeviceName = deviceName;
            YearOfManufacture = yearOfManufacture;
            PurchasePrice = purchasePrice;
            Status = status;
            MaxBorrowPeriods = maxBorrowPeriods;
        }

        public void MarkAsBorrowed()
        {
            if (Status != DeviceStatus.Available)
                throw new DeviceUnavailableException(DeviceId, Status);
            Status = DeviceStatus.Borrowed;
        }

        public void MarkAsAvailable()
        {
            Status = DeviceStatus.Available;
        }

        public abstract decimal CalculateAnnualMaintenanceCost();

        public override string ToString()
        {
            return $"[{DeviceId}] {DeviceName} | Trạng thái: {Status} | Tiết tối đa: {MaxBorrowPeriods}";
        }
    }
}