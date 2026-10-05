using System;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Core.Facturacion.Exceptions
{
    public class DomainException : Exception
    {
        public Errores Error { get; }

        public DomainException(
            Errores error,
            string message)
            : base(message)
        {
            Error = error;
        }
    }
}