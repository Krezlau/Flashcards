using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards.Core.Services
{
    public interface IDialogService
    {
        void ShowMessageDialog(string title, string message);
        void ShowSnackbarMessage(string title, string message);
    }
}
