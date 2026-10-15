/****************/
/* Mã sinh viên: 202419003 */
/* Họ tên: Nguyễn Thành Trung */
/****************/

namespace DeviceManagement2
{
    public class Lecturer : IBorrower
    {
        public string BorrowerId { get; }
        public string FullName { get; }

        public Lecturer(string borrowerId, string fullName)
        {
            BorrowerId = borrowerId;
            FullName = fullName;
        }

        public int GetMaximumDevices() => 3;
    }

    public class TeachingAssistant : IBorrower
    {
        public string BorrowerId { get; }
        public string FullName { get; }

        public TeachingAssistant(string borrowerId, string fullName)
        {
            BorrowerId = borrowerId;
            FullName = fullName;
        }

        public int GetMaximumDevices() => 2;
    }
}