using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Linq;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Web;
using System.IO;
using System.Data;
using System.Web.Mvc;
using System.Data.Common;
using System.Web.Mvc.Ajax;
using BTDatabaseApplicationAppFramework.Models;
using System.Data.Entity.Core.EntityClient;
using System.Net;
using PagedList;
using System.Configuration;
using System.Web.Routing;
using System.Globalization;

namespace BTDatabaseApplicationAppFramework.Controllers
{

    public class HomeController: Controller
    {
       private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ActionResult Index()
            {
            
               return View();
            }

        [Authorize]
    
        public ActionResult About()
            {
            //lblVersion.Text =

            var asm = System.Reflection.Assembly.GetExecutingAssembly();            
               ViewBag.Message = "Version: " + asm.GetName().Version.ToString();

            logger.Info("About menu clicked...");
            return View();
            }

        public ActionResult Test()
        {

            return View();
        }

        [Authorize]

        public ActionResult Contact()
            {



            ViewBag.Message = "Telephone Management System";
             logger.Info("About menu clicked...");
            return View();
            }

        [Authorize]
        public ActionResult DDIDisplay(string Sorting_Order, string Search_Data, string Filter_Value, int? Page_No)
        {

          try
          {
                
                using (var entitydb = new AtlasDatabaseEntities())
            {

                    int PagingNumber = Convert.ToInt32(ConfigurationManager.AppSettings["DDITablePagingNumber"]);

                    ViewBag.CurrentSortOrder = Sorting_Order;
                    //ViewBag.SortingName = String.IsNullOrEmpty(Sorting_Order) ? "DDI_Number" : "";
                    //ViewBag.SortingName = String.IsNullOrEmpty(Sorting_Order) ? "DDI_Assigned_To_Puid" : "";
                    //ViewBag.SortingName = String.IsNullOrEmpty(Sorting_Order) ? "DDI_Status" : "";

                    ViewBag.DDI_Number = String.IsNullOrEmpty(Sorting_Order) ? "DDI_Number" : "";
                    ViewBag.DDI_Assigned_To_Puid = String.IsNullOrEmpty(Sorting_Order) ? "DDI_Assigned_To_Puid" : "";
                    ViewBag.DDI_Status = String.IsNullOrEmpty(Sorting_Order) ? "DDI_Status" : "";

                    if (Search_Data != null)
                    {
                        Page_No = 1;
                    }
                    else
                    {
                        Search_Data = Filter_Value;
                    }

                    ViewBag.FilterValue = Search_Data;

                    var entitydbMembers = (from DDIDetail in entitydb.DDI_Details orderby DDIDetail.DDI_ID select DDIDetail).ToList();

                    if (!String.IsNullOrEmpty(Search_Data))
                    {
                        entitydbMembers = entitydbMembers.Where(DDIDetail => DDIDetail.DDI_Assigned_To_Puid.Contains(Search_Data.Trim()) || DDIDetail.DDI_Number.Contains(Search_Data.Trim())).ToList();
                    }

                    switch (Sorting_Order)
                    {
                        case "DDI_Number":
                            entitydbMembers = entitydbMembers.OrderByDescending(DDIDetail => DDIDetail.DDI_Number).ToList();
                            break;
                        case "DDI_Assigned_To_Puid":
                            entitydbMembers = entitydbMembers.OrderByDescending(DDIDetail => DDIDetail.DDI_Assigned_To_Puid).ToList();
                            break;
                          case "DDI_Status":
                            entitydbMembers = entitydbMembers.OrderByDescending(DDIDetail => DDIDetail.DDI_Status).ToList();
                            break;
                        default:
                            entitydbMembers = entitydbMembers.OrderBy(DDIDetail => DDIDetail.DDI_ID).ToList();
                            break;
                    }

                    int Size_Of_Page = PagingNumber;
                    int No_Of_Page = (Page_No ?? 1);

                    //return this.View(new ViewModelData {DDIDetails = entitydbMembers});
                    //return View(entitydbMembers);
                    return View(entitydbMembers.ToPagedList(No_Of_Page, Size_Of_Page));
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message + " in Method - ActionResult DDIDisplay()");
                return View(ex.Message);
            }
        }


        public ActionResult DDIAvaliabilityReport()
        {
            try
            {
                var ddiList = new List<DDITotalList>();
                string available = Convert.ToString(ConfigurationManager.AppSettings["DDIAvailability"]);

                using (var conn = new EntityConnection("name=AtlasDatabaseEntities"))
              {
                conn.Open();

                    using (EntityCommand cmd = conn.CreateCommand())
                {
                    // Create parameters and add them to 
                    // the EntityCommand's Parameters collection 
                    cmd.CommandText = "AtlasDatabaseEntities.TelephoneAvailability";
                    cmd.CommandType = CommandType.StoredProcedure;
                    EntityParameter param1 = new EntityParameter();
                    param1.ParameterName = "DDIStatus";
                    param1.Value = available;
                    cmd.Parameters.Add(param1);
                    using (EntityDataReader rdr = cmd.ExecuteReader(CommandBehavior.SequentialAccess))
                    {
                        // Iterate through the collection of Contact items.
                        while (rdr.Read())
                        {
                            var myList = new DDITotalList();
                            myList.DDI_Location = rdr.GetString(0);
                            myList.DDI_Number = rdr.GetString(1);
                            myList.DDI_Status = rdr.GetString(2);
                            myList.Total_DDI_Available = rdr.GetValue(3).ToString();        
                            ddiList.Add(myList);
                        }                       
                    }                  
               }

                conn.Close();
            }
            return View(ddiList);

         }catch (Exception ex)
          {
               logger.Error(ex.Message + " in Method - ActionResult DDIAvaliabilityReport()");
                return View(ex.Message);
          }
            finally
            {          
            }
        }

       


        [Authorize]
        public ActionResult ResponseGpDisplay()
        {
            try
            {
                using (var entitydb = new AtlasDatabaseEntities())
            {             
                var entitydbMembers = (from GpDetail in entitydb.Response_Group orderby GpDetail.ResponseGroup_ID select GpDetail).ToList();
                    return View(new ViewModelData { ResponseGroup = entitydbMembers });
                    //return View(entitydbMembers);
                }
        }catch (Exception ex)
         {
                logger.Error(ex.Message + " in Method - ActionResult ResponseGpDisplay()");
                return View(ex.Message);
         }
      }

