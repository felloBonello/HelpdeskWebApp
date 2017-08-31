///<summary>
/// @purpose: Holds information about each Problem and allows weblayer to communicate with the datalayer
///                         adds functions that allow for CRUD
/// @date: 2015-10-28
/// @author: Justin Bonello
/// @revisions: 4
///</summary>


using System;
using HelpdeskDAL;
using MongoDB.Bson;
using System.Collections.Generic;

namespace HelpdeskViewModels
{
    public class ProblemViewModel : ViewModelUtils
    {
        private ProblemDAO _dao;
        
        public string Entity64 { get; set; }
        public string Id { get; set; }
        public string Description { get; set; }

        /// <summary>
        /// constructor
        /// </summary>
        public ProblemViewModel()
        {
            _dao = new ProblemDAO();
        }

        //
        //find Problem with surnme from dal
        //
        public void GetByID(string id)
        {
            try
            {
                Problem prob = _dao.GetByID(id);
                Id = prob._id.ToString();
                Description = prob.Description;
                Entity64 = Convert.ToBase64String(Serializer(prob));
            }
            catch (Exception ex)
            {
                ErrorRoutine(ex, "ProblemViewModel", "GetByID");
            }
        }

        public int Update()
        {
            int probsUpdated = -1;

            try
            {
                byte[] bytProb = Convert.FromBase64String(Entity64);
                Problem prob = (Problem)Deserializer(bytProb);
                prob.Description = Description;
                probsUpdated = _dao.Update(prob);
            }
            catch (Exception ex)
            {
                ErrorRoutine(ex, "ProblemViewModel", "Update");
            }
            return probsUpdated;
        }

        public void Create()
        {
            try
            {
                Problem prob = new Problem();
                prob.Description = Description;
                Id = _dao.Create(prob);
            }
            catch (Exception ex)
            {
                ErrorRoutine(ex, "ProblemViewModel", "Create");
            }
        }

        public List<ProblemViewModel> GetAll()
        {
            List<ProblemViewModel> viewModels = new List<ProblemViewModel>();

            try
            {
                List<Problem> Problems = _dao.GetAll();

                foreach (Problem e in Problems)
                {
                    //return only fields for display, subsequent get wil other fields
                    ProblemViewModel viewModel = new ProblemViewModel();
                    viewModel.Id = e._id.ToString();
                    viewModel.Description = e.Description;
                    viewModels.Add(viewModel); //add to list
                }
            }
            catch (Exception ex)
            {
                ErrorRoutine(ex, "ProblemViewModel", "GetAll");
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
                ErrorRoutine(ex, "ProblemViewModel", "Delete");
            }

            return deleteOk;
        }
    }
}