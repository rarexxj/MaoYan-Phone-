using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BntWeb.OrderProcess.ViewModels
{
    public class StateOrderCount
    {
        public int TotalCount { set; get; } = 0;
        public int PendingPaymentCount { set; get; }=0;
        public int WaitingForReceivingCount { set; get; } = 0;
        public int WaitingEvaluateCount { set; get; } = 0;
        public int RefundCount { set; get; } = 0;
    }
}
