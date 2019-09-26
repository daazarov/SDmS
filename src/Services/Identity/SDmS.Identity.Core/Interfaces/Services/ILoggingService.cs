using SDmS.Common.Models;
using System;

namespace SDmS.Identity.Core.Interfaces.Services
{
    public interface ILoggingService
    {
        void Error(Exception exception, ActionInfo action);
    }
}
