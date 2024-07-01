using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Domain.Entities;

namespace Washyn.UNAJ.Lot.Controllers
{

    [RemoteService(isEnabled: true)]
    public class ErrorSampleController : AbpControllerBase
    {
        public ErrorSampleController()
        {
        }

        [HttpGet]
        public void Error500()
        {
            throw new Exception("Internal error.");
        }

        [HttpGet]
        public void Error401()
        {
            throw new UnauthorizedAccessException("Un autorized ex");
        }

        [HttpGet]
        public void Error403()
        {
            throw new UserFriendlyException("erro from back");
        }

        [HttpGet]
        public void Error40XXX(ModelSample modelSample)
        {
        }

        [HttpGet]
        public void Error404()
        {
            throw new EntityNotFoundException("No se encontro la entidad.");
        }
    }

    public class ModelSample : IValidatableObject
    {
        public string? TestValue { get; set; }
        public string? SecondValue { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(TestValue))
            {
                yield return new ValidationResult("El campo es requerido 1.", new[] { nameof(TestValue) });
            }
            if (string.IsNullOrEmpty(SecondValue))
            {
                yield return new ValidationResult("El campo es requerido 2.", new[] { nameof(SecondValue) });
            }
        }
    }
}