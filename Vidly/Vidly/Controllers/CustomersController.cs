using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vidly.Models;
using System.Data.Entity;
using Vidly.ViewModels;

namespace Vidly.Controllers
{
    public class CustomersController : Controller
    {
        // To access data
        private CustomerContext _context;

        public CustomersController()
        {
            _context = new CustomerContext();
        }

        //Dispose

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        // GET: Customers
        public ActionResult GetCustomerList()
        {
            //var customer = _context.Customers.Include(c=>c.MembershipType).ToList();
            //return View(customer);
            return View();
        }

        public ActionResult GetCustomerDetails(int id)
        {
            var name = _context.Customers.Include(c => c.MembershipType).SingleOrDefault(c => c.Id == id);
            if (name != null)
            {
                var customer = new Customer
                {

                    Name = name.Name,
                    MembershipType=name.MembershipType,
                    Birthdate=name.Birthdate
                   
                };
                return View(customer);
            }
            else
            {
                return HttpNotFound();
            }
            
        }

        //private IEnumerable<Customer> GetCustomers()
        //{
        //    return new List<Customer>
        //    {
        //        new Customer { Id = 1, Name = "John Smith" },
        //        new Customer { Id = 2, Name = "Mary Williams" }
        //    };
        //}

        public ActionResult NewCustomer()
        {
            var membershipTypes = _context.MembershipTypes.ToList();
            var viewModel = new NewCustomerViewModel
            {
                MembershipTypes = membershipTypes
            };
            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(Customer customer)
        {
            if (!ModelState.IsValid)
            {
                var viewModel = new NewCustomerViewModel
                {
                    Customer = customer,
                    MembershipTypes = _context.MembershipTypes.ToList()
                };
                return View("NewCustomer", viewModel);
            }
            if(customer.Id==0)
            _context.Customers.Add(customer);
            else
            {
                var customerInDb = _context.Customers.Single(c => c.Id == customer.Id);
                customerInDb.Name = customer.Name;
                customerInDb.Birthdate = customer.Birthdate;
                customerInDb.IsSubscribedToNewsLetter = customer.IsSubscribedToNewsLetter;
                customerInDb.MembershipTypeId = customer.MembershipTypeId;

            }
            _context.SaveChanges();
            return RedirectToAction("GetCustomerList","Customers");
        }

        public ActionResult EditCustomerDetails(int id)
        {
            var customer = _context.Customers.SingleOrDefault(c => c.Id == id);
            if (customer == null)
            {
                return HttpNotFound();
            }
                var customerViewModel = new NewCustomerViewModel
                {
                    Customer = customer,
                    MembershipTypes = _context.MembershipTypes.ToList()
                };
            return View("NewCustomer", customerViewModel);
        }

    }

}