        // POST: Database/Edit/5
        [HttpPost]
        public ActionResult ResponseGpDisplay(FormCollection collection)
        {       
         try
            {
                string SearchNumber = collection["SearchNumber"].Trim();

                if (ModelState.IsValid)
                {
                    using (var entitydb = new AtlasDatabaseEntities())
                    {
                        var entitydbMembers = entitydb.Response_Group.Where((D => D.Response_Group_Name == SearchNumber || D.Response_Group_DDI_Number == SearchNumber)).ToList();
                        return View(new ViewModelData { ResponseGroup = entitydbMembers});
                    }
                }
                ModelState.Clear();
                return RedirectToAction("ResponseGpDisplay");
                // TODO: Add update logic here               

            }
            catch (Exception ex)
            {
                logger.Error(ex.Message + " in Method - ActionResult ResponseGpDisplay(FormCollection collection)");
                return View(ex.Message);
            }
        }


        [Authorize]
        public ActionResult ResponseGpMemberDisplay()
        {
            try
            {
                using (var entitydb = new AtlasDatabaseEntities())
              {              
                var entitydbMembers = (from GpMemberDetail in entitydb.Response_Group_Members orderby GpMemberDetail.Response_GroupMember_ID select GpMemberDetail).ToList();

                    return this.View(new ViewModelData { ResponseGroupMembers = entitydbMembers });
                    //return View(entitydbMembers);
                }
            }catch (Exception ex)
            {
                logger.Error(ex.Message + " in Method - ActionResult ResponseGpMemberDisplay()");
                return View(ex.Message);
            }
        }


        // POST: Database/Edit/5
        [HttpPost]
        public ActionResult ResponseGpMemberDisplay(FormCollection collection)
        {
            try
            {
                string SearchNumber = collection["SearchNumber"].Trim();

                if (ModelState.IsValid)
                {
                    using (var entitydb = new AtlasDatabaseEntities())
                    {
                        var entitydbMembers = entitydb.Response_Group_Members.Where((D => D.Response_GroupMember_PUID_Name == SearchNumber)).ToList();
                        return View(new ViewModelData { ResponseGroupMembers = entitydbMembers });
                    }
                }
                ModelState.Clear();
                return RedirectToAction("ResponseGpMemberDisplay");
                // TODO: Add update logic here               

            }
            catch (Exception ex)
            {
                logger.Error(ex.Message + " in Method - ActionResult ResponseGpMemberDisplay(FormCollection collection)");
                return View(ex.Message);
            }
        }

        public ActionResult ResponseGpMemberReport()
        {
            try
            {
                var memberList = new List<ResponseGroupMembersList>();

                using (var conn = new EntityConnection("name=AtlasDatabaseEntities"))
                {
                    conn.Open();

                    string commandText = @"SELECT Response_Group_Members.Response_GroupMember_PUID_Name, Response_Group_Members.Response_GroupMember_Name, Response_Group_Members.Response_GroupMember_Type FROM AtlasDatabaseEntities.Response_Group_Members where Response_Group_Members.Response_GroupMember_PUID_Name <> @ResponseGroupMemberPUIDName group by Response_Group_Members.Response_GroupMember_Type, Response_Group_Members.Response_GroupMember_Name, Response_Group_Members.Response_GroupMember_PUID_Name order by Response_Group_Members.Response_GroupMember_Name, Response_Group_Members.Response_GroupMember_Type desc";

                    using (EntityCommand cmd = new EntityCommand(commandText, conn))
                    {
                        // Create two parameters and add them to 
                        // the EntityCommand's Parameters collection 
                        EntityParameter param1 = new EntityParameter();
                        param1.ParameterName = "ResponseGroupMemberPUIDName";
                        param1.Value = "";
                        cmd.Parameters.Add(param1);
                        using (DbDataReader rdr = cmd.ExecuteReader(CommandBehavior.SequentialAccess))
                        {
                            // Iterate through the collection of Contact items.
                            while (rdr.Read())
                            {
                                var myList = new ResponseGroupMembersList();
                                myList.Response_GroupMember_PUID_Name = rdr.GetString(0);
                                myList.Response_GroupMember_Name = rdr.GetString(1);
                                myList.Response_GroupMember_Type = rdr.GetValue(2).ToString();
                                memberList.Add(myList);
                            }
                        }
                    }
                    conn.Close();
                }
                return View(memberList);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message + " in Method - ActionResult ResponseGpMemberReport()");
                return View(ex.Message);
            }
            finally
            {
            }
        }



        // GET: Database/Details/5
        public ActionResult DDIChanges(int id, string DNumber, string DAssignedToPuid, string DEmailAddress, DateTime DLastAlloDate, string DIStatus, DateTime DToBeVacatedDate, string DLocation, string DNumberType)
        {
            try
            {
                DateTime DDILastAllocatedDate = Convert.ToDateTime((DateTime.Now).ToShortDateString().Trim());
                string lastAllocateDate = String.Format("{0:dd/MM/yyyy}", DDILastAllocatedDate);

                DateTime DDIToBeVacatedDate = Convert.ToDateTime((DateTime.Now).ToShortDateString().Trim());
                string toBevacatedDate = String.Format("{0:dd/MM/yyyy}", DDIToBeVacatedDate);

                ViewBag.DDI_ID = id;
                ViewBag.DDI_Number = DNumber;
                ViewBag.DDI_Assigned_To_Puid = DAssignedToPuid;
                ViewBag.DDI_Email_Address = DEmailAddress;
  
                if (DLastAlloDate != null)
                {
                    ViewBag.DDI_Last_Allocated_Date = DLastAlloDate.ToShortDateString();
                }
                else
                {
                    ViewBag.DDI_Last_Allocated_Date = lastAllocateDate;
                }

                if (DToBeVacatedDate != null)
                {
                    ViewBag.DDI_To_Be_Vacated_Date = DToBeVacatedDate.ToShortDateString();
                }
                else
                {
                    ViewBag.DDI_To_Be_Vacated_Date = toBevacatedDate;
                }

                ViewBag.DDI_Status = DIStatus;
                ViewBag.DDI_Location = DLocation;
                ViewBag.DDI_Number_Type = DNumberType;

                DDI_Details entitydb = new DDI_Details();
                using (AtlasDatabaseEntities entityGroup = new AtlasDatabaseEntities())
                entitydb.DDIStatusCollection = entityGroup.DDI_Status.ToList<DDI_Status>();

                using (AtlasDatabaseEntities entityStatus = new AtlasDatabaseEntities())
                entitydb.LocationCollection = entityStatus.Locations.ToList<Locations>();

                using (AtlasDatabaseEntities entityStatus = new AtlasDatabaseEntities())
                entitydb.ResponseGrpMemberTypesCollection = entityStatus.Response_Grp_MemberTypes.ToList<Response_Grp_MemberTypes>();

                return View(entitydb);
          }
          catch (Exception ex)
          {
                logger.Error(ex.Message + " in Method - ActionResult DDIChanges(int id, string DNumber, string DAssignedToPuid, string DEmailAddress, DateTime DLastAlloDate, string DIStatus, DateTime DToBeVacatedDate, string DLocation, string DNumberType)");
                return View(ex.Message);
          }
        }



