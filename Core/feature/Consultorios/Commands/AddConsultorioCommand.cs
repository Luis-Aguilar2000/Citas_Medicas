using Generics.Interfaces;
using Domain.Models;
using MediatR;

namespace Core.feature.Consultorios.Commands
{
    public class AddConsultorioCommand : IRequest<bool>
    {
        public int IdConsultorio { get; set; }

        public string? Nombre { get; set; }

        public string? NumeroConsultorio { get; set; }

        public string? Piso { get; set; }

        public string? Ubicacion { get; set; }

        public string? Descripcion { get; set; }

        public bool Estado { get; set; }

        public DateTime FechaRegistro { get; set; }
    }


    public class AddConsultorioCommandHandler
        : IRequestHandler<AddConsultorioCommand, bool>
    {
        private readonly IGenericRepository<Consultorio> _repository;


        public AddConsultorioCommandHandler(
            IGenericRepository<Consultorio> repository)
        {
            _repository = repository;
        }


        public async Task<bool> Handle(
            AddConsultorioCommand request,
            CancellationToken cancellationToken)
        {
            var consultorio = new Consultorio
            {
                IdConsultorio = request.IdConsultorio,

                Nombre =
                    request.Nombre ?? string.Empty,

                NumeroConsultorio =
                    request.NumeroConsultorio,

                Piso =
                    request.Piso,

                Ubicacion =
                    request.Ubicacion,

                Descripcion =
                    request.Descripcion,

                Estado =
                    request.Estado,

                FechaRegistro =
                    request.FechaRegistro
            };


            await _repository.AddAsync(
                consultorio
            );


            return true;
        }
    }
}