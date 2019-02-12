using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fogy.Core.Application.Services.Dto
{
    public interface ISortedResultItem
    {
        bool Ascending { get; set; }

        string PropertyName { get; set; }
    }
}
