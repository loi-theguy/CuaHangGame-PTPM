using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLDAL
{
    public class BLDAL_Quyen : DataHelper<Quyen>
    {
        public override List<Quyen> GetData()
        {
            return context.Quyens.Select(q=>q).ToList();
        }
    }
}
