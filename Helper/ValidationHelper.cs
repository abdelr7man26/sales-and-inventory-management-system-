using Auto_Parts_Store.Models;
using Auto_Parts_Store.Services;
using System;
using System.Threading.Tasks;
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

        public static async Task<bool> ConfirmAdminAccessAsync(AuthService authService)
        {
            if (AuthService.CurrentSession?.CurrentUserRole == "Admin")
                return true;

            string inputPin = Microsoft.VisualBasic.Interaction.InputBox(
                "هذه العملية تتطلب صلاحية المدير.\nبرجاء إدخال الـ PIN Code الخاص بالإدارة:",
                "تأكيد صلاحية",
                "");

            if (await authService.VerifyAdminPinAsync(inputPin))
                return true;

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
        public static bool ValidateQuickPay(string amountText, int personId, out decimal amount, out string error)
        {
            error = "";
            if (!decimal.TryParse(amountText, out amount) || amount <= 0)
            {
                error = "برجاء إدخال مبلغ صحيح أكبر من صفر";
                return false;
            }
            if (personId <= 0)
            {
                error = "برجاء اختيار عميل أو مورد أولاً";
                return false;
            }
            return true;
        }

        public static bool ValidatePurchaseInput(string partName, string partNumber, string priceText, decimal quantity, int categoryIndex, int supplierIndex, out string errorMessage)
        {
            errorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(partName))
            {
                errorMessage = "من فضلك تأكد من كتابة اسم القطعة!";
                return false;
            }

            if (string.IsNullOrWhiteSpace(partNumber))
            {
                errorMessage = "من فضلك تأكد من كتابة رقم القطعة.";
                return false;
            }

            if (!decimal.TryParse(priceText, out decimal buyPrice) || buyPrice <= 0)
            {
                errorMessage = "سعر الشراء غير صحيح! دخل رقم أكبر من صفر.";
                return false;
            }

            if (quantity <= 0)
            {
                errorMessage = "الكمية لازم تكون 1 على الأقل.";
                return false;
            }

            if (categoryIndex <= 0)
            {
                errorMessage = "من فضلك اختار فئة القطعة.";
                return false;
            }

            if (supplierIndex == -1)
            {
                errorMessage = "من فضلك اختر المورد أولاً!";
                return false;
            }

            return true;
        }
    }
}