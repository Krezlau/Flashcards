using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards.Core.Services
{
    public interface ILoadingService
    {
        void Enable();

        void Disable();
    }
}
