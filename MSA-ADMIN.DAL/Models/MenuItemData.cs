using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSA_ADMIN.DAL.Models
{
    public class MenuItemData
    {
        private MenuData pData;
        public MenuItemData()
            : base()
        {

        }
        public MenuItemData(MenuData cdata)
            : base()
        {
            this.InitMenu(cdata);
        }
        public MenuItemData(int id, DateTime date, int meal, int menus_id, int Quantity, string Status, int WebCallID, string Paid)
        {
            pData .MenuID =menus_id;
          
        }
        private void InitMenu(MenuData cdata)
        {
            this.pData = cdata;
        }

        #region Public
        public int MenuID
        {

            get { return pData.MenuID; }
            set { pData.MenuID = value; }
        }
        public int menus_id
        {
            get { return pData.MenuID; }
            set { pData.MenuID = value; }
        }

        public int CategoryID
        {
            get { return pData.Category.Value; }
            //set { pData.Category.Value = value; }
        }
        public int ShowOrder
        {
            set { }
            get { return 0; }
        }
        public string CategoryName
        {
            get { return pData.Category.Name; }
            //set { pData.Category.Name = value; }
        }
        public string ItemName
        {
            get { return pData.ItemName; }
            set { pData.ItemName = value; }
        }

        public string M_F6_Code
        {
            get { return pData.M_F6_Code; }
            set { pData.M_F6_Code = value; }
        }
        public decimal StudentFullPrice
        {
            get { return pData.StudentFullPrice; }
            set { pData.StudentFullPrice = value; }
        }
        public decimal StudentRedPrice
        {
            get { return pData.StudentRedPrice; }
            set { pData.StudentRedPrice = value; }
        }
        public decimal EmployeePrice
        {
            get { return pData.EmployeePrice; }
            set { pData.EmployeePrice = value; }
        }
        public decimal GuestPrice
        {
            get { return pData.GuestPrice; }
            set { pData.GuestPrice = value; }
        }
        public bool isTaxable
        {
            get { return pData.isTaxable; }
            set { pData.isTaxable = value; }
        }
        public bool isDeleted
        {
            get { return pData.isDeleted; }
            set { pData.isDeleted = value; }
        }

        public string AltDescription
        {
            get { return pData.AltDescription; }
            set { pData.AltDescription = value; }
        }

        public int CalId
        {
            get { return pData.CalId; }
            set { pData.CalId = value; }
        }

        #endregion
    }
}
