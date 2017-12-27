using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _07_CustomTaskLikeTypes
{
    public class Sample
    {
        public async TaskLike FooAsync()
        {
            await Task.Yield();
            await default(TaskLike);
        }
    }
}
