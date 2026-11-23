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
    public class AddSigVitalesCommand : IRequest<bool>
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

    public class AddSigVitalesCommandHandler
       : IRequestHandler<AddSigVitalesCommand, bool>
    {
        private readonly IGenericRepository<SignosVitales> _repository;

        public AddSigVitalesCommandHandler(
            IGenericRepository<SignosVitales> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            AddSigVitalesCommand request,
            CancellationToken cancellationToken)
        {
            var signos = new SignosVitales
            {
                AtencionId = request.AtencionId,
                PresionSistolica = request.PresionSistolica,
                PresionDiastolica = request.PresionDiastolica,
                FrecuenciaCardiaca = request.FrecuenciaCardiaca,
                FrecuenciaRespiratoria = request.FrecuenciaRespiratoria,
                Temperatura = request.Temperatura,
                SaturacionOxigeno  = request.SaturacionOxigeno,
                Peso = request.Peso,
                Talla = request.Talla,
                Imc = request.Imc,
                FechaRegistro = request.FechaRegistro,


            };

            await _repository.AddAsync(signos);

            return true;
        }
    }
}
