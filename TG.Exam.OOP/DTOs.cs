using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TG.Exam.OOP
{
    public class BaseEntity
    {
        public virtual string ToString2()
        {
            return this.ToString();
        }
    }

    public class Employee : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Salary { get; set; }
        public override string ToString2()
        {
            return $"{FirstName} {LastName} {Salary}";
        }
    }

    public class SalesManager : Employee
    {
        public int BonusPerSale { get; set; }
        public int SalesThisMonth { get; set; }
        public override string ToString2()
        {
            return $"{base.ToString2()} {BonusPerSale} {SalesThisMonth}";
        }
    }

    public class CustomerServiceAgent : Employee
    {
        public int Customers { get; set; }
        public override string ToString2()
        {
            return $"{base.ToString2()} {Customers}";
        }
    }

    public class Dog: BaseEntity
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public override string ToString2()
        {
            return $"{Name} {Age}";
        }
    }
}
