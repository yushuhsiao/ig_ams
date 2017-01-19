
namespace System.ComponentModel.DataAnnotations
{
    public class NotZeroAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var zero = Convert.ChangeType(0, value.GetType());

            return !zero.Equals(value);
        }
    }
}
