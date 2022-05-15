using Flashcards.Core.Models;
using Microsoft.Toolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards.Core.Messages
{
    public class NewDeckMessage : ValueChangedMessage<string>
    {
        public NewDeckMessage(string value) : base(value)
        {

        }
    }
}
