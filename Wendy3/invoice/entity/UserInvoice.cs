using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wendy.Invoice.Entity
{
    /// <summary>
    /// User invoice
    /// </summary>
    public class UserInvoice : Invoice
    {
        /// <summary>
        /// Owner of invoice
        /// </summary>
        public string InvoiceOwner { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="readOutDate"></param>
        /// <param name="estimatedReadout"></param>
        /// <param name="realReadOut"></param>
        public UserInvoice(string user, DateTime readOutDate, ulong estimatedReadout, ulong realReadOut)
            : base(readOutDate, estimatedReadout, realReadOut)
        {
            InvoiceOwner = user;
        }
    }
}
