using Domain.Models;
using Generics.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.feature.Medicamentos.Queries
{
    public class GetMedicamentoByIdQuery: IRequest<Medicamento>
    {
        public int IdMedicamento { get; set; }
    }

    public class GetMedicamentoByIdQueryHandler
       : IRequestHandler<GetMedicamentoByIdQuery, Medicamento>
    {
        private readonly IGenericRepository<Medicamento> _repository;

        public GetMedicamentoByIdQueryHandler(
            IGenericRepository<Medicamento> repository)
        {
            _repository = repository;
        }

        public async Task<Medicamento> Handle(
            GetMedicamentoByIdQuery request,
            CancellationToken cancellationToken)
        {
            return await _repository.GetByIdAsync(request.IdMedicamento);
        }

        
    }
}