        // POST: Database/Edit/5
        [HttpPost]
        public ActionResult DDIChanges(int id, FormCollection collection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string DDI_Number = collection["DDI_Number"].Trim();
                    string DDI_Assigned_To_Puid = collection["DDI_Assigned_To_Puid"].Trim();
                    string DDI_Email_Address = collection["DDI_Email_Address"];
                    string DDI_Last_Allocated_Date = collection["DDI_Last_Allocated_Date"].Trim();
                    string DDI_Status = collection["DDI_Status"].Trim();
                    string DDI_To_Be_Vacated_Date = collection["DDI_To_Be_Vacated_Date"].Trim();
                    string DDI_Location = collection["DDI_Location"].Trim();
                    string DDI_Number_Type = collection["DDI_Number_Type"].Trim();

                    DateTime dt1 = DateTime.ParseExact(DDI_Last_Allocated_Date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    var convertedDate1 = dt1.ToString("MM/dd/yyyy");
 
                    DateTime dt2 = DateTime.ParseExact(DDI_To_Be_Vacated_Date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    var convertedDate2 = dt2.ToString("MM/dd/yyyy");

                    using (var entitydb = new AtlasDatabaseEntities())
                    {
                        if (entitydb.DDI_Details.Any(p => p.DDI_Assigned_To_Puid == DDI_Assigned_To_Puid))
                        {
                            var entitydbMemberDDI = (from DDIDetail in entitydb.DDI_Details where DDIDetail.DDI_Assigned_To_Puid == DDI_Assigned_To_Puid orderby DDIDetail.DDI_ID select DDIDetail).ToList().FirstOrDefault();
                            if (entitydbMemberDDI != null)
                            {
                                if (entitydbMemberDDI.DDI_ID == id)
                                {
                                    entitydb.Database.ExecuteSqlCommand("Update DDI_Details SET DDI_Number = '" + DDI_Number + "', DDI_Assigned_To_Puid = '" + DDI_Assigned_To_Puid + "', DDI_Email_Address = '" + DDI_Email_Address + "', DDI_Last_Allocated_Date = '" + convertedDate1 + "', DDI_Status = '" + DDI_Status + "', DDI_To_Be_Vacated_Date = '" + convertedDate2 + "', DDI_Location = '" + DDI_Location + "', DDI_Number_Type = '" + DDI_Number_Type + "'  Where DDI_ID = @DDIID", new SqlParameter("@DDIID", id));
                                }
                                else
                                {
                                    TempData["Message"] = "The puid already exists";
                                    return RedirectToAction("DDIChanges", "Home", new { @id = id, @DNumber = DDI_Number, @DAssignedToPuid = DDI_Assigned_To_Puid, @DEmailAddress = DDI_Email_Address, @DLastAlloDate = DDI_Last_Allocated_Date, @DIStatus = DDI_Status, @DToBeVacatedDate = DDI_To_Be_Vacated_Date, @DLocation = DDI_Location, @DNumberType = DDI_Number_Type });
                                }
                            }
                        }
                        else
                        {
                            entitydb.Database.ExecuteSqlCommand("Update DDI_Details SET DDI_Number = '" + DDI_Number + "', DDI_Assigned_To_Puid = '" + DDI_Assigned_To_Puid + "', DDI_Email_Address = '" + DDI_Email_Address + "', DDI_Last_Allocated_Date = '" + convertedDate1 + "', DDI_Status = '" + DDI_Status + "', DDI_To_Be_Vacated_Date = '" + convertedDate2 + "', DDI_Location = '" + DDI_Location + "', DDI_Number_Type = '" + DDI_Number_Type + "'  Where DDI_ID = @DDIID", new SqlParameter("@DDIID", id));
                        }
                    }
                }
                ModelState.Clear();
                return RedirectToAction("DDIDisplay");

            }
            catch (Exception ex)
            {
                logger.Error(ex.Message + " in Method - ActionResult DDIChanges(int id, FormCollection collection)");
                return View(ex.Message);
            }
        }

   

    // GET: Database/Create
    public ActionResult DDIAdd()
        {
            try
            {

             DDI_Details entitydb = new DDI_Details();
            using (AtlasDatabaseEntities entityGroup = new AtlasDatabaseEntities())                       
                entitydb.DDIStatusCollection = entityGroup.DDI_Status.ToList<DDI_Status>();

            using (AtlasDatabaseEntities entityStatus = new AtlasDatabaseEntities())
                entitydb.LocationCollection = entityStatus.Locations.ToList<Locations>();

            using (AtlasDatabaseEntities entityStatus = new AtlasDatabaseEntities())
               entitydb.ResponseGrpMemberTypesCollection = entityStatus.Response_Grp_MemberTypes.ToList<Response_Grp_MemberTypes>();
          
            return View(entitydb);

            }catch (Exception ex)
            {
                logger.Error(ex.Message + " in Method - ActionResult DDIAdd()");
                return View(ex.Message);
            }
        }



        // POST: Database/Create
        [HttpPost]
        public ActionResult DDIAdd(FormCollection collection )
        { 
            try
            {
                         
                if (ModelState.IsValid)
                {
                        DDI_Details entitydb = new DDI_Details();
                        entitydb.DDI_Number = collection["DDI_Number"].Trim();
                        entitydb.DDI_Assigned_To_Puid = collection["DDI_Assigned_To_Puid"].Trim();
                        entitydb.DDI_Email_Address = collection["DDI_Email_Address"].Trim();
                        entitydb.DDI_Last_Allocated_Date = Convert.ToDateTime(collection["DDI_Last_Allocated_Date"].Trim());

                        DateTime dt1 = DateTime.ParseExact("01/01/0001", "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        var convertedDate1 = dt1.ToString("MM/dd/yyyy");

                        entitydb.DDI_To_Be_Vacated_Date = Convert.ToDateTime(convertedDate1.Trim());
                        entitydb.DDI_Status = collection["DDI_Status"].Trim();
                        entitydb.DDI_Location = collection["DDI_Location"].Trim();
                        entitydb.DDI_Number_Type = collection["DDI_Number_Type"].Trim();
                
                    AtlasDatabaseEntities entitydbAll = new AtlasDatabaseEntities();
                    if (entitydbAll.DDI_Details.Any(p => p.DDI_Number == entitydb.DDI_Number))
                    {
                        TempData["Message"] = "The DDI number already exists";
                        return RedirectToAction("DDIAdd");
                    }
                    else if (entitydbAll.DDI_Details.Any(p => p.DDI_Assigned_To_Puid == entitydb.DDI_Assigned_To_Puid))
                       {
                            TempData["Message"] = "The Puid number already exists";
                            return RedirectToAction("DDIAdd");
                   }else {
                        entitydbAll.DDI_Details.Add(entitydb);
                        entitydbAll.SaveChanges();
                     }                  
                  }

                //ViewData["result"] = result;
                ModelState.Clear();
                return RedirectToAction("DDIDisplay");
                
                
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message + " in Method - ActionResult DDIAdd(FormCollection collection )" );
                return View(ex.Message);
            }
        }




