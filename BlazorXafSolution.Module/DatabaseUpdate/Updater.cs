using System;
using System.Data.SqlClient;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;

using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Updating;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using BlazorXafSolution.Module.BusinessObjects;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using System.Drawing;
using DevExpress.ExpressApp.Utils;
using System.Data;
using System.IO;
using System.Collections.Generic;

namespace BlazorXafSolution.Module.DatabaseUpdate {
    public class Updater : ModuleUpdater {
        private static List<string> tasks = new List<string>() {
            "Set-up home theater (surround sound) system",
            "Install 3 overhead lights in bedroom",
            "Change light bulbs in backyard",
            "Install a programmable thermostat",
            "Install LED lights in kitchen",
            "Check wiring in main electricity panel",
            "Replace master bedroom light switch with dimmer",
            "Install an new electric outlet in garage",
            "Install electric outlet and ethernet drop in closet",
            "Install chandelier in dining room",
            "Hook up DVD Player to TV for kids",
            "Clean the House top to bottom",
            "Light cleaning of the house",
            "Clean the entire house",
            "Clean and organize basement",
            "Pick up clothes for charity event",
            "Ironing, laundry and vacuuming",
            "Take kids to park and play baseball on Sunday",
            "Clean art studio",
            "Bake brownies and send them to neighbors",
            "Assemble Kitchen Cart",
            "Move piano",
            "Clean backyard",
            "Clean out garage",
            "Organize guest bedroom",
            "Clean out closet",
            "Preapre for yard sale",
            "Sorting clothing for give-away",
            "Organize Storage Room",
            "Return coffee machine",
            "Purchase plastic trash bins",
            "Shop for dinner ingredients at the store",
            "Buy new utensils for kitchen",
            "Send post card to Timothy",
            "Buy dining table and TV stand online",
            "Buy ingredients for Pasta Bolognese",
            "Size 3 diapers (3 cases)",
            "Order 3 pizzas",
            "Find out where to buy the new tablet",
            "Buy broccoli and tomatoes",
            "Buy bottle of Champagne",
            "Grocery shopping at Market Basket",
            "Find a bike at a store close to me",
            "Return jeans at JCrew",
            "Buy dog food for Fido",
            "Buy rigid foam insulation",
            "Purchase 3 24-packs of bottled Coke",
            "Purchase & deliver flowers to my home",
            "Input bank statement transactions into Excel spreadsheet",
            "Schedule appointments and pay bills",
            "Place new address stickers on envelopes",
            "Set up and arrange appointments",
            "Copy PDF file into Word",
            "Organize business expenses (last 6 months)",
            "Return samples to vendor",
            "Organize receipts and match them up with business expenses and trips",
            "File papers and receipts",
            "Ship out CDs to customers",
            "Respond to e-mails until noon",
            "Enter expenses into an online accounting system",
            "Conduct inventory of all furniture in office",
            "Arrange travel to conference",
            "Staple flyers to gift bags",
            "File and shred mail",
            "Print copies of brochures",
            "Enter all receipts into an Excel spreadsheet",
            "Research possible vendors",
            "Sort through paper receipts",
            "Re-package products for retail sale",
            "Scan docs, and put in desktop folder",
            "Print registration stickers"
        };

        private IList<DemoTask> GenerateTask(Contact[] contacts) {
            Random rndGenerator = new Random();
            List<DemoTask> taskList = new List<DemoTask>();
            foreach(string taskSubject in tasks) {
                if(ObjectSpace.FindObject<DemoTask>(CriteriaOperator.Parse("Subject == '" + taskSubject + "'")) == null) {
                    DemoTask task = ObjectSpace.CreateObject<DemoTask>();
                    task.Subject = taskSubject;
                    int rndStatus = rndGenerator.Next(0, 5);
                    task.Status = (DevExpress.Persistent.Base.General.TaskStatus)rndStatus;
                    task.DueDate = DateTime.Now.AddHours((90 - rndStatus * 9) + 24).Date;
                    task.EstimatedWorkHours = rndGenerator.Next(10, 20);
                    if(task.Status == DevExpress.Persistent.Base.General.TaskStatus.WaitingForSomeoneElse ||
                       task.Status == DevExpress.Persistent.Base.General.TaskStatus.Completed ||
                       task.Status == DevExpress.Persistent.Base.General.TaskStatus.InProgress) {
                        task.StartDate = DateTime.Now.AddHours(-rndGenerator.Next(720)).Date;
                        task.ActualWorkHours = rndGenerator.Next(task.EstimatedWorkHours - 10, task.EstimatedWorkHours + 10);
                    }
                    task.DueDate = DateTime.Now.AddHours((90 - rndStatus * 9) + 24).Date;
                    task.AssignedTo = contacts[rndGenerator.Next(0, 4)];
                    task.Priority = (Priority)rndGenerator.Next(3);
                    taskList.Add(task);
                }
            }
            return taskList;
        }

