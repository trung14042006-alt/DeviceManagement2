/****************/
/* Mã sinh viên: 202419003 */
/* Họ tên: Nguyễn Thành Trung */
/****************/

using System;

namespace DeviceManagement2
{
    public class BorrowingException : Exception
    {
        public BorrowingException(string message) : base(message) { }
        public BorrowingException(string message, Exception innerException) : base(message, innerException) { }
    }

    public class DeviceUnavailableException : BorrowingException
    {
        public string DeviceId { get; }
        public DeviceStatus CurrentStatus { get; }

        public DeviceUnavailableException(string deviceId, DeviceStatus currentStatus)
            : base($"Thiết bị {deviceId} không khả dụng (Trạng thái hiện tại: {currentStatus}).")
        {
            DeviceId = deviceId;
            CurrentStatus = currentStatus;
        }
    }

    public class BorrowLimitExceededException : BorrowingException
    {
        public string BorrowerId { get; }
        public int MaximumDevices { get; }

        public BorrowLimitExceededException(string borrowerId, int maxDevices)
            : base($"Người mượn {borrowerId} đã đạt số thiết bị tối đa cho phép ({maxDevices}).")
        {
            BorrowerId = borrowerId;
            MaximumDevices = maxDevices;
        }
    }

    public class InvalidBorrowPeriodException : BorrowingException
    {
        public string DeviceId { get; }
        public int RequestedDays { get; }
        public int MaxAllowedDays { get; }

        public InvalidBorrowPeriodException(string deviceId, int requestedDays, int maxAllowedDays)
            : base($"Thời hạn mượn thiết bị {deviceId} ({requestedDays} ngày) không hợp lệ hoặc vượt quá giới hạn ({maxAllowedDays} ngày).")
        {
            DeviceId = deviceId;
            RequestedDays = requestedDays;
            MaxAllowedDays = maxAllowedDays;
        }
    }

    public class DuplicateDeviceException : BorrowingException
    {
        public string DeviceId { get; }

        public DuplicateDeviceException(string deviceId)
            : base($"Thiết bị {deviceId} đã tồn tại trong phiếu mượn.")
        {
            DeviceId = deviceId;
        }
    }

    public class DeviceNotInSlipException : BorrowingException
    {
        public string DeviceId { get; }

        public DeviceNotInSlipException(string deviceId)
            : base($"Thiết bị {deviceId} không thuộc phiếu mượn này.")
        {
            DeviceId = deviceId;
        }
    }

    public class DeviceAlreadyReturnedException : BorrowingException
    {
        public string DeviceId { get; }

        public DeviceAlreadyReturnedException(string deviceId)
            : base($"Thiết bị {deviceId} trong phiếu đã được trả trước đó.")
        {
            DeviceId = deviceId;
        }
    }
}