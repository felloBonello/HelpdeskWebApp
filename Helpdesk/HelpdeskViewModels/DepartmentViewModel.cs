using System;
using HelpdeskDAL;
using MongoDB.Bson;
using System.Collections.Generic;

namespace HelpdeskViewModels
{
    public class DepartmentViewModel : ViewModelUtils
    {
        private DepartmentDAO _dao;
        public string Id { get; set; }
        public string DepartmentName { get; set; }
        public string Entity64 { get; set; }

        /// <summary>
        /// constructor
        /// </summary>
        public DepartmentViewModel()
        {
            _dao = new DepartmentDAO();
        }

        //
        //find Department with surnme from dal
        //
        public void GetByID(string id)
        {
            try
            {
                Department dep = _dao.GetByID(id);
                Id = dep._id.ToString();
                DepartmentName = dep.DepartmentName;
                Entity64 = Convert.ToBase64String(Serializer(dep));
            }
            catch (Exception ex)
            {
                ErrorRoutine(ex, "DepartmentViewModel", "GetByID");
            }
        }

        public int Update()
        {
            int updateOk = -1;

            try
            {
                byte[] bytDep = Convert.FromBase64String(Entity64);
                Department dep = (Department)Deserializer(bytDep);
                dep.DepartmentName = DepartmentName;
                updateOk = _dao.Update(dep);
            }
            catch (Exception ex)
            {
                ErrorRoutine(ex, "DepartmentViewModel", "Update");
            }
            return updateOk;
        }

        public void Create()
        {
            try
            {
                Department dep = new Department();
                dep.DepartmentName = DepartmentName;
                Id = _dao.Create(dep);
            }
            catch (Exception ex)
            {
                ErrorRoutine(ex, "DepartmentViewModel", "Create");
            }
        }

        public List<DepartmentViewModel> GetAll()
        {
            List<DepartmentViewModel> viewModels = new List<DepartmentViewModel>();

            try
            {
                List<Department> departments = _dao.GetAll();

                foreach (Department e in departments)
                {
                    //return only fields for display, subsequent get wil other fields
                    DepartmentViewModel viewModel = new DepartmentViewModel();
                    viewModel.Id = e._id.ToString();
                    viewModel.DepartmentName = e.DepartmentName;
                    viewModels.Add(viewModel); //add to list
                }
            }
            catch (Exception ex)
            {
                ErrorRoutine(ex, "DepartmentViewModel", "GetAll");
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
                ErrorRoutine(ex, "DepartmentViewModel", "Delete");
            }

            return deleteOk;
        }
    }
}