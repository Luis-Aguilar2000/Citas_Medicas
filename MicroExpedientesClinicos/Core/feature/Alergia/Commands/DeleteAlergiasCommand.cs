using Domain.Models;
using Generics.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.feature.Alergia.Commands
{
    public class DeleteAlergiasCommand: IRequest<bool>
    {
        public int AlergiaId { get; set; }
    }

    public class DeleteAlergiasCommandHandler
        : IRequestHandler<DeleteAlergiasCommand, bool>
    {
        private readonly IGenericRepository<Alergias> _repository;

        public DeleteAlergiasCommandHandler(
            IGenericRepository<Alergias> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            DeleteAlergiasCommand request,
            CancellationToken cancellationToken)
        {
            var alergias = await _repository.GetByIdAsync(request.AlergiaId);

            if (alergias == null)
            {
                return false;
            }

            await _repository.DeleteAsync(alergias);

            return true;
        }
    }
}
