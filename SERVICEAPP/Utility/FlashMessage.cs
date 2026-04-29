using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SERVICEAPP.Utility
{
    public static class FlashMessage
    {

        public enum FlashType { Success, Error, Info, Warning }
        public static void Success(Controller controller, string message)
           => Set(controller, FlashType.Success.ToString(), message);

        public static void Error(Controller controller, string message)
            => Set(controller, FlashType.Error.ToString(), message);

        public static void Info(Controller controller, string message)
            => Set(controller, FlashType.Info.ToString(), message);

        public static void Warning(Controller controller, string message)
            => Set(controller, FlashType.Warning.ToString(), message);

        private static void Set(Controller controller, string type, string message)
        {
            controller.TempData["Flash.Type"] = type;
            controller.TempData["Flash.Message"] = message;
        }
    }
}
