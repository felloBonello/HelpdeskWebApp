///<summary>
/// @purpose: Holds information about each employee and allows weblayer to communicate with the database
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
    public class EmployeeViewModel : ViewModelUtils
    {
        private EmployeeDAO _dao;
        public string Title { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string Phoneno { get; set; }
        public string Entity64 { get; set; }
        public string Id { get; set; }
        public string DepartmentId { get; set; }
        public string StaffPicture64 { get; set; }
        public bool IsTech { get; set; }

        /// <summary>
        /// constructor
        /// </summary>
        public EmployeeViewModel()
        {
            _dao = new EmployeeDAO();
        }

        //
        //find employee with surnme from dal
        //
        public void GetByID(string id)
        {
            try
            {
                Employee emp = _dao.GetByID(id);
                Id = emp._id.ToString();
                Title = emp.Title;
                Firstname = emp.Firstname;
                Lastname = emp.Lastname;
                Phoneno = emp.Phoneno;
                Email = emp.Email;
                DepartmentId = emp.DepartmentId.ToString();
                StaffPicture64 = emp.StaffPicture64;
                IsTech = emp.IsTech;
                Entity64 = Convert.ToBase64String(Serializer(emp));
            }
            catch (Exception ex)
            {
                ErrorRoutine(ex, "EmployeeViewModel", "GetByID");
            }
        }

        public int Update()
        {
            int empsUpdated = -1;

            try
            {
                byte[] bytEmp = Convert.FromBase64String(Entity64);
                Employee emp = (Employee)Deserializer(bytEmp);
                emp.Title = Title;
                emp.Firstname = Firstname;
                emp.Lastname = Lastname;
                emp.Phoneno = Phoneno;
                emp.Email = Email;
                emp.DepartmentId = new ObjectId(DepartmentId);
                emp.StaffPicture64 = StaffPicture64;
                emp.IsTech = IsTech;
                empsUpdated = _dao.Update(emp);
            }
            catch (Exception ex)
            {
                ErrorRoutine(ex, "EmployeeViewModel", "Update");
            }
            return empsUpdated;
        }

        public void Create()
        {
            try
            {
                Employee emp = new Employee();
                emp.DepartmentId = new ObjectId(DepartmentId);
                emp.Title = Title;
                emp.Firstname = Firstname;
                emp.Lastname = Lastname;
                emp.Phoneno = Phoneno;
                emp.Email = Email;
                emp.StaffPicture64 = StaffPicture64;
                Id = _dao.Create(emp);
            }
            catch(Exception ex)
            {
                ErrorRoutine(ex, "EmployeeViewModel", "Create");
            }
        }

        public List<EmployeeViewModel> GetAll()
        {
            List<EmployeeViewModel> viewModels = new List<EmployeeViewModel>();

            try
            {
                List<Employee> employees = _dao.GetAll();

                foreach (Employee e in employees)
                {
                    //return only fields for display, subsequent get wil other fields
                    EmployeeViewModel viewModel = new EmployeeViewModel();
                    viewModel.Id = e._id.ToString();
                    viewModel.Title = e.Title;
                    viewModel.Firstname = e.Firstname;
                    viewModel.Lastname = e.Lastname;
                    viewModel.StaffPicture64 = e.StaffPicture64;
                    viewModels.Add(viewModel); //add to list
                }
            }
            catch (Exception ex)
            {
                ErrorRoutine(ex, "EmployeeViewModel", "GetAll");
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
                ErrorRoutine(ex, "EmployeeViewModel", "Delete");
            }

            return deleteOk;
        }
    }
}