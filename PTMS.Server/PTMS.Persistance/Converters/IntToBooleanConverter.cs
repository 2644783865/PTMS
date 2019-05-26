using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace PTMS.Persistance.Converters
{
    internal class IntToBooleanConverter : ValueConverter<bool, int>
    {
        public IntToBooleanConverter()
            : base(boolVar => boolVar ? 1 : 0, intVar => intVar == 1)
        {
        }
    }
}
