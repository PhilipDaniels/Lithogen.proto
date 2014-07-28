using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lithogen.Core
{
    public static class TaskFactory
    {
        //public static T MakeSubTask<T>(Task parent)
        //    where T : Task
        //{
        //    return (T)MakeSubTask(typeof(T), parent);
        //}

        //public static Task MakeSubTask(Type taskType, Task parent)
        //{
        //    Task task = (Task)Activator.CreateInstance(taskType);
        //    task.BuildEngine = parent.BuildEngine;
        //    task.HostObject = parent.HostObject;
        //    return task;
        //}

        //public static T MakeLithogenTask<T>(LithogenTask parent)
        //    where T : LithogenTask
        //{
        //    return (T)MakeLithogenTask(typeof(T), parent);
        //}

        //public static LithogenTask MakeLithogenTask(Type taskType, LithogenTask parent)
        //{
        //    LithogenTask task = (LithogenTask)Activator.CreateInstance(taskType);
        //    task.BuildEngine = parent.BuildEngine;
        //    task.HostObject = parent.HostObject;
        //    task.Logger = parent.Logger;
        //    return task;
        //}
    }
}
