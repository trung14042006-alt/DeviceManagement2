/****************/
/* Mã sinh viên: 202419003 */
/* Họ tên: Nguyễn Thành Trung */
/****************/

using DeviceManagement;
using System;

namespace DeviceManagement2
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("=== CHƯƠNG TRÌNH QUẢN LÝ MƯỢN THIẾT BỊ GIẢNG DẠY (DEVICEMANAGEMENT2) ===\n");

            IBorrower lecturer = new Lecturer("GV01", "Dr. Nguyen Van A");
            IBorrower ta = new TeachingAssistant("TA01", "Tran Thi B");

            TeachingDevice acRemote = new AirConditionerRemote("AC01", "Điều khiển ĐH Panasonic", 2022, 500000m, DeviceStatus.Available, 5);
            TeachingDevice mic = new Microphone("MIC01", "Micro không dây Sony", 2023, 1500000m, DeviceStatus.Available, 3);
            TeachingDevice projRemote = new ProjectorRemote("PJ01", "Điều khiển Máy chiếu Sony", 2021, 800000m, DeviceStatus.Available, 4);
            TeachingDevice brokenAc = new AirConditionerRemote("AC99", "Điều khiển hỏng", 2020, 500000m, DeviceStatus.UnderMaintenance, 5);

            Console.WriteLine("--- 1. CHẠY LUỒNG THÀNH CÔNG ---");
            try
            {
                var slip = new BorrowingSlip("SLIP01", lecturer, "Phòng 301-B1", DateTime.Now);

                slip.AddDevice(projRemote, 2, 1, 2);
                slip.AddDevice(mic, 1, 3, 4);
                Console.WriteLine("Thêm thành công 2 thiết bị vào phiếu mượn SLIP01.");

                decimal fee1 = slip.ReturnDevice("PJ01", DateTime.Now);
                Console.WriteLine($"Đã trả 'PJ01' đúng hạn. Phí phạt: {fee1:N0} VNĐ");

                decimal fee2 = slip.ReturnDevice("MIC01", DateTime.Now.AddDays(3));
                Console.WriteLine($"Đã trả 'MIC01' muộn hạn. Phí phạt: {fee2:N0} VNĐ");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi không mong muốn: {ex.Message}");
            }

            Console.WriteLine("\n--- 2. KIỂM THỬ 7 TÌNH HUỐNG PHÁT SINH NGOẠI LỆ ---");

            RunTest("1. Mượn điều hòa đang bảo trì", () => {
                var slip = new BorrowingSlip("ERR01", lecturer, "Phòng 101", DateTime.Now);
                slip.AddDevice(brokenAc, 1, 1, 2);
            });

            RunTest("2. Trợ giảng mượn quá giới hạn (thiết bị thứ 3)", () => {
                var slip = new BorrowingSlip("ERR02", ta, "Phòng 102", DateTime.Now);
                slip.AddDevice(acRemote, 1, 1, 2);
                slip.AddDevice(projRemote, 1, 1, 2);

                TeachingDevice extraMic = new Microphone("MIC02", "Micro phụ", 2023, 1000000m, DeviceStatus.Available, 3);
                slip.AddDevice(extraMic, 1, 1, 2);
            });

            RunTest("3. Thêm trùng một thiết bị vào phiếu", () => {
                var slip = new BorrowingSlip("ERR03", lecturer, "Phòng 103", DateTime.Now);
                slip.AddDevice(mic, 1, 1, 2);
                slip.AddDevice(mic, 1, 3, 4);
            });

            RunTest("4. Mượn thiết bị vượt quá số tiết cho phép", () => {
                var slip = new BorrowingSlip("ERR04", lecturer, "Phòng 104", DateTime.Now);
                slip.AddDevice(mic, 1, 1, 5);
            });

            RunTest("5. Trả thiết bị không nằm trong phiếu", () => {
                var slip = new BorrowingSlip("ERR05", lecturer, "Phòng 105", DateTime.Now);
                slip.ReturnDevice("AC99", DateTime.Now);
            });

            RunTest("6. Trả thiết bị hai lần", () => {
                var slip = new BorrowingSlip("ERR06", lecturer, "Phòng 106", DateTime.Now);
                slip.AddDevice(acRemote, 1, 1, 2);
                slip.ReturnDevice("AC01", DateTime.Now);
                slip.ReturnDevice("AC01", DateTime.Now);
            });

            RunTest("7. Ngày trả trước ngày mượn", () => {
                var item = new BorrowItem(acRemote, DateTime.Now, DateTime.Now.AddDays(2), 1, 2);
                item.Return(DateTime.Now.AddDays(-1));
            });

            Console.WriteLine("\n=== HOÀN TẤT KIỂM THỬ TOÀN BỘ HỆ THỐNG ===");
        }

        private static void RunTest(string testName, Action testAction)
        {
            Console.Write($"[TEST] {testName} ... ");
            try
            {
                testAction();
                Console.WriteLine("THẤT BẠI (Không bắt được ngoại lệ dự kiến).");
            }
            catch (BorrowingException ex)
            {
                Console.WriteLine($"THÀNH CÔNG (Bắt được BorrowingException): {ex.Message}");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"THÀNH CÔNG (Bắt được ArgumentException): {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"THÀNH CÔNG (Bắt được Exception): {ex.Message}");
            }
        }
    }
}