using System;
using System.Collections.Generic;

namespace Ecc.Contract.Models.EmailRedirects
{
    public class EmailLinkCatchDetailedModel
    {
        public string Id { get; set; }

        public string Url { get; set; }

        public List<DateTime> RedirectsOn { get; set; }
    }
}