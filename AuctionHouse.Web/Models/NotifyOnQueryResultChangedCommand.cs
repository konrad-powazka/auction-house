using System.ComponentModel.DataAnnotations;
using AuctionHouse.Core.Messaging;

namespace AuctionHouse.Web.Models
{
    public class NotifyOnQueryResultChangedCommand<TQuery, TResult> where TQuery : IQuery<TResult>
    {
        [Required]
        public TQuery Query { get; set; }

        [Required]
        public string SignalRConnectionId { get; set; }
    }
}