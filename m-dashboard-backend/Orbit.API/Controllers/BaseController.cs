using Microsoft.AspNetCore.Mvc;
using Orbit.Models.Repositories;

namespace Orbit.API.Controllers
{
    public abstract class BaseController:ControllerBase
    {
        protected IUnitOfWork _unitOfWork;
    }
}
