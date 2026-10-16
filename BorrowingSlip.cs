/****************/
/* Mã sinh viên: 202419003 */
/* Họ tên: Nguyễn Thành Trung */
/****************/

using DeviceManagement2;
using System;
using System.Collections.Generic;

namespace DeviceManagement
{
    public class BorrowingSlip
    {
        public string SlipId { get; }
        public IBorrower Borrower { get; }
        public string TeachingRoom { get; }
        public DateTime BorrowedDate { get; }

        private readonly List<BorrowItem> _items = new List<BorrowItem>();
        public IReadOnlyList<BorrowItem> Items => _items.AsReadOnly();

        public BorrowingSlip(string slipId, IBorrower borrower, string teachingRoom, DateTime borrowedDate)
        {
            SlipId = slipId;
            Borrower = borrower ?? throw new ArgumentNullException(nameof(borrower));
            TeachingRoom = teachingRoom ?? throw new ArgumentNullException(nameof(teachingRoom));
            BorrowedDate = borrowedDate;
        }

        public void AddDevice(TeachingDevice device, int borrowDays, int fromPeriod, int toPeriod)
        {
            if (device == null)
                throw new ArgumentNullException(nameof(device));

            if (borrowDays <= 0)
                throw new InvalidBorrowPeriodException(device.DeviceId, borrowDays, 30);

            // Kiểm tra trùng thiết bị trong phiếu
            foreach (var item in _items)
            {
                if (item.Device.DeviceId.Equals(device.DeviceId, StringComparison.OrdinalIgnoreCase))
                    throw new DuplicateDeviceException(device.DeviceId);
            }

            // Kiểm tra giới hạn số lượng thiết bị của người mượn
            if (_items.Count >= Borrower.GetMaximumDevices())
                throw new BorrowLimitExceededException(Borrower.BorrowerId, Borrower.GetMaximumDevices());

            // Kiểm tra thiết bị có sẵn sàng không
            if (device.Status != DeviceStatus.Available)
                throw new DeviceUnavailableException(device.DeviceId, device.Status);

            // Kiểm tra giới hạn thời gian/tiết mượn
            int totalPeriods = (toPeriod - fromPeriod) + 1;
            if (totalPeriods > device.MaxBorrowPeriods)
                throw new InvalidBorrowPeriodException(device.DeviceId, totalPeriods, device.MaxBorrowPeriods);

            // Thỏa mãn toàn bộ điều kiện -> Thực hiện thay đổi trạng thái và thêm vào phiếu (Đảm bảo tính nhất quán)
            var dueDate = BorrowedDate.AddDays(borrowDays);
            device.MarkAsBorrowed();
            var newItem = new BorrowItem(device, BorrowedDate, dueDate, fromPeriod, toPeriod);
            _items.Add(newItem);
        }

        public decimal ReturnDevice(string deviceId, DateTime returnedDate)
        {
            BorrowItem? targetItem = null;
            foreach (var item in _items)
            {
                if (item.Device.DeviceId.Equals(deviceId, StringComparison.OrdinalIgnoreCase))
                {
                    targetItem = item;
                    break;
                }
            }

            if (targetItem == null)
                throw new DeviceNotInSlipException(deviceId);

            if (targetItem.IsReturned)
                throw new DeviceAlreadyReturnedException(deviceId);

            targetItem.Return(returnedDate);

            // Tính phí phạt trễ hạn (nếu có, ví dụ 50,000 VNĐ / ngày trễ)
            decimal lateFee = 0;
            if (returnedDate > targetItem.DueDate)
            {
                int lateDays = (int)(returnedDate - targetItem.DueDate).TotalDays;
                lateFee = lateDays * 50000m;
            }

            return lateFee;
        }

        public decimal CalculateTotalLateFee()
        {
            decimal total = 0;
            foreach (var item in _items)
            {
                if (item.IsReturned && item.ReturnedDate.HasValue && item.ReturnedDate.Value > item.DueDate)
                {
                    int lateDays = (int)(item.ReturnedDate.Value - item.DueDate).TotalDays;
                    total += lateDays * 50000m;
                }
            }
            return total;
        }
    }
}