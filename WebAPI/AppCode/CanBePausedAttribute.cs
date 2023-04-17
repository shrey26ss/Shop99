using Hangfire.Common;
using Hangfire.Server;
using Hangfire.Storage;
using System;

namespace WebAPI.AppCode
{
    public class CanBePausedAttribute : JobFilterAttribute, IServerFilter
    {
        public void OnPerformed(PerformedContext filterContext)
        {
            filterContext.Connection.GetAllItemsFromSet("start-job");
            //throw new NotImplementedException();
        }

        public void OnPerforming(PerformingContext filterContext)
        {
            var values = filterContext.Connection.GetAllItemsFromSet("paused-jobs");
            if (values.Contains(filterContext.Job.Type.Name))
            {
                filterContext.Canceled = true;
            }
        }
    }


    public static class PauseJobStorageExtensions
    {
        public static void Pause(this IStorageConnection connection, Type type)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (type == null) throw new ArgumentNullException("type");

            using (var transaction = connection.CreateWriteTransaction())
            {
                transaction.AddToSet("paused-jobs", type.Name);
                transaction.Commit();
            }
        }

        public static void Resume(this IStorageConnection connection, Type type)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (type == null) throw new ArgumentNullException("type");

            using (var transaction = connection.CreateWriteTransaction())
            {
                transaction.RemoveFromSet("paused-jobs", type.Name);
                transaction.Commit();
            }
        }
    }
}
