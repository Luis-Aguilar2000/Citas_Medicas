using Domain.Models;
using Generics.Interfaces;
using MediatR;

namespace Core.feature.CatalogoEstudios.Commands
{
    public class UpdateCtalogoEstudCommand : IRequest<bool>
    {
        public int IdCatalogoEstudio { get; set; }
        public string Codigo { get; set; }
        public string NombreEstudio { get; set; }
        public string Categoria { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public bool Activo { get; set; }
        public List<SolicitudEstudioDetalle> SolicitudEstudioDetalles { get; set; }
    }

    public class UpdateCtalogoEstudCommandHandler
        : IRequestHandler<UpdateCtalogoEstudCommand, bool>
    {
        private readonly IGenericRepository<CatalogoEstudio> _repository;

        public UpdateCtalogoEstudCommandHandler(
            IGenericRepository<CatalogoEstudio> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            UpdateCtalogoEstudCommand request,
            CancellationToken cancellationToken)
        {
            var catalogo = await _repository.GetByIdAsync(
                request.IdCatalogoEstudio,
                cancellationToken
            );

            if (catalogo == null)
            {
                return false;
            }

            catalogo.Codigo = request.Codigo;
            catalogo.NombreEstudio = request.NombreEstudio;
            catalogo.Categoria = request.Categoria;
            catalogo.Descripcion = request.Descripcion;
            catalogo.Precio = request.Precio;
            catalogo.Activo = request.Activo;
            catalogo.SolicitudEstudioDetalles = request.SolicitudEstudioDetalles;

            await _repository.UpdateAsync(
                catalogo,
                cancellationToken
            );

            return true;
        }
    }
}