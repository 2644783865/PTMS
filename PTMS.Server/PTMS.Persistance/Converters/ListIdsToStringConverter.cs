using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PTMS.Persistance.Converters
{
    internal class ListIdsToStringConverter : ValueConverter<List<int>, string>
    {
        public ListIdsToStringConverter()
            : base(list => list != null ? string.Join(',', list) : null, 
                  str => str != null ? str.Split(',', StringSplitOptions.None).Select(int.Parse).ToList() : null)
        {
        }
    }
}