        // GET: Database/Edit/5
        public ActionResult DDIMoves(int id, string DNumber, string DAssignedToPuid, string DEmailAddress, DateTime DLastAlloDate, string DIStatus, DateTime DToBeVacatedDate, string DLocation, string DNumberType)
        {

            try
            {
                DateTime DDILastAllocatedDate = Convert.ToDateTime((DateTime.Now).ToShortDateString().Trim());
                string lastAllocateDate = String.Format("{0:dd/MM/yyyy}", DDILastAllocatedDate);

                int tobeVacatedDays = Convert.ToInt32(ConfigurationManager.AppSettings["TObEvacatedDays"]);
                DateTime DDIToBeVacatedDate = Convert.ToDateTime((DateTime.Now.AddDays(tobeVacatedDays)).ToShortDateString().Trim());
                string toBevacatedDate = String.Format("{0:dd/MM/yyyy}", DDIToBeVacatedDate);

              
                ViewBag.DDI_ID = id;
                ViewBag.DDI_Number = DNumber;
                ViewBag.DDI_Assigned_To_Puid = DAssignedToPuid;
                ViewBag.DDI_Email_Address = DEmailAddress;

                //if (DLastAlloDate != null)
                //{
                   // ViewBag.DDI_Last_Allocated_Date = DLastAlloDate.ToShortDateString();
                //}else{
                    ViewBag.DDI_Last_Allocated_Date = lastAllocateDate;
                //}
                
                //if (DToBeVacatedDate != null)
                //{
                  //  ViewBag.DDI_To_Be_Vacated_Date = DToBeVacatedDate.ToShortDateString();
                //}else{
                    ViewBag.DDI_To_Be_Vacated_Date = toBevacatedDate;
                //}

                ViewBag.DDI_Status = DIStatus;
                ViewBag.DDI_Location = DLocation;
                ViewBag.DDI_Number_Type = DNumberType;

                DDI_Details entitydb = new DDI_Details();
                using (AtlasDatabaseEntities entityGroup = new AtlasDatabaseEntities())
                entitydb.DDIStatusCollection = entityGroup.DDI_Status.ToList<DDI_Status>();

                using (AtlasDatabaseEntities entityStatus = new AtlasDatabaseEntities())
                entitydb.LocationCollection = entityStatus.Locations.ToList<Locations>();

                using (AtlasDatabaseEntities entityStatus = new AtlasDatabaseEntities())
                entitydb.ResponseGrpMemberTypesCollection = entityStatus.Response_Grp_MemberTypes.ToList<Response_Grp_MemberTypes>();

                return View(entitydb);

         }catch (Exception ex)
            {
                logger.Error(ex.Message + " in Method - ActionResult DDIMoves(int id, string DNumber, string DAssignedToPuid, string DEmailAddress, DateTime DLastAlloDate, string DIStatus, DateTime DToBeVacatedDate, string DLocation, string DNumberType)");
                return View(ex.Message);
            }
        }



        // POST: Database/Edit/5
        [HttpPost]
        public ActionResult DDIDisplay(FormCollection collection)
        {
            try
            {
                string SearchNumber = collection["SearchNumber"].Trim();
                   
               if (ModelState.IsValid)
                {
                    using (var entitydb = new AtlasDatabaseEntities())
                    {
                        var entitydbMembers = entitydb.DDI_Details.Where((D => D.DDI_Assigned_To_Puid == SearchNumber || D.DDI_Number == SearchNumber)).ToList();
                        return View(new ViewModelData { DDIDetails = entitydbMembers });
                    }                                       
                }
                ModelState.Clear();
                return RedirectToAction("DDIDisplay");
                    // TODO: Add update logic here               
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message + " in Method - ActionResult DDIDisplay(FormCollection collection)");
                return View(ex.Message);
            }
        }
      

        // POST: Database/Edit/5
        [HttpPost]
        public ActionResult DDIMoves(int id, FormCollection collection)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    // TODO: Add update logic here
                    string DDI_Number = collection["DDI_Number"].Trim();
                    string DDI_Assigned_To_Puid = collection["DDI_Assigned_To_Puid"].Trim();
                    string DDI_Email_Address = collection["DDI_Email_Address"];
                    string DDI_Last_Allocated_Date = collection["DDI_Last_Allocated_Date"].Trim();
                    string DDI_Status = collection["DDI_Status"].Trim();
                    string DDI_To_Be_Vacated_Date = collection["DDI_To_Be_Vacated_Date"].Trim();
                    string DDI_Location = collection["DDI_Location"].Trim();
                    string DDI_Number_Type = collection["DDI_Number_Type"].Trim();

                    DateTime dt1 = DateTime.ParseExact(DDI_Last_Allocated_Date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    var convertedDate1 = dt1.ToString("MM/dd/yyyy");

                    DateTime dt2 = DateTime.ParseExact(DDI_To_Be_Vacated_Date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    var convertedDate2 = dt2.ToString("MM/dd/yyyy");

                    using (var entitydb = new AtlasDatabaseEntities())
                    {
                        if (entitydb.DDI_Details.Any(p => p.DDI_Assigned_To_Puid == DDI_Assigned_To_Puid))
                        {
                            var entitydbMemberDDI = (from DDIDetail in entitydb.DDI_Details where DDIDetail.DDI_Assigned_To_Puid == DDI_Assigned_To_Puid orderby DDIDetail.DDI_ID select DDIDetail).ToList().FirstOrDefault();
                            if (entitydbMemberDDI != null)
                            {
                                if (entitydbMemberDDI.DDI_ID == id)
                                {
                                    entitydb.Database.ExecuteSqlCommand("Update DDI_Details SET DDI_Number = '" + DDI_Number + "', DDI_Assigned_To_Puid = '" + DDI_Assigned_To_Puid + "', DDI_Email_Address = '" + DDI_Email_Address + "', DDI_Last_Allocated_Date = '" + convertedDate1 + "', DDI_Status = '" + DDI_Status + "', DDI_To_Be_Vacated_Date = '" + convertedDate2 + "', DDI_Location = '" + DDI_Location + "', DDI_Number_Type = '" + DDI_Number_Type + "'  Where DDI_ID = @DDIID", new SqlParameter("@DDIID", id));
                                }
                                else
                                {
                                    TempData["Message"] = "The puid already exists";
                                    return RedirectToAction("DDIMoves", "Home", new { @id = id, @DNumber = DDI_Number, @DAssignedToPuid = DDI_Assigned_To_Puid, @DEmailAddress = DDI_Email_Address, @DLastAlloDate = DDI_Last_Allocated_Date, @DIStatus = DDI_Status, @DToBeVacatedDate = DDI_To_Be_Vacated_Date, @DLocation = DDI_Location, @DNumberType = DDI_Number_Type });
                                }
                            }
                        }
                        else
                        {
                            entitydb.Database.ExecuteSqlCommand("Update DDI_Details SET DDI_Number = '" + DDI_Number + "', DDI_Assigned_To_Puid = '" + DDI_Assigned_To_Puid + "', DDI_Email_Address = '" + DDI_Email_Address + "', DDI_Last_Allocated_Date = '" + String.Format("{0:MM/dd/yyyy}", DDI_Last_Allocated_Date) + "', DDI_Status = '" + DDI_Status + "', DDI_To_Be_Vacated_Date = '" + String.Format("{0:MM/dd/yyyy}", DDI_To_Be_Vacated_Date) + "', DDI_Location = '" + DDI_Location + "', DDI_Number_Type = '" + DDI_Number_Type + "'  Where DDI_ID = @DDIID", new SqlParameter("@DDIID", id));
                        }
                    }
                }
                    ModelState.Clear();
                    return RedirectToAction("DDIDisplay");
                
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message + " in Method - ActionResult DDIMoves(int id, FormCollection collection)");
                return View(ex.Message);
            }
        }


        




