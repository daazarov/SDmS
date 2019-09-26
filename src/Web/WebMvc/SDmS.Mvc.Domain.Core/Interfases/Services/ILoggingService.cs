using SDmS.Domain.Core.Models;
using System;
using System.Collections.Generic;

namespace SDmS.Domain.Core.Interfases.Services
{
    public interface ILoggingService
    {
        void Error(string message);
        void Error(Exception ex);
        void Initialise();
        void Recycle();
        void ClearLogFiles();
    }
}
