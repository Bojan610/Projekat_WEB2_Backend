using Projekat_Web2.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projekat_Web2.Interfaces
{
    public interface IDelivererService
    {
        RetStringDto VerifyCheck(string email);

    }
}