// GET: Database/Delete/5
public ActionResult DDIDelete(int id)
        {

          try
         {
                using (var entitydb = new AtlasDatabaseEntities())
            {
                entitydb.Database.ExecuteSqlCommand("Delete from DDI_Details Where DDI_ID = @DDIID", new SqlParameter("@DDIID", id));
            }
            return RedirectToAction("DDIDisplay");
                //return View();
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message + " in Method - ActionResult DDIDelete(int id)");
                return View(ex.Message);
            }
        }



        // POST: Database/Delete/5
        [HttpPost]
        public ActionResult DDIDelete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message + " in Method - DDIDelete(int id, FormCollection collection)");
                return View(ex.Message);
            }
        }


    


        // GET: Database/Details/5
        public ActionResult ResponseGpChanges(int id, string ResponseGroupName, string ResponseGroupDDINumber, string ResponseGroupOwner, string ResponseGroupEmail)
        {
            try
            {
                ViewBag.ResponseGroup_ID = id;
                ViewBag.Response_Group_Name = ResponseGroupName;
                ViewBag.Response_Group_DDI_Number = ResponseGroupDDINumber;
                ViewBag.Response_Group_Owner = ResponseGroupOwner;
                ViewBag.Response_Group_Email = ResponseGroupEmail;

                return View();
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message + " in Method - ActionResult ResponseGpChanges(int id, string ResponseGroupName, string ResponseGroupDDINumber, string ResponseGroupOwner, string ResponseGroupEmail)");
                return View(ex.Message);
            }
        }


        // POST: Database/Edit/5
        [HttpPost]
        public ActionResult ResponseGpChanges(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
                string Response_Group_Name = collection["Response_Group_Name"].Trim();
                string Response_Group_DDI_Number = collection["Response_Group_DDI_Number"].Trim();
                string Response_Group_Owner = collection["Response_Group_Owner"].Trim();
                string Response_Group_Email = collection["Response_Group_Email"].Trim();

                using (var entitydb = new AtlasDatabaseEntities())
                {

                    entitydb.Database.ExecuteSqlCommand("Update Response_Group  SET Response_Group_Name = '" + Response_Group_Name + "', Response_Group_DDI_Number = '" + Response_Group_DDI_Number + "', Response_Group_Owner = '" + Response_Group_Owner + "', Response_Group_Email = '" + Response_Group_Email + "' Where  ResponseGroup_ID = @ResponseGroupID", new SqlParameter("@ResponseGroupID", id));
                }
                return RedirectToAction("ResponseGpDisplay");

            }
            catch (Exception ex)
            {
                logger.Error(ex.Message + " in Method - public ActionResult ResponseGpChanges(int id, FormCollection collection)");
                return View(ex.Message);
            }
        }



        // GET: Database/Create
        public ActionResult ResponseGpAdd()
        {
            return View();
        }

        // POST: Database/Create
        [HttpPost]
        public ActionResult ResponseGpAdd(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here
                if (ModelState.IsValid)
                {
                    Response_Group entitydb = new Response_Group();
                    entitydb.Response_Group_Owner = collection["Response_Group_Owner"].Trim();
                    entitydb.Response_Group_DDI_Number = collection["Response_Group_DDI_Number"].Trim();
                    entitydb.Response_Group_Email = collection["Response_Group_Email"].Trim();
                    entitydb.Response_Group_Name = collection["Response_Group_Name"].Trim();

                    AtlasDatabaseEntities entitydbAll = new AtlasDatabaseEntities();
                    if (entitydbAll.Response_Group.Any(p => p.Response_Group_Name == entitydb.Response_Group_Name))
                    {
                        TempData["Message"] = "The Group name already exists";
                        return RedirectToAction("ResponseGpAdd");
                    }
                    else
                    {
                        entitydbAll.Response_Group.Add(entitydb);
                        entitydbAll.SaveChanges();                      
                    }
                }

                ModelState.Clear();
                return RedirectToAction("ResponseGpDisplay");
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message + " in Method - ActionResult ResponseGpAdd(FormCollection collection)");
                return View(ex.Message);
            }
        }


        // GET: Database/Delete/5
        public ActionResult ResponseGpDelete(int id, string ResponseGroupDDINumber)
        {
            try
            {
               
                string available = Convert.ToString(ConfigurationManager.AppSettings["DDIAvailability"]);
                int tobeVacatedDays = Convert.ToInt32(ConfigurationManager.AppSettings["TObEvacatedDays"]);
                using (var entitydb = new AtlasDatabaseEntities())
                {
                  entitydb.Database.ExecuteSqlCommand("Delete from Response_Group Where  ResponseGroup_ID = @GroupID", new SqlParameter("@GroupID", id));

                        string DDILastAllocatedDate = (DateTime.Now).ToShortDateString().Trim();
                        string DDIToBeVacatedDate = (DateTime.Now.AddDays(tobeVacatedDays)).ToShortDateString().Trim();

                        DateTime dt1 = DateTime.ParseExact(DDILastAllocatedDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        var convertedDate1 = dt1.ToString("MM/dd/yyyy");

                        DateTime dt2 = DateTime.ParseExact(DDIToBeVacatedDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        var convertedDate2 = dt2.ToString("MM/dd/yyyy");

                    entitydb.Database.ExecuteSqlCommand("Update DDI_Details SET DDI_Status = '" + available + "',  DDI_Last_Allocated_Date = '" + convertedDate1 + "', DDI_To_Be_Vacated_Date = '" + convertedDate2 + "' Where  DDI_Number = @GroupDDI", new SqlParameter("@GroupDDI", ResponseGroupDDINumber));               
                }
            return RedirectToAction("ResponseGpDisplay");
            //return View();

           }
            catch (Exception ex)
            {
                logger.Error(ex.Message + " in Method - ActionResult ResponseGpDelete(int id)");
                return View(ex.Message);
           }
        }


        // POST: Database/Delete/5
        [HttpPost]
        public ActionResult ResponseGpDelete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return View(ex.Message);
            }
        }



        // GET: Database/Details/5
        public ActionResult ResponseGpMemberChanges(int id, string ResponseGpMemberPUIDName, string ResponseGpMemberEmail, string ResponseGpMemberName, string ResponseGpMemberType)
        {
            try
            {
                ViewBag.Response_GroupMember_ID = id;
                ViewBag.Response_GroupMember_PUID_Name = ResponseGpMemberPUIDName;
                ViewBag.Response_GroupMember_Email = ResponseGpMemberEmail;
                ViewBag.Response_GroupMember_Name = ResponseGpMemberName;
                ViewBag.Response_GroupMember_Type = ResponseGpMemberType;

                Response_Group_Members entitydb = new Response_Group_Members();

                using (AtlasDatabaseEntities entityGroup = new AtlasDatabaseEntities())
                    entitydb.ResponseGroupCollection = entityGroup.Response_Group.ToList<Response_Group>();

                using (AtlasDatabaseEntities entityGroup = new AtlasDatabaseEntities())
                    entitydb.ResponseGrpMemberTypesCollection = entityGroup.Response_Grp_MemberTypes.ToList<Response_Grp_MemberTypes>();

                return View(entitydb);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message + " in Method - ActionResult ResponseGpMemberChanges(int id, string ResponseGpMemberPUIDName, string ResponseGpMemberEmail, string ResponseGpMemberName, string ResponseGpMemberType)");
                return View(ex.Message);
            }
        }



        // GET: Database/Create
        public ActionResult ResponseGpMemberAdd()
        {
            try
            {
                Response_Group_Members entitydb = new Response_Group_Members();

            using (AtlasDatabaseEntities entityGroup= new AtlasDatabaseEntities())
            entitydb.ResponseGroupCollection = entityGroup.Response_Group.ToList<Response_Group>();

            using (AtlasDatabaseEntities entityGroup  = new AtlasDatabaseEntities())
                entitydb.ResponseGrpMemberTypesCollection = entityGroup.Response_Grp_MemberTypes.ToList<Response_Grp_MemberTypes>();
             return View(entitydb);

            }

            catch (Exception ex)
            {
                logger.Error(ex.Message + " in Method - ActionResult ResponseGpMemberAdd()");
                return View(ex.Message);
            }
        }




        // POST: Database/Create
        [HttpPost]
        public ActionResult ResponseGpMemberAdd(FormCollection collection)
{
            try
            {
                // TODO: Add insert logic here
                if (ModelState.IsValid)
                {
                    Response_Group_Members entitydb = new Response_Group_Members();
                    entitydb.Response_GroupMember_PUID_Name = collection["Response_GroupMember_PUID_Name"].Trim();
                    entitydb.Response_GroupMember_Email = collection["Response_GroupMember_Email"].Trim();
                    entitydb.Response_GroupMember_Name = collection["Response_GroupMember_Name"].Trim();
                    entitydb.Response_GroupMember_Type = collection["Response_GroupMember_Type"].Trim();
                
                    AtlasDatabaseEntities entitydbAll = new AtlasDatabaseEntities();
                    if (entitydbAll.Response_Group_Members.Any(p => p.Response_GroupMember_PUID_Name == entitydb.Response_GroupMember_PUID_Name))
                    {
                        TempData["Message"] = "The Group Member Puid already exists";
                        return RedirectToAction("ResponseGpMemberAdd");
                    }
                    else
                    {
                        entitydbAll.Response_Group_Members.Add(entitydb);
                        entitydbAll.SaveChanges();
                    }             
                }
            
                ModelState.Clear();
                return RedirectToAction("ResponseGpMemberDisplay");
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message + " in Method - ActionResult ResponseGpMemberAdd(FormCollection collection)");
                return View(ex.Message);
            }
        }


        // POST: Database/Edit/5
        [HttpPost]
        public ActionResult ResponseGpMemberMoves(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
                string Response_GroupMember_PUID_Name = collection["Response_GroupMember_PUID_Name"].Trim();
                string Response_GroupMember_Email = collection["Response_GroupMember_Email"].Trim();
                string Response_GroupMember_Name = collection["Response_GroupMember_Name"].Trim();
                string Response_GroupMember_Type = collection["Response_GroupMember_Type"].Trim();

                if (ModelState.IsValid)
                {
                    using (var entitydb = new AtlasDatabaseEntities())
                    {
                        string Primaryflag = "Primary";
                        if (Response_GroupMember_Type != Primaryflag)
                        {
                            entitydb.Database.ExecuteSqlCommand("Update Response_Group_Members SET Response_GroupMember_PUID_Name = '" + Response_GroupMember_PUID_Name + "', Response_GroupMember_Email = '" + Response_GroupMember_Email + "' , Response_GroupMember_Name= '" + Response_GroupMember_Name + "', Response_GroupMember_Type = '" + Response_GroupMember_Type + "' Where  Response_GroupMember_ID = @ResponseGroupMemberID", new SqlParameter("@ResponseGroupMemberID", id));
                            return RedirectToAction("ResponseGpMemberDisplay");
                        }
                        else
                        {
                            using (var conn = new EntityConnection("name=AtlasDatabaseEntities"))
                            {
                                conn.Open();
                                string commandText = @"SELECT Response_Group_Members.Response_GroupMember_PUID_Name, Response_Group_Members.Response_GroupMember_Name FROM AtlasDatabaseEntities.Response_Group_Members where (Response_Group_Members.Response_GroupMember_Name = @ResponseGroupMemberName AND Response_Group_Members.Response_GroupMember_Type = @ResponseGroupMemberType)";
                                using (EntityCommand cmd = new EntityCommand(commandText, conn))
                                {
                                    // Create two parameters and add them to 
                                    // the EntityCommand's Parameters collection 
                                    EntityParameter param1 = new EntityParameter();
                                    param1.ParameterName = "ResponseGroupMemberName";
                                    param1.Value = Response_GroupMember_Name;

                                    EntityParameter param2 = new EntityParameter();
                                    param2.ParameterName = "ResponseGroupMemberType";
                                    param2.Value = Primaryflag;

                                    cmd.Parameters.Add(param1);
                                    cmd.Parameters.Add(param2);
                                    using (DbDataReader rdr = cmd.ExecuteReader(CommandBehavior.SequentialAccess))
                                    {
                                        string memberpuid = "";
                                        string memberName;
                                        // Iterate through the collection of Contact items.
                                        while (rdr.Read())
                                        {
                                            memberpuid = rdr.GetString(0);
                                            memberName = rdr.GetString(1);
                                        }
                                        if (rdr.HasRows)
                                        {
                                            if (Response_GroupMember_PUID_Name == memberpuid)
                                            {
                                                entitydb.Database.ExecuteSqlCommand("Update Response_Group_Members SET Response_GroupMember_PUID_Name = '" + Response_GroupMember_PUID_Name + "', Response_GroupMember_Email = '" + Response_GroupMember_Email + "' , Response_GroupMember_Name= '" + Response_GroupMember_Name + "', Response_GroupMember_Type = '" + Response_GroupMember_Type + "' Where  Response_GroupMember_ID = @ResponseGroupMemberID", new SqlParameter("@ResponseGroupMemberID", id));
                                            }
                                            else
                                            {
                                                TempData["message"] = "Primary Group member already exists";
                                                conn.Close();
                                                return RedirectToAction("ResponseGpMemberMoves", "Home", new { @id = id, @ResponseGpMemberPUIDName = Response_GroupMember_PUID_Name, @ResponseGpMemberEmail = Response_GroupMember_Email, @ResponseGpMemberName = Response_GroupMember_Name, @ResponseGpMemberType = Response_GroupMember_Type });
                                            }
                                        }
                                        else
                                        {
                                            entitydb.Database.ExecuteSqlCommand("Update Response_Group_Members SET Response_GroupMember_PUID_Name = '" + Response_GroupMember_PUID_Name + "', Response_GroupMember_Email = '" + Response_GroupMember_Email + "' , Response_GroupMember_Name= '" + Response_GroupMember_Name + "', Response_GroupMember_Type = '" + Response_GroupMember_Type + "' Where  Response_GroupMember_ID = @ResponseGroupMemberID", new SqlParameter("@ResponseGroupMemberID", id));
                                        }
                                    }
                                }
                                conn.Close();
                            }
                        }
                    }
                }
                ModelState.Clear();
                return RedirectToAction("ResponseGpMemberDisplay");
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message + " in Method - ActionResult ResponseGpMemberMoves(int id, FormCollection collection)");
                return View(ex.Message);
            }
        }


        // GET: Database/Edit/5
        public ActionResult ResponseGpMemberMoves(int id, string ResponseGpMemberPUIDName, string ResponseGpMemberEmail, string ResponseGpMemberName, string ResponseGpMemberType)
        {
            try
            {
                ViewBag.Response_GroupMember_ID = id;
                ViewBag.Response_GroupMember_PUID_Name = ResponseGpMemberPUIDName;
                ViewBag.Response_GroupMember_Email = ResponseGpMemberEmail;
                ViewBag.Response_GroupMember_Name = ResponseGpMemberName;
                ViewBag.Response_GroupMember_Type = ResponseGpMemberType;


                Response_Group_Members entitydb = new Response_Group_Members();
                using (AtlasDatabaseEntities entityGroup = new AtlasDatabaseEntities())
                entitydb.ResponseGroupCollection = entityGroup.Response_Group.ToList<Response_Group>();

                using (AtlasDatabaseEntities entityGroup = new AtlasDatabaseEntities())
                entitydb.ResponseGrpMemberTypesCollection = entityGroup.Response_Grp_MemberTypes.ToList<Response_Grp_MemberTypes>();

                return View(entitydb);

            }
            catch (Exception ex)
            {
                logger.Error(ex.Message + " in Method - ActionResult ResponseGpMemberMoves(int id, string ResponseGpMemberPUIDName, string ResponseGpMemberEmail, string ResponseGpMemberName, string ResponseGpMemberType)");
                return View(ex.Message);
            }
        }


       

        // POST: Database/Edit/5
        [HttpPost]
        public ActionResult ResponseGpMemberChanges(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
                string Response_GroupMember_PUID_Name = collection["Response_GroupMember_PUID_Name"].Trim();
                string Response_GroupMember_Email = collection["Response_GroupMember_Email"].Trim();
                string Response_GroupMember_Name = collection["Response_GroupMember_Name"].Trim();
                string Response_GroupMember_Type = collection["Response_GroupMember_Type"].Trim();

                if (ModelState.IsValid)
                {
                    using (var entitydb = new AtlasDatabaseEntities())
                    {
                        string Primaryflag = "Primary";

                        if (Response_GroupMember_Type != Primaryflag)
                        {
                            entitydb.Database.ExecuteSqlCommand("Update Response_Group_Members SET Response_GroupMember_PUID_Name = '" + Response_GroupMember_PUID_Name + "', Response_GroupMember_Email = '" + Response_GroupMember_Email + "' , Response_GroupMember_Name= '" + Response_GroupMember_Name + "', Response_GroupMember_Type = '" + Response_GroupMember_Type + "' Where  Response_GroupMember_ID = @ResponseGroupMemberID", new SqlParameter("@ResponseGroupMemberID", id));
                            return RedirectToAction("ResponseGpMemberDisplay");
                        }
                        else
                        {
                            using (var conn = new EntityConnection("name=AtlasDatabaseEntities"))
                            {
                                conn.Open();
                                string commandText = @"SELECT Response_Group_Members.Response_GroupMember_PUID_Name, Response_Group_Members.Response_GroupMember_Name FROM AtlasDatabaseEntities.Response_Group_Members where (Response_Group_Members.Response_GroupMember_Name = @ResponseGroupMemberName AND Response_Group_Members.Response_GroupMember_Type = @ResponseGroupMemberType)";
                                using (EntityCommand cmd = new EntityCommand(commandText, conn))
                                {
                                    // Create two parameters and add them to 
                                    // the EntityCommand's Parameters collection 
                                    EntityParameter param1 = new EntityParameter();
                                    param1.ParameterName = "ResponseGroupMemberName";
                                    param1.Value = Response_GroupMember_Name;

                                    EntityParameter param2 = new EntityParameter();
                                    param2.ParameterName = "ResponseGroupMemberType";
                                    param2.Value = Primaryflag;

                                    cmd.Parameters.Add(param1);
                                    cmd.Parameters.Add(param2);
                                    using (DbDataReader rdr = cmd.ExecuteReader(CommandBehavior.SequentialAccess))
                                    {
                                        string memberpuid = "";
                                        string memberName;
                                        // Iterate through the collection of Contact items.
                                        while (rdr.Read())
                                        {
                                            memberpuid = rdr.GetString(0);
                                            memberName = rdr.GetString(1);
                                        }
                                        if (rdr.HasRows)
                                        {
                                            if (Response_GroupMember_PUID_Name == memberpuid)
                                            {
                                                entitydb.Database.ExecuteSqlCommand("Update Response_Group_Members SET Response_GroupMember_PUID_Name = '" + Response_GroupMember_PUID_Name + "', Response_GroupMember_Email = '" + Response_GroupMember_Email + "' , Response_GroupMember_Name= '" + Response_GroupMember_Name + "', Response_GroupMember_Type = '" + Response_GroupMember_Type + "' Where  Response_GroupMember_ID = @ResponseGroupMemberID", new SqlParameter("@ResponseGroupMemberID", id));
                                            }
                                            else
                                            {
                                                TempData["message"] = "Primary Group member already exists";
                                                conn.Close();
                                                return RedirectToAction("ResponseGpMemberChanges", "Home", new { @id = id, @ResponseGpMemberPUIDName = Response_GroupMember_PUID_Name, @ResponseGpMemberEmail = Response_GroupMember_Email, @ResponseGpMemberName = Response_GroupMember_Name, @ResponseGpMemberType = Response_GroupMember_Type });                                             
                                            }
                                        }
                                        else
                                        {
                                            entitydb.Database.ExecuteSqlCommand("Update Response_Group_Members SET Response_GroupMember_PUID_Name = '" + Response_GroupMember_PUID_Name + "', Response_GroupMember_Email = '" + Response_GroupMember_Email + "' , Response_GroupMember_Name= '" + Response_GroupMember_Name + "', Response_GroupMember_Type = '" + Response_GroupMember_Type + "' Where  Response_GroupMember_ID = @ResponseGroupMemberID", new SqlParameter("@ResponseGroupMemberID", id));
                                        }
                                    }
                                }
                                conn.Close();
                            }                           
                        }                    
                    }                
                }
                ModelState.Clear();        
                return RedirectToAction("ResponseGpMemberDisplay");
            }
            
            catch (Exception ex)
            {
                logger.Error(ex.Message + " in Method - ActionResult ResponseGpMemberChanges(int id, FormCollection collection)");
                return View(ex.Message);
            }
        }


        // GET: Database/Delete/5
        public ActionResult ResponseGpMemberDelete(int id)
        {
           try
            {
                using (var entitydb = new AtlasDatabaseEntities())
            {
                entitydb.Database.ExecuteSqlCommand("Delete from Response_Group_Members Where  Response_GroupMember_ID = @GroupMemberID", new SqlParameter("@GroupMemberID", id));
            }
            return RedirectToAction("ResponseGpMemberDisplay");
            //return View();
          }
            catch (Exception ex)
            {
                logger.Error(ex.Message + " in Method - ActionResult ResponseGpMemberDelete(int id)");
                return View(ex.Message);
            }
        }


        // POST: Database/Delete/5
        [HttpPost]
        public ActionResult ResponseGpMemberDelete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here


                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message + " in Method - ActionResult ResponseGpMemberDelete(int id, FormCollection collection)");
                return View(ex.Message);
            }
        }

        [Authorize]
        public ActionResult DDIStatusDisplay()
        {
            try
            {
                using (var entitydb = new AtlasDatabaseEntities())
                {
                    var entitydbMembers = (from statusDetail in entitydb.DDI_Status orderby statusDetail.DDI_Status_ID select statusDetail).ToList();
                    return View(entitydbMembers); 
                }

            }
            catch (Exception ex)
            {
                logger.Error(ex.Message + " in Method - ActionResult DDIStatusDisplay()");
                return View(ex.Message);
            }
        }


      

        // GET: Database/Create
        public ActionResult DDIStatusAdd()
        {
            return View();
        }


        // GET: Database/Delete/5
        public ActionResult DDIStatusDelete(int id)
        {
            try
            {
                using (var entitydb = new AtlasDatabaseEntities())
                {
                    entitydb.Database.ExecuteSqlCommand("Delete from DDI_Status Where  DDI_Status_ID = @StatusID", new SqlParameter("@StatusID", id));
                }
                return RedirectToAction("DDIStatusDisplay");

            }
            catch (Exception ex)
            {
                logger.Error(ex.Message + " in Method - ActionResult  DDIStatusDelete(int id)");
                return View(ex.Message);
            }
        }


        // POST: Database/Create
        [HttpPost]
        public ActionResult DDIStatusAdd(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here
                if (ModelState.IsValid)
                {
                    DDI_Status entitydb = new DDI_Status();                 
                    entitydb.DDI_Status_Desc = collection["DDI_Status_Desc"].Trim();
                   
                    AtlasDatabaseEntities entitydbAll = new AtlasDatabaseEntities();
                    if (entitydbAll.DDI_Status.Any(p => p.DDI_Status_Desc == entitydb.DDI_Status_Desc))
                    {
                        TempData["Message"] = "The DDI Status already exists";
                        return RedirectToAction("DDIStatusAdd");
                    }
                    else
                    {
                        entitydbAll.DDI_Status.Add(entitydb);
                        entitydbAll.SaveChanges();
                    }
                }

                ModelState.Clear();
                return RedirectToAction("DDIStatusDisplay");
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message + " in Method - ActionResult DDIStatusAdd(FormCollection collection)");
                return View(ex.Message);
            }
        }



        // GET: Database/Details/5
        public ActionResult DDIStatusChanges(int id, string StatusDescription)
        {
            try
            {
                ViewBag.ResponseGroup_ID = id;
                ViewBag.DDI_Status_Desc = StatusDescription;                

                return View();
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message + " in Method - ActionResult DDIStatusChange(int id, string StatusDescription)");
                return View(ex.Message);
            }
        }


        // POST: Database/Edit/5
        [HttpPost]
        public ActionResult DDIStatusChanges(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
                string DDI_Status_Desc = collection["DDI_Status_Desc"].Trim();

                using (var entitydb = new AtlasDatabaseEntities())
                {
                    AtlasDatabaseEntities entitydbAll = new AtlasDatabaseEntities();
                    if (entitydbAll.DDI_Status.Any(p => p.DDI_Status_Desc == DDI_Status_Desc))
                    {
                        TempData["Message"] = "The DDI Status already exists";
                        return RedirectToAction("DDIStatusChanges");
                    }
                    else
                    {
                        entitydb.Database.ExecuteSqlCommand("Update DDI_Status  SET DDI_Status_Desc = '" + DDI_Status_Desc + "'   Where  DDI_Status_ID = @StatusID", new SqlParameter("@StatusID", id));
                    }             
                }
                return RedirectToAction("DDIStatusDisplay");

            }
            catch (Exception ex)
            {
                logger.Error(ex.Message + " in Method - public ActionResult DDIStatusChanges(int id, FormCollection collection)");
                return View(ex.Message);
            }
        }



    }

}