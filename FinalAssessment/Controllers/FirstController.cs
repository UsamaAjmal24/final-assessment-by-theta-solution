using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using FinalAssessment.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FinalAssessment.Controllers
{
    public class FirstController : Controller
    {
        private FinalAssessmentContext MyDBContext = null;
        public FirstController(FinalAssessmentContext _MyDBContext)
        {
            MyDBContext = _MyDBContext;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult RegisterUser()
        {
            return View();
        }       //register user create view (get request)
        [HttpPost]
        public IActionResult RegisterUser(Users U)
        {
            try
            {
                MyDBContext.Users.Add(U);
                MyDBContext.SaveChanges();
                ViewBag.Message = "Registration Successful!";
                string SMTPServer = "smtp.gmail.com";
                string SMTPUsernmae = "u.chohan24.ua@gmail.com";
                string SMTPPassword = "chohanthegreat1996";
                int Port = 587;

                MailMessage oMail = new MailMessage
                {
                    From = new MailAddress("u.chohan24.ua@gmail.com")
                };
                oMail.To.Add("u.chohan24.ua@gmail.com");
                //oMail.Attachments.Add(new Attachment("/Images/a.jpg"))

                oMail.Subject = "New User Added";
                oMail.Body = "New User Added please review your project";
                oMail.IsBodyHtml = true;

                SmtpClient oSMTP = new SmtpClient
                {
                    Credentials = new System.Net.NetworkCredential(SMTPUsernmae, SMTPPassword),
                    Host = SMTPServer,
                    Port = Port,
                    EnableSsl = true
                };


                try
                {
                    oSMTP.Send(oMail);
                    ViewBag.Message = "Email Sent Successfully!";
                }
                catch (Exception ex)
                {
                    ViewBag.Message = ex.InnerException + "Error" + ex.Message;
                }
            }
            catch
            {
                ViewBag.Message = "Sorry, unable to register, please try again.";
            }


            //return View();
            return RedirectToAction("ViewRegisterUser");

         
        }       //register user with email sending logic (post request)
        public IActionResult ViewRegisterUser(string SearchParameter)       //users list view with search parameter
        {
            IList<Users> AllUsers = null;

            if (string.IsNullOrEmpty(SearchParameter))
            {
                AllUsers = MyDBContext.Users.ToList<Users>();
            }
            else
            {
                AllUsers = MyDBContext.Users.Where(abc => abc.UserName.Contains(SearchParameter)).ToList<Users>();
            }


            return View(AllUsers);
           // return View();
        }       
        public IActionResult DeleteRegisterUser( Users U)       //delete user
        {
            MyDBContext.Users.Remove(U);
            MyDBContext.SaveChanges();
            return RedirectToAction("ViewRegisterUser");
        }       
        public IActionResult RegisterUserDetail( Users U)
        {
            Users ResultUser = MyDBContext.Users.Where(abc => abc.Id == U.Id).FirstOrDefault<Users>();

            ViewBag.Title = ResultUser.Id + "-" + ResultUser.UserName;
            ViewBag.MetaDescription = "Detail page of Registered User with username " + ResultUser.UserName + " and ID " + ResultUser.Id;


            return View(ResultUser);
        }       //users detail view
        [HttpGet]
        public IActionResult EditUser(int Id)
        {
            Users EUser = MyDBContext.Users.Where(x => x.Id == Id).SingleOrDefault();
            return View(EUser);
        }
        [HttpPost]
        public IActionResult EditUser(Users U)
        {
            Users EUser = MyDBContext.Users.Where(x => x.Id == U.Id).SingleOrDefault();
            if(EUser != null)
            {
                MyDBContext.Entry(EUser).CurrentValues.SetValues(U);
                MyDBContext.SaveChanges();
                return RedirectToAction("ViewRegisterUser");
            }
            return View(U);
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }   //login get request
        [HttpPost]
        public IActionResult Login(Users U)     //session for login post request
        {
            bool user = MyDBContext.Users.Where(A => A.UserName == U.UserName && A.Password == U.Password).Any();
            if(user)
            {
                string SerializedFoundUserInDB = JsonConvert.SerializeObject(user);
                // HttpContext.Session.("LoggedInUser", SerializedFoundUserInDB);


                HttpContext.Session.SetString("LoggedInUser", SerializedFoundUserInDB);
                return RedirectToAction("Dashboard");
            }
            else
            {
                return View();
            }
            
        }
        public bool Authenticate()
        {
            bool Result = false;
            try
            {
               Result = JsonConvert.DeserializeObject<bool>(HttpContext.Session.GetString("LoggedInUser"));                
            }
            catch (Exception)
            {

            }
            return Result;
        }           //Funtion for authentication
        [HttpPost]
        public IActionResult AddNewItem(Items I)
        {
            bool IsAuthenticateUser = Authenticate();
            if (IsAuthenticateUser)
            {
                try
                {
                    MyDBContext.Items.Add(I);
                    MyDBContext.SaveChanges();
                    TempData["Message"] = "Item added successfully";
                }
                catch
                {
                    TempData["Message"] = "Sorry, unable to add, please try again.";
                }

                //return View();
                return RedirectToAction("ViewAllItems");

            }
            return RedirectToAction("Login", "First");


        }       //Add new item create view(post request)
        [HttpGet]
        public IActionResult AddNewItem()               //Add new item (get request)
        {
            
                bool IsAuthenticateUser = Authenticate();
                if (IsAuthenticateUser)
                {
                    ViewBag.Message = TempData["Message"];
                    return View();
                }
                return RedirectToAction("Login", "First");

        }
        public IActionResult ViewAllItems()         //view all items list view
        {
            bool IsAuthenticateUser = Authenticate();
            if (IsAuthenticateUser)
            {
                return View(MyDBContext.Items.ToList<Items>());
            }
            return RedirectToAction("Login", "First");

        }
        public IActionResult DeleteItem(Items I)
        {
            MyDBContext.Items.Remove(I);
            MyDBContext.SaveChanges();
            return RedirectToAction("ViewAllItems");
        }       //delete view for items
        public IActionResult ItemDetail(Items I)
        {
            Items ResultItem = MyDBContext.Items.Where(abc => abc.Id == I.Id).FirstOrDefault<Items>();

            ViewBag.Title = ResultItem.Id + "-" + ResultItem.ItemName;
            ViewBag.MetaDescription = "Detail page of Item with itemname " + ResultItem.ItemName + " and ID " + ResultItem.Id;


            return View(ResultItem);

        }       //detail view for items
        [HttpGet]
        public IActionResult EditItem(int Id)
        {
            Items EItem = MyDBContext.Items.Where(x => x.Id == Id).SingleOrDefault();
            return View(EItem);
        }
        [HttpPost]
        public IActionResult EditItem(Items I)
        {
            Items EItem = MyDBContext.Items.Where(x => x.Id == I.Id).SingleOrDefault();
            if (EItem != null)
            {
                MyDBContext.Entry(EItem).CurrentValues.SetValues(I);
                MyDBContext.SaveChanges();
                return RedirectToAction("ViewAllItems");
            }
            return View(I);
        }

        [HttpGet]
        public IActionResult AddNewCategory()
        {
            bool IsAuthenticateUser = Authenticate();
            if (IsAuthenticateUser)
            {
                ViewBag.Message = TempData["Message"];
                return View();
            }
            return RedirectToAction("Login", "First");
           
        }       //add new category get request
        [HttpPost]
        public IActionResult AddNewCategory( Category C)
        {

            bool IsAuthenticateUser = Authenticate();
            if (IsAuthenticateUser)
            {
                try
                {
                    MyDBContext.Category.Add(C);
                    MyDBContext.SaveChanges();
                    TempData["Message"] = "Category added successfully";
                }
                catch
                {
                    TempData["Message"] = "Sorry, unable to add, please try again.";
                }

                //return View();
                return RedirectToAction("AddNewCategory");
            }
            return RedirectToAction("Login", "First");

            
        }       //add new category post request
        public IActionResult ViewAllCategory()
        {
            bool IsAuthenticateUser = Authenticate();
            if (IsAuthenticateUser)
            {
                return View(MyDBContext.Category.ToList<Category>());
            }
            return RedirectToAction("Login", "First");
            
        }           //list view of category
        //public IActionResult CategoryDetail(Category C)
        //{
        //    Category ResultCategory = MyDBContext.Category.Where(abc => abc.Id == C.Id).FirstOrDefault<Category>();

        //    ViewBag.Title = ResultCategory.Id + "-" + ResultCategory.CategoryName;
        //    ViewBag.MetaDescription = "Detail page of Registered User with categoryname " + ResultCategory.CategoryName + " and ID " + ResultCategory.Id;


        //    return View(ResultCategory);
        //}       //detail view category
        public IActionResult DeleteCategory(Category C)
        {
            MyDBContext.Category.Remove(C);
            MyDBContext.SaveChanges();
            return RedirectToAction("ViewAllCategory");
        }    //delete category
        public IActionResult EditCategory(int Id)
        {
            Category ECategory = MyDBContext.Category.Where(x => x.Id == Id).SingleOrDefault();
            return View(ECategory);
        }
        [HttpPost]
        public IActionResult EditCategory(Category C)
        {
            Category ECategory = MyDBContext.Category.Where(x => x.Id == C.Id).SingleOrDefault();
            if (ECategory != null)
            {
                MyDBContext.Entry(ECategory).CurrentValues.SetValues(C);
                MyDBContext.SaveChanges();
                return RedirectToAction("ViewAllCategory");
            }
            return View(C);
        }

        [HttpPost]
        public IActionResult AddNewCostumer(Customers N)
        {
            bool IsAuthenticateUser = Authenticate();
            if (IsAuthenticateUser)
            {
                try
                {
                    MyDBContext.Customers.Add(N);
                    MyDBContext.SaveChanges();
                    TempData["Message"] = "Customer added successfully";
                }
                catch
                {
                    TempData["Message"] = "Sorry, unable to add, please try again.";
                }

                //return View();
                return RedirectToAction("AddNewCostumer");
            }
            return RedirectToAction("Login", "First");


        }     //add new customer create view post request with session
        [HttpGet]
        public IActionResult AddNewCostumer()
        {
            bool IsAuthenticateUser = Authenticate();
            if (IsAuthenticateUser)
            {
                ViewBag.Message = TempData["Message"];
                return View();
            }
            return RedirectToAction("Login", "First");

        }       //add new customers get request
        [HttpGet]
        public IActionResult ViewAllCustomer()
        {
            bool IsAuthenticateUser = Authenticate();
            if (IsAuthenticateUser)
            {
                return View(MyDBContext.Customers.ToList<Customers>());
            }
            return RedirectToAction("Login", "First");
        }       //list view of customers
        public IActionResult DeleteCustomer(Customers N)
        {
            MyDBContext.Customers.Remove(N);
            MyDBContext.SaveChanges();
            return RedirectToAction("ViewAllCustomer");
        }       //delete customers
        public IActionResult CustomerDetail(Customers N)
        {
            Customers ResultCustomer = MyDBContext.Customers.Where(abc => abc.Id == N.Id).FirstOrDefault<Customers>();

            ViewBag.Title = ResultCustomer.Id + "-" + ResultCustomer.CustomerName;
            ViewBag.MetaDescription = "Detail page of Registered Customer with name " + ResultCustomer.CustomerName + " and ID " + ResultCustomer.Id;


            return View(ResultCustomer);
        }       //detail view customer
        [HttpPost]
        public IActionResult AddNewSale( Sales S)
        {
            bool IsAuthenticateUser = Authenticate();
            if (IsAuthenticateUser)
            {
                try
                {
                    MyDBContext.Sales.Add(S);
                    MyDBContext.SaveChanges();
                    TempData["Message"] = "Added successfully";
                }
                catch
                {
                    TempData["Message"] = "Sorry, unable to add, please try again.";
                }

                //return View();
                return RedirectToAction("AddNewSale");
            }
            return RedirectToAction("Login", "First");
            
        }       //add new sale create view post request
        [HttpGet]
        public IActionResult AddNewSale()       //add new sale get request
        {
            bool IsAuthenticateUser = Authenticate();
            if (IsAuthenticateUser)
            {
                ViewBag.Message = TempData["Message"];
                return View();
            }
            return RedirectToAction("Login", "First");
        }           
        public IActionResult ViewAllSale()
        {
            bool IsAuthenticateUser = Authenticate();
            if (IsAuthenticateUser)
            {
                return View(MyDBContext.Sales.ToList<Sales>());
            }
            return RedirectToAction("Login", "First");
        }       //sale list view 
        [HttpPost]
        public IActionResult AddNewPurchase( Purchase P)
        {

            bool IsAuthenticateUser = Authenticate();
            if (IsAuthenticateUser)
            {
                try
                {
                    MyDBContext.Purchase.Add(P);
                    MyDBContext.SaveChanges();
                    TempData["Message"] = "Added successfully";
                }
                catch
                {
                    TempData["Message"] = "Sorry, unable to add, please try again.";
                }

                //return View();
                return RedirectToAction("AddNewPurchase");
            }
            return RedirectToAction("Login", "First");


        }               //add new purchase create view post request
        [HttpGet]
        public IActionResult AddNewPurchase()
        {
            bool IsAuthenticateUser = Authenticate();
            if (IsAuthenticateUser)
            {
                ViewBag.Message = TempData["Message"];
                return View();
            }
            return RedirectToAction("Login", "First");


        }       //add new purchase get request
        public IActionResult ViewAllPurchase()
        {
            bool IsAuthenticateUser = Authenticate();
            if (IsAuthenticateUser)
            {
                return View(MyDBContext.Purchase.ToList<Purchase>());
            }
            return RedirectToAction("Login", "First");
        }          //purchase list view
        [HttpPost]
        public IActionResult AddNewVendor( Vendor V)
        {
            bool IsAuthenticateUser = Authenticate();
            if (IsAuthenticateUser)
            {
                try
                {
                    MyDBContext.Vendor.Add(V);
                    MyDBContext.SaveChanges();
                    TempData["Message"] = "Added successfully";
                }
                catch
                {
                    TempData["Message"] = "Sorry, unable to add, please try again.";
                }

                //return View();
                return RedirectToAction("AddNewPurchase");
            }
            return RedirectToAction("Login", "First");
                      
        }       //add new vendor create view post request
        [HttpGet]
        public IActionResult AddNewVendor()
        {
            bool IsAuthenticateUser = Authenticate();
            if (IsAuthenticateUser)
            {
                ViewBag.Message = TempData["Message"];
                return View();
            }
            return RedirectToAction("Login", "First");
        }       //add new vendor get request
        public IActionResult ViewAllVendor()
        {
            bool IsAuthenticateUser = Authenticate();
            if (IsAuthenticateUser)
            {
                return View(MyDBContext.Vendor.ToList<Vendor>());
            }
            return RedirectToAction("Login", "First");
        }       //vendor list view
        public IActionResult Dashboard()
        {
            return View();
        }       //list view action
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction(nameof(FirstController.Login));
        }       //log out session


        public int AllItemsCount()
        {
            int ItemResult = 0;

            ItemResult =  MyDBContext.Items.ToList().Count();

            return ItemResult;
        }
        public int AllCategoriesCount()
        {
            int CategoryResult = 0;
            CategoryResult = MyDBContext.Category.ToList().Count();
            return CategoryResult;
        }
        public int CustomerCount()
        {
            int CustomerResult = 0;

            CustomerResult = MyDBContext.Customers.ToList().Count();

            return CustomerResult;
        }
        public int UserCount()
        {
            int UserResult = 0;

            UserResult = MyDBContext.Users.ToList().Count();

            return UserResult;
        }

        //public bool DuplicateCheck()
        //{
        //    bool result = false;
        //    result = MyDBContext.Category.
        //}
    }
}