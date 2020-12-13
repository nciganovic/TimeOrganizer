using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeOrganizer.Model.Tables;

namespace TimeOrganizer.Services
{
    public interface IMailService
    {
        System.Threading.Tasks.Task SendEmailAsync(MailRequest mailRequest);
    }
}
