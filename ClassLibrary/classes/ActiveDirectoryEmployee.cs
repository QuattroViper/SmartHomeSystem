using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.DirectoryServices.AccountManagement;

namespace ClassLibrary.classes
{
    public class ActiveDirectoryEmployee
    {

        #region Fields 
        private string username;
        private string password;
        const string domainName = "howldev";
        private ActiveDirectoryEmployee adPrincipal;

        private string employeeID;
        private string displayName;
        private string email;
        private bool? enabled;
        private Guid? guid;
        private string name;
        private string surname;
        private DateTime? lastLogon;
        private bool? isEmployee;
        private bool? isTechnician;
        private bool? isAdministrator;

        #endregion

        #region Constructors

        public ActiveDirectoryEmployee()
        {

        }

        /// <summary>
        /// Only the name of the AD is used here. 
        /// </summary>
        /// <param name="name"></param>
        public ActiveDirectoryEmployee(string name)
        {
            this.name = name;
        }

        /// <summary>
        /// Sets all the detialls the program needs from Active directory.
        /// </summary>
        /// <param name="employeeID"></param>
        /// <param name="displayName"></param>
        /// <param name="email"></param>
        /// <param name="enabled"></param>
        /// <param name="guid"></param>
        /// <param name="name"></param>
        /// <param name="surname"></param>
        /// <param name="lastLogon"></param>
        /// <param name="isEmployee"></param>
        /// <param name="isTechnician"></param>
        /// <param name="isAdministrator"></param>
        public ActiveDirectoryEmployee(string employeeID, string displayName, string email, bool? enabled, Guid? guid, string name, string surname, DateTime? lastLogon, bool? isEmployee, bool? isTechnician, bool? isAdministrator)
        {
            this.employeeID = employeeID;
            this.displayName = displayName;
            this.email = email;
            this.enabled = enabled;
            this.guid = guid;
            this.name = name;
            this.surname = surname;
            this.lastLogon = lastLogon;
            this.isEmployee = isEmployee;
            this.isTechnician = isTechnician;
            this.isAdministrator = isAdministrator;
        }

        public ActiveDirectoryEmployee(string employeeID, string displayName, string email, bool? enabled, Guid? guid, string name, string surname, DateTime? lastLogon)
        {
            this.employeeID = employeeID;
            this.displayName = displayName;
            this.email = email;
            this.enabled = enabled;
            this.guid = guid;
            this.name = name;
            this.surname = surname;
            this.lastLogon = lastLogon;
        }

        #endregion

        #region Properties

        public bool? IsAdministrator
        {
            get { return isAdministrator; }
            set { isAdministrator = value; }
        }

        public ActiveDirectoryEmployee(string username, string password)
        {
            this.username = username;
            this.password = password;
        }

        public bool? IsTechnician
        {
            get { return isTechnician; }
            set { isTechnician = value; }
        }


        public bool? IsEmployee
        {
            get { return isEmployee; }
            set { isEmployee = value; }
        }

        public DateTime? LastLogon
        {
            get { return lastLogon; }
            set { lastLogon = value; }
        }


        public string Surname
        {
            get { return surname; }
            set { surname = value; }
        }


        public string Name
        {
            get { return name; }
            set { name = value; }
        }


        public Guid? GUID
        {
            get { return guid; }
            set { guid = value; }
        }


        public bool? Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }


        public string Email
        {
            get { return email; }
            set { email = value; }
        }


        public string DisplayName
        {
            get { return displayName; }
            set { displayName = value; }
        }


        public string EmployeeID
        {
            get { return employeeID; }
            set { employeeID = value; }
        }

        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        public string Username
        {
            get { return username; }
            set { username = value; }
        }

        public ActiveDirectoryEmployee ADPrincipal
        {
            get { return adPrincipal; }
            set { adPrincipal = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Authenticates the Active Directory user and specify in which group the user is in. 
        /// </summary>
        /// <returns>bool authenticated</returns>
        public bool AuthenticateUser()
        {

            bool authenticated = false;

            try
            {
                // domainName = howldev.local
                PrincipalContext ctx = new PrincipalContext(ContextType.Domain, domainName);

                // Validate the user against the AD
                bool validated = ctx.ValidateCredentials(this.username, this.password);

                // If the password and the username is correct.
                if (validated == true)
                {
                    // Specify the different groups on Active Directory. 
                    GroupPrincipal employeeGroup = GroupPrincipal.FindByIdentity(ctx, "Employee");
                    GroupPrincipal technicianGroup = GroupPrincipal.FindByIdentity(ctx, "Technician");
                    GroupPrincipal administratorGroup = GroupPrincipal.FindByIdentity(ctx, "Administrators");

                    // Get all the details of the user in AD.
                    UserPrincipal user = UserPrincipal.FindByIdentity(ctx, this.username);

                    if (user != null)
                    {
                        // Check if the user is actually enabled in the system.
                        if (user.Enabled == true)
                        {
                                                           
                            this.adPrincipal = new ActiveDirectoryEmployee(user.EmployeeId, user.DisplayName, user.EmailAddress, user.Enabled, user.Guid, user.Name, user.Surname, user.LastLogon, user.IsMemberOf(employeeGroup), user.IsMemberOf(technicianGroup), user.IsMemberOf(administratorGroup));
                            return authenticated = true;
                        }


                        return authenticated = false;
                    }
                } else
                {
                    return authenticated = false;
                }

                
            }
            catch 
            {
                return authenticated = false;
            }




            return authenticated;

        }


        /// <summary>
        /// Get all the technicians that exist in the Technician group inside Active Directory.
        /// </summary>
        /// <returns></returns>
        public List<ActiveDirectoryEmployee> getAllTechnicians()
        {
            List<ActiveDirectoryEmployee> technicians = new List<ActiveDirectoryEmployee>();


            // Set the contect of the Active Directory. 
            using (var context = new PrincipalContext(ContextType.Domain, domainName))
            {

                using (var group = GroupPrincipal.FindByIdentity(context, "Technician"))
                {
                    if (group != null)
                    {
                        var users = group.GetMembers(true);
                        foreach (UserPrincipal user in users)
                        {
                            //user variable has the details about the user 
                            technicians.Add(
                                    new ActiveDirectoryEmployee(user.EmployeeId, user.DisplayName, user.EmailAddress, user.Enabled, user.Guid, user.Name, user.Surname, user.LastLogon, false, true, false)
                                );
                        }                      
                    }
                    else
                    {
                        return technicians;
                    }
                }
            }

            return technicians;
        }


        /// <summary>
        /// Find a specific user in the Active Directory based on the GUID of that particular user.
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public ActiveDirectoryEmployee getSpecificTechnician(Guid guid)
        {

            using (PrincipalContext ctx = new PrincipalContext(ContextType.Domain, domainName))
            {
                // find the specific user in active directory.
                UserPrincipal user = UserPrincipal.FindByIdentity(ctx, IdentityType.Guid, guid.ToString());

                if (user != null)
                {
                    // get the User details of the searched user.
                    return new ActiveDirectoryEmployee(user.EmployeeId, user.DisplayName, user.EmailAddress, user.Enabled, user.Guid, user.Name, user.Surname, user.LastLogon, false, true, false);
                } else
                {
                    return new ActiveDirectoryEmployee("No User");
                }
            }
        }

        #endregion

    }
}
