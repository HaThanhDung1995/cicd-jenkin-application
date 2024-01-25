using Castle.Core.Logging;
using DemoCICD.Contract.Abstractions.Messages;
using DemoCICD.Contract.Services.Products;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoCICD.Application.UseCases.V1.Events
{
    public class SendEmailWhenProductChangedEventHandler
    : IDomainEventHandler<DomainEvent.ProductCreated>,
    IDomainEventHandler<DomainEvent.ProductDeleted>
    {
      
        private readonly ILogger<SendEmailWhenProductChangedEventHandler> _logger;
        public SendEmailWhenProductChangedEventHandler(ILogger<SendEmailWhenProductChangedEventHandler> logger)
        {
            _logger = logger;
        }

        public async Task Handle(DomainEvent.ProductCreated notification, CancellationToken cancellationToken)
        {
            SendEmail(notification.Id);
            await Task.Delay(1000);
        }

        public async Task Handle(DomainEvent.ProductDeleted notification, CancellationToken cancellationToken)
        {
            SendEmail(notification.Id);
            await Task.Delay(1000);
        }

        private void SendEmail(Guid Id)
        {
            _logger.LogInformation($"Email is sented with productId {Id.ToString()}");
        }
    }
}