        public Updater(IObjectSpace objectSpace, Version currentDBVersion) : base(objectSpace, currentDBVersion) { }
        public override void UpdateDatabaseAfterUpdateSchema() {
            base.UpdateDatabaseAfterUpdateSchema();

            UpdateStatus("CreateContacts", "", "Creating contacts, departments and positions in the database...");
            Position developerPosition = ObjectSpace.FindObject<Position>(CriteriaOperator.Parse("Title == 'Developer'"));
            if(developerPosition == null) {
                developerPosition = ObjectSpace.CreateObject<Position>();
                developerPosition.Title = "Developer";
            }
            Position managerPosition = ObjectSpace.FindObject<Position>(CriteriaOperator.Parse("Title == 'Manager'"));
            if(managerPosition == null) {
                managerPosition = ObjectSpace.CreateObject<Position>();
                managerPosition.Title = "Manager";
            }

            Department devDepartment = ObjectSpace.FindObject<Department>(CriteriaOperator.Parse("Title == 'Development Department'"));
            if(devDepartment == null) {
                devDepartment = ObjectSpace.CreateObject<Department>();
                devDepartment.Title = "Development Department";
                devDepartment.Office = "205";
                devDepartment.Positions.Add(developerPosition);
                devDepartment.Positions.Add(managerPosition);
            }
            Department seoDepartment = ObjectSpace.FindObject<Department>(CriteriaOperator.Parse("Title == 'SEO'"));
            if(seoDepartment == null) {
                seoDepartment = ObjectSpace.CreateObject<Department>();
                seoDepartment.Title = "SEO";
                seoDepartment.Office = "703";
                seoDepartment.Positions.Add(developerPosition);
                seoDepartment.Positions.Add(managerPosition);
            }
            ObjectSpace.CommitChanges();

            try {
                DataTable employeesTable = GetEmployeesDataTable();
                foreach(DataRow employee in employeesTable.Rows) {
                    string email = Convert.ToString(employee["EmailAddress"]);
                    Contact contact = ObjectSpace.FindObject<Contact>(CriteriaOperator.Parse("Email=?", email));
                    if(contact == null) {
                        contact = ObjectSpace.CreateObject<Contact>();
                        contact.Email = email;
                        contact.FirstName = Convert.ToString(employee["FirstName"]);
                        contact.LastName = Convert.ToString(employee["LastName"]);
                        contact.BirthDate = Convert.ToDateTime(employee["BirthDate"]);
                        contact.Photo = Convert.FromBase64String(Convert.ToString(employee["ImageData"]));
                        string titleOfCourtesyText = Convert.ToString(employee["Title"]).ToLower();
                        if(!string.IsNullOrEmpty(titleOfCourtesyText)) {
                            titleOfCourtesyText = titleOfCourtesyText.Replace(".", "");
                            TitleOfCourtesy titleOfCourtesy;
                            if(Enum.TryParse<TitleOfCourtesy>(titleOfCourtesyText, true, out titleOfCourtesy)) {
                                contact.TitleOfCourtesy = titleOfCourtesy;
                            }
                        }
                        PhoneNumber phoneNumber = ObjectSpace.CreateObject<PhoneNumber>();
                        phoneNumber.Party = contact;
                        phoneNumber.Number = Convert.ToString(employee["Phone"]);
                        phoneNumber.PhoneType = "Work";

                        Address address = ObjectSpace.CreateObject<Address>();
                        contact.Address1 = address;
                        address.ZipPostal = Convert.ToString(employee["PostalCode"]);
                        address.Street = Convert.ToString(employee["AddressLine1"]);
                        address.City = Convert.ToString(employee["City"]);
                        address.StateProvince = Convert.ToString(employee["StateProvinceName"]);
                        string countryName = Convert.ToString(employee["CountryRegionName"]);
                        Country country = ObjectSpace.FindObject<Country>(CriteriaOperator.Parse("Name=?", countryName), true);
                        if(country == null) {
                            country = ObjectSpace.CreateObject<Country>();
                            country.Name = countryName;
                        }
                        address.Country = country;

                        string departmentTitle = Convert.ToString(employee["GroupName"]);
                        Department department = ObjectSpace.FindObject<Department>(CriteriaOperator.Parse("Title=?", departmentTitle), true);
                        if(department == null) {
                            department = ObjectSpace.CreateObject<Department>();
                            department.Title = departmentTitle;
                            Random rnd = new Random();
                            department.Office = string.Format("{0}0{0}", rnd.Next(1, 7), rnd.Next(9));
                        }
                        contact.Department = department;

                        string positionTitle = Convert.ToString(employee["JobTitle"]);
                        Position position = ObjectSpace.FindObject<Position>(CriteriaOperator.Parse("Title=?", positionTitle), true);
                        if(position == null) {
                            position = ObjectSpace.CreateObject<Position>();
                            position.Title = positionTitle;
                            position.Departments.Add(department);
                        }
                        contact.Position = position;
                    }
                }
                ObjectSpace.CommitChanges();
            }
            catch(Exception e) {
                Tracing.Tracer.LogText("Cannot initialize contacts, departments and positions from the XML file.");
                Tracing.Tracer.LogError(e);
            }
            Contact contactMary = ObjectSpace.FindObject<Contact>(CriteriaOperator.Parse("FirstName == 'Mary' && LastName == 'Tellitson'"));
            Contact contactJohn = ObjectSpace.FindObject<Contact>(CriteriaOperator.Parse("FirstName == 'John' && LastName == 'Nilsen'"));
            Contact contactJanete = ObjectSpace.FindObject<Contact>(CriteriaOperator.Parse("FirstName == 'Janete' && LastName == 'Limeira'"));
            Contact contactKarl = ObjectSpace.FindObject<Contact>(CriteriaOperator.Parse("FirstName == 'Karl' && LastName == 'Jablonski'"));


            ObjectSpace.CommitChanges();


            UpdateStatus("CreatePayments", "", "Creating payments, resumes and scheduler events in the database...");
            IList<Contact> topTenContacts = ObjectSpace.GetObjects<Contact>();
            ObjectSpace.SetCollectionSorting(topTenContacts, new SortProperty[] { new SortProperty("LastName", DevExpress.Xpo.DB.SortingDirection.Ascending) });
            ObjectSpace.SetTopReturnedObjectsCount(topTenContacts, 10);
            string[] notes = {
                "works with customers until their problems are resolved and often goes an extra step to help upset customers be completely surprised by how far we will go to satisfy customers",
                "is very good at making team members feel included. The inclusion has improved the team's productivity dramatically",
                "is very good at sharing knowledge and information during a problem to increase the chance it will be resolved quickly",
                "actively elicits feedback from customers and works to resolve their problems",
                "creates an inclusive work environment where everyone feels they are a part of the team",
                "consistently keeps up on new trends in the industry and applies these new practices to every day work",
                "is clearly not a short term thinker - the ability to set short and long term business goals is a great asset to the company",
                "seems to want to achieve all of the goals in the last few weeks before annual performance review time, but does not consistently work towards the goals throughout the year",
                "does not yet delegate effectively and has a tendency to be overloaded with tasks which should be handed off to subordinates",
                "to be discussed with the top management..."
            };
            for(int i = 0; i < topTenContacts.Count; i++) {
                Contact contact = topTenContacts[i];
                Payment payment = ObjectSpace.FindObject<Payment>(CriteriaOperator.Parse("Contact=?", contact));
                if(payment == null) {
                    payment = ObjectSpace.CreateObject<Payment>();
                    payment.Contact = contact;
                    payment.Hours = new Random().Next(10, 40);
                    payment.Rate = new Random().Next(30, 50) + new Random().Next(5, 20);
                }
                Resume resume = ObjectSpace.FindObject<Resume>(CriteriaOperator.Parse("Contact=?", contact));
                if(resume == null) {
                    resume = ObjectSpace.CreateObject<Resume>();
                    FileData file = ObjectSpace.CreateObject<FileData>();
                    try {
                        file.LoadFromStream(string.Format("{0}_Photo.png", contact.FullName), new MemoryStream(contact.Photo));
                    }
                    catch(Exception e) {
                        Tracing.Tracer.LogText("Cannot initialize FileData for the contact {0}.", contact.FullName);
                        Tracing.Tracer.LogError(e);
                    }
                    resume.File = file;
                    resume.Contact = contact;
                }
                Contact reviewerContact = i < 5 ? contactMary : contactJanete;
                Note note = ObjectSpace.FindObject<Note>(CriteriaOperator.Parse("Contains(Text, ?)", contact.FullName));
                if(note == null) {
                    note = ObjectSpace.CreateObject<Note>();
                    note.Author = reviewerContact.FullName;
                    note.Text = string.Format("<span style='color:#000000;font-family:Tahoma;font-size:8pt;'><b>{0}</b> \r\n{1}</span>", contact.FullName, notes[i]);
                    note.DateTime = DateTime.Now.AddDays(i * (-1));
                }
                Event appointment = ObjectSpace.FindObject<Event>(CriteriaOperator.Parse("Contains(Subject, ?)", contact.FullName));
                if(appointment == null) {
                    appointment = ObjectSpace.CreateObject<Event>();
                    appointment.Subject = string.Format("{0} - performance review", contact.FullName);
                    appointment.Description = string.Format("{0} \r\n{1}", contact.FullName, notes[i]);
                    appointment.StartOn = note.DateTime.AddDays(5).AddHours(12);
                    appointment.EndOn = appointment.StartOn.AddHours(2);
                    appointment.Location = "101";
                    appointment.AllDay = false;
                    appointment.Status = 0;
                    appointment.Label = i % 2 == 0 ? 2 : 5;
                    Resource reviewerContactResource = ObjectSpace.FindObject<Resource>(CriteriaOperator.Parse("Contains(Caption, ?)", reviewerContact.FullName));
                    if(reviewerContactResource == null) {
                        reviewerContactResource = ObjectSpace.CreateObject<Resource>();
                        reviewerContactResource.Caption = reviewerContact.FullName;
                        reviewerContactResource.Color = reviewerContact == contactMary ? Color.AliceBlue : Color.LightCoral;
                    }
                    appointment.Resources.Add(reviewerContactResource);
                }
            }

            ObjectSpace.CommitChanges();
            UpdateStatus("CreateTasks", "", "Creating demo tasks in the database...");
            IList<DemoTask> taskList = GenerateTask(new Contact[] { contactMary, contactJohn, contactJanete, contactKarl });
            if(taskList.Count > 0) {
                IList<Contact> contacts = ObjectSpace.GetObjects<Contact>();
                Random rndGenerator = new Random();
                foreach(Contact contact in contacts) {
                    if(taskList.Count == 1) {
                        contact.Tasks.Add(taskList[0]);
                    }
                    else if(taskList.Count == 2) {
                        contact.Tasks.Add(taskList[0]);
                        contact.Tasks.Add(taskList[1]);
                    }
                    else {
                        int index = rndGenerator.Next(1, taskList.Count - 2);
                        contact.Tasks.Add(taskList[index]);
                        contact.Tasks.Add(taskList[index - 1]);
                        contact.Tasks.Add(taskList[index + 1]);
                    }
                }
            }
            UpdateStatus("CreateAnalysis", "", "Creating analysis reports in the database...");
            CreateDataToBeAnalysed();
            UpdateStatus("CreateSecurityData", "", "Creating users and roles in the database...");


            ObjectSpace.CommitChanges();
        }
        private DataTable GetEmployeesDataTable() {
            string shortName = "EmployeesWithPhoto.xml";
            string embeddedResourceName = Array.Find<string>(this.GetType().Assembly.GetManifestResourceNames(), (s) => { return s.Contains(shortName); });
            Stream stream = this.GetType().Assembly.GetManifestResourceStream(embeddedResourceName);
            if(stream == null) {
                throw new Exception(string.Format("Cannot read employees data from the {0} file!", shortName));
            }
            DataSet ds = new DataSet();
            ds.ReadXml(stream);
            return ds.Tables["Employee"];
        }
        private void CreateDataToBeAnalysed() {
            Analysis taskAnalysis1 = ObjectSpace.FindObject<Analysis>(CriteriaOperator.Parse("Name='Completed tasks'"));
            if(taskAnalysis1 == null) {
                taskAnalysis1 = ObjectSpace.CreateObject<Analysis>();
                taskAnalysis1.Name = "Completed tasks";
                taskAnalysis1.ObjectTypeName = typeof(DemoTask).FullName;
                taskAnalysis1.Criteria = "[Status] = ##Enum#DevExpress.Persistent.Base.General.TaskStatus,Completed#";
            }
            Analysis taskAnalysis2 = ObjectSpace.FindObject<Analysis>(CriteriaOperator.Parse("Name='Estimated and actual work comparison'"));
            if(taskAnalysis2 == null) {
                taskAnalysis2 = ObjectSpace.CreateObject<Analysis>();
                taskAnalysis2.Name = "Estimated and actual work comparison";
                taskAnalysis2.ObjectTypeName = typeof(DemoTask).FullName;
            }
        }
    }
}