using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public sealed class Inspector
    {
        public List<string> InvokedMembers { get; } = new List<string>();

        public void Clear() => InvokedMembers.Clear();

        public void Print()
        {
            foreach (var m in InvokedMembers)
            {
                Console.WriteLine(m);
            }
        }

        public void RecordInvocation([CallerMemberName]string member = null)
        {
            InvokedMembers.Add(member);
        }

        public void Record(string message)
        {
            InvokedMembers.Add(message);
        }
    }
}
