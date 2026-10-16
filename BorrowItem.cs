/****************/
/* Mã sinh viên: 202419003 */
/* Họ tên: Nguyễn Thành Trung */
/****************/

using DeviceManagement2;
using System;

namespace DeviceManagement
{
    public class BorrowItem
    {
        public TeachingDevice Device { get; }
        public DateTime BorrowedDate { get; }
        public DateTime DueDate { get; }
        public int FromPeriod { get; set; }
        public int ToPeriod { get; set; }
        public DateTime? ReturnedDate { get; private set; }
        public bool IsReturned => ReturnedDate.HasValue;

        public BorrowItem(TeachingDevice device, DateTime borrowedDate, DateTime dueDate, int fromPeriod, int toPeriod)
        {
            Device = device ?? throw new ArgumentNullException(nameof(device));
            if (dueDate < borrowedDate)
                throw new ArgumentException("Ngày đến hạn (DueDate) phải sau hoặc bằng ngày mượn.");

            BorrowedDate = borrowedDate;
            DueDate = dueDate;
            FromPeriod = fromPeriod;
            ToPeriod = toPeriod;
            ReturnedDate = null;
        }

        public void Return(DateTime returnedDate)
        {
            if (IsReturned)
                throw new DeviceAlreadyReturnedException(Device.DeviceId);

            if (returnedDate < BorrowedDate)
                throw new ArgumentException("Ngày trả không được trước ngày mượn.");

            ReturnedDate = returnedDate;
            Device.MarkAsAvailable();
        }
    }
}