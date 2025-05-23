﻿using KooliProjekt.Data;
using KooliProjekt.Search;

namespace KooliProjekt.Services
{
    public interface ILogEntryService
    {
        Task<PagedResult<LogEntry>> List(int page, int v, LogEntriesSearch searchModel);
    }
}