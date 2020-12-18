using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Linq;

namespace API.Contracts.Response
{
    public record ErrorResponseDTO
    {
        public string ErrorMessage { get; set; }
        public ModelStateDictionary ModelErrors { get; set; }

        public ErrorResponseDTO() { }
        public ErrorResponseDTO(string message)
        {
            this.ErrorMessage = message;
        }
        public ErrorResponseDTO(string message, ModelStateDictionary modelState)
        {
            this.ErrorMessage = message;
            ModelErrors = modelState;
        }
    }
}
