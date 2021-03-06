﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeOrganizer.Model.InterfaceRepo;
using TimeOrganizer.Model.Tables;

namespace TimeOrganizer.Model.SqlRepository
{
    public class SqlMailRequestRepository : IMailRequestRepository
    {
        private AppDbContext appDbContext;

        public SqlMailRequestRepository(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public MailRequest Create(MailRequest mailRequest)
        {
            appDbContext.MailRequests.Add(mailRequest);
            appDbContext.SaveChanges();
            return mailRequest;
        }
    }
}
