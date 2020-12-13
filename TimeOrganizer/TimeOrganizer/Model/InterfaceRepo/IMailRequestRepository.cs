using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeOrganizer.Model.Tables;

namespace TimeOrganizer.Model.InterfaceRepo
{
    public interface IMailRequestRepository
    {
        public MailRequest Create(MailRequest mailRequest);
    }
}
