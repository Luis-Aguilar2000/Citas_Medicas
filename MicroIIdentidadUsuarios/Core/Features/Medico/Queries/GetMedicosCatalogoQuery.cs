
using Core.feature.Medico.DTOs;
using Generics.Interfaces;
using MediatR;

namespace Core.feature.Medico.Queries
{
    public class GetMedicosCatalogoQuery
        : IRequest<List<MedicoCatalogoDto>>
    {
    }

    public class GetMedicosCatalogoQueryHandler
        : IRequestHandler<
            GetMedicosCatalogoQuery,
            List<MedicoCatalogoDto>>
    {
        private readonly IGenericRepository<global::Domain.Models.Medico>
            _medicoRepository;

        private readonly IGenericRepository<global::Domain.Models.Usuario>
            _usuarioRepository;

        public GetMedicosCatalogoQueryHandler(
            IGenericRepository<global::Domain.Models.Medico> medicoRepository,
            IGenericRepository<global::Domain.Models.Usuario> usuarioRepository)
        {
            _medicoRepository = medicoRepository;
            _usuarioRepository = usuarioRepository;
        }

        public async Task<List<MedicoCatalogoDto>> Handle(
            GetMedicosCatalogoQuery request,
            CancellationToken cancellationToken)
        {
            var medicos = await _medicoRepository.GetAllAsync(
                cancellationToken);

            var usuarios = await _usuarioRepository.GetAllAsync(
                cancellationToken);

            var resultado = (
                from medico in medicos
                join usuario in usuarios
                    on medico.IdUsuario equals usuario.IdUsuario
                where medico.Estado && usuario.Estado
                orderby usuario.Apellidos, usuario.Nombres
                select new MedicoCatalogoDto
                {
                    IdMedico = medico.IdMedico,
                    IdUsuario = medico.IdUsuario,
                    Nombres = usuario.Nombres,
                    Apellidos = usuario.Apellidos,
                    NumeroJVPM = medico.NumeroJVPM,
                    Estado = medico.Estado
                }
            ).ToList();

            return resultado;
        }
    }
}
