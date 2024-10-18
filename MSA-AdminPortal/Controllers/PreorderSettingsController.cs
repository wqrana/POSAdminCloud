
using AdminPortalModels.ViewModels;
using AdminPortalModels.Models;
using Repository;
using Repository.edmx;
using Repository.Helpers;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MSA_AdminPortal.Controllers
{
    public class PreorderSettingsController : BaseAuthorizedController
    {


        private PreorderSettingsHelper preorderSettingsHelper;

        public PreorderSettingsController()
        {
            preorderSettingsHelper = new PreorderSettingsHelper();
        }

        public ActionResult Index()
        {

            //Data to populate the Dropdown list for Pickup type options
            ViewBag.PickupOptions = new SelectList(
                             new List<SelectListItem>
                                 {
                                    new SelectListItem { Text = "Manual Mode", Value = "0"},
                                    new SelectListItem { Text = "Automatic Mode", Value = "1"},
                                    new SelectListItem { Text = "POS Pickup Mode", Value = "2"},

                                }, "Value", "Text");

            //Get ViewModel from Helper Class

            PreorderSettingModel viewModel = preorderSettingsHelper.SelectData(ClientInfoData.GetClientID());

            return View(viewModel);
        }

        public JsonResult SaveSettings(PreorderSettingModel data)
        {
            int resultSts = 0;

            if (ModelState.IsValid)
            {


                try
                {

                    resultSts = preorderSettingsHelper.UpdateData(data);


                }
                catch (Exception ex)
                {

                    ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "PreorderSettingController", "Error : " + ex.Message, null, "Update");
                    resultSts = 0;
                }


            }

            return Json(new { result = resultSts });
        }
    }
// Controller Helper Class
    public class  PreorderSettingsHelper
    {
        private UnitOfWork unitOfWork = null;
        public PreorderSettingsHelper()
        {
            unitOfWork = new UnitOfWork(ClientInfoData.getConectionString());

        }

        public PreorderSettingModel SelectData(long clientID)
        {
            
            int? POPickMode = 0;
            string POPickModeDescription = "";

            SystemOptions getDataObject = unitOfWork.SystemOptionRepository.GetQuery(s => s.ClientID == clientID).FirstOrDefault();

            if (getDataObject != null)
            { 
                POPickMode = getDataObject.POPickupMode;
            }  
            
            POPickModeDescription = GetPOPickModeDescription(POPickMode);

            PreorderSettingModel viewModel = new PreorderSettingModel() { Id = clientID,
                                                                          POPickMode = POPickMode,
                                                                          POPickModeDescription = POPickModeDescription
                                                                        };

            return viewModel;

        }

        public int UpdateData(PreorderSettingModel data)
        {
            int returnSts = 0;
            SystemOptions updatableObject = unitOfWork.SystemOptionRepository.GetQuery(s => s.ClientID == data.Id).FirstOrDefault();
            if (updatableObject!=null)
            {
                updatableObject.POPickupMode = data.POPickMode;
                updatableObject.LastUpdatedUTC = DateTime.UtcNow;

                unitOfWork.SystemOptionRepository.Update(updatableObject);
                unitOfWork.Save();
                returnSts = 1;
            }

            return returnSts;
        }
     private string GetPOPickModeDescription(int? mode){
         
         string description="";

         switch (mode)
         {
             case 0:
                 description = "In this mode, a user is responsible for manually marking each preorder item as picked up.";
                 break;
             case 1:
                 description = "In this mode, the server will automatically mark all preorder items that were to be served today.  A User may come back into this screen and manually mark them as not picked up or picked up.";
                 break;
             case 2:
                 description = "In this mode, the cashier at the POS will be able to mark the preordered items as the customers come through the line" +
                 "to pickup their items.  A User may come back into this screen and manually mark the items as picked up.";
                 break;
            
         }
         return description;

     }

    }

}