using BugTrackerBD.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace BugTrackerBD.Helpers
{
    public class TicketAttachmentHelper
    {
        public static ApplicationDbContext db = new ApplicationDbContext();
        public static bool IsWebFriendly(HttpPostedFileBase file)
        {
            if (file == null) return false;
            try
            {
                var allowedTypes = db.AttachmentTypes.Select(a => a.Type).ToList();
                var fileExt = Path.GetExtension(file.FileName).Split('.')[1];
                return allowedTypes.Contains(fileExt);
            }
            catch
            {
                return false;
            }
        }
    }
}