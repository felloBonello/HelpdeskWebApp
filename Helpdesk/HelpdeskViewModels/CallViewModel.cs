///<summary>
/// @purpose: Holds information about each call and allows weblayer to communicate with the database
///                         adds functions that allow for CRUD
/// @date: 2015-12-04
/// @author: Justin Bonello
/// @revisions: 4
///</summary>

using System;
using HelpdeskDAL;
using MongoDB.Bson;
using System.Collections.Generic;

namespace HelpdeskViewModels
{
    public class CallViewModel : ViewModelUtils
    {
        private CallDAO _dao;
        public string Id { get; set; }
        public string EmployeeId { get; set; }
        public string ProblemId { get; set; }
        public string TechId { get; set; }
        public System.DateTime DateOpened { get; set; }
        public System.DateTime? DateClosed { get; set; }
        public bool? OpenStatus { get; set; }
        public string Notes { get; set; }
        public string Entity64 { get; set; }

        /// <summary>
        /// constructor
        /// </summary>
        public CallViewModel()
        {
            _dao = new CallDAO();
        }

        //
        //find employee with surnme from dal
        //
        public void GetByID(string id)
        {
            try
            {
                Call call = _dao.GetByID(id);
                Id = call._id.ToString();
                EmployeeId = call.EmployeeId.ToString();
                ProblemId = call.ProblemId.ToString();
                TechId = call.TechId.ToString();
                DateOpened = call.DateOpened;
                DateClosed = call.DateClosed;
                OpenStatus = call.OpenStatus;
                Notes = call.Notes;

                Entity64 = Convert.ToBase64String(Serializer(call));
            }
            catch (Exception ex)
            {
                ErrorRoutine(ex, "EmployeeViewModel", "GetByID");
            }
        }

        public int Update()
        {
            int callsUpdated = -1;

            try
            {
                byte[] bytCall = Convert.FromBase64String(Entity64);
                Call call = (Call)Deserializer(bytCall);

                call.EmployeeId = new ObjectId(EmployeeId);
                call.ProblemId = new ObjectId(ProblemId);
                call.TechId = new ObjectId(TechId);
                call.DateOpened = DateOpened;
                call.DateClosed = DateClosed;
                call.OpenStatus = OpenStatus;
                call.Notes = Notes;

                callsUpdated = _dao.Update(call);
            }
            catch (Exception ex)
            {
                ErrorRoutine(ex, "CallViewModel", "Update");
            }
            return callsUpdated;
        }

        public void Create()
        {
            try
            {
                Call call = new Call();
                call.EmployeeId = new ObjectId(EmployeeId);
                call.ProblemId = new ObjectId(ProblemId);
                call.TechId = new ObjectId(TechId);
                call.DateOpened = DateOpened;
                call.DateClosed = DateClosed;
                call.OpenStatus = OpenStatus;
                call.Notes = Notes;

                Id = _dao.Create(call);
            }
            catch (Exception ex)
            {
                ErrorRoutine(ex, "CallViewModel", "Create");
            }
        }

        public List<CallViewModel> GetAll()
        {
            List<CallViewModel> viewModels = new List<CallViewModel>();

            try
            {
                List<Call> calls = _dao.GetAll();

                foreach (Call c in calls)
                {
                    //return only fields for display, subsequent get wil other fields
                    CallViewModel viewModel = new CallViewModel();
                    viewModel.Id = c._id.ToString();
                    viewModel.DateOpened = c.DateOpened;
                    viewModel.EmployeeId = c.EmployeeId.ToString();
                    viewModel.ProblemId = c.ProblemId.ToString();

                    viewModels.Add(viewModel); //add to list
                }
            }
            catch (Exception ex)
            {
                ErrorRoutine(ex, "CallViewModel", "GetAll");
            }
            return viewModels;
        }

        public bool Delete()
        {
            bool deleteOk = false;

            try
            {
                deleteOk = _dao.Delete(Id);
            }
            catch (Exception ex)
            {
                ErrorRoutine(ex, "CallViewModel", "Delete");
            }

            return deleteOk;
        }
    }
}