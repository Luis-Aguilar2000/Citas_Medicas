using Domain.Models;
using Generics.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.feature.SignoVitales.Commands
{

    public class UpdateSigVitalesCommand : IRequest<bool>
    {
        public int SignosVitalesId { get; set; }
        public int AtencionId { get; set; }
        public int? PresionSistolica { get; set; }
        public int? PresionDiastolica { get; set; }
        public int? FrecuenciaCardiaca { get; set; }
        public int? FrecuenciaRespiratoria { get; set; }
        public decimal? Temperatura { get; set; }
        public decimal? SaturacionOxigeno { get; set; }
        public decimal? Peso { get; set; }
        public decimal? Talla { get; set; }
        public decimal? Imc { get; set; }
        public DateTime FechaRegistro { get; set; }
    }

    public class UpdateSigVitalesCommandHandler
       : IRequestHandler<UpdateSigVitalesCommand, bool>
    {
        private readonly IGenericRepository<SignosVitales> _repository;

        public UpdateSigVitalesCommandHandler(
            IGenericRepository<SignosVitales> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            UpdateSigVitalesCommand request,
            CancellationToken cancellationToken)
        {
            var signos = await _repository.GetByIdAsync(request.SignosVitalesId);

            if (signos == null)
            {
                return false;
            }

            signos.AtencionId = request.AtencionId;
            signos.PresionSistolica = request.PresionSistolica;
            signos.PresionDiastolica = request.PresionDiastolica;
            signos.FrecuenciaCardiaca = request.FrecuenciaCardiaca;
            signos.FrecuenciaCardiaca = request.FrecuenciaRespiratoria;
            signos.Temperatura = request.Temperatura;
            signos.SaturacionOxigeno = request.SaturacionOxigeno;
            signos.Peso = request.Peso;
            signos.Talla = request.Talla;
            signos. Imc = request.Imc;
            signos.FechaRegistro = request.FechaRegistro;


            await _repository.UpdateAsync(signos);

            return true;
        }
    }
}
