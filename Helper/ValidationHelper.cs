using Auto_Parts_Store.Models;
using Auto_Parts_Store.Services;
using System;
using System.Windows.Forms;
using System.Linq;

namespace Auto_Parts_Store.Helpers
{
    public static class ValidationHelper
    {
        public static void ValidatePart(AutoPart part)
        {
            if (part == null)
                throw new Exception("Part is null");

            if (string.IsNullOrWhiteSpace(part.PartName))
                throw new Exception("اسم القطعة مطلوب");

            if (string.IsNullOrWhiteSpace(part.PartNumber))
                throw new Exception("رقم القطعة مطلوب");

            if (part.PurchasePrice < 0)
                throw new Exception("سعر الشراء غير صحيح");

            if (part.SellingPrice < 0)
                throw new Exception("سعر البيع غير صحيح");

            if (part.CategoryID <= 0)
                throw new Exception("الفئة غير صحيحة");
        }

        public static bool ConfirmAdminAccess(AuthService authService)
        {
            if (AuthService.CurrentSession.CurrentUserRole == "Admin")
            {
                return true;
            }

            string inputPin = Microsoft.VisualBasic.Interaction.InputBox(
                "هذه العملية تتطلب صلاحية المدير.\nبرجاء إدخال الـ PIN Code الخاص بالإدارة:",
                "تأكيد صلاحية",
                "");

            if (authService.VerifyAdminPin(inputPin))
            {
                return true;
            }

            MessageBox.Show("الـ PIN الذي أدخلته غير صحيح!", "خطأ أمان", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            return false;
        }

        public static void ValidatePerson(Person person)
        {
            if (person == null) throw new Exception("البيانات فارغة");

            if (string.IsNullOrWhiteSpace(person.PersonName))
                throw new Exception("اسم الشخص مطلوب");

            if (!string.IsNullOrWhiteSpace(person.Phone) && !person.Phone.All(char.IsDigit))
                throw new Exception("رقم التليفون يجب أن يحتوي على أرقام فقط");
        }
    }
}