using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary
{
    public static class ShoppingItems
    {
        public static string GetStudentID(string email)
        {
            string getStudentID = $"SELECT StudentID FROM dbo.Users WHERE Email='{email}';";
            string studentID = SqlDataAccess.selectQuery(getStudentID);
            return studentID;
        }
        public static List<Clothing_Item> LoadClothingItems()
        {
            string sql = @"SELECT row_no, tbl.ClothingID, tbl.CategoryID, tbl.Name, tbl.Description, tbl.Price
                          FROM (SELECT *, ROW_NUMBER() OVER(PARTITION BY CategoryID ORDER BY PRICE desc)
                          AS row_no FROM Clothing_Item)
                          tbl WHERE row_no<=2";
            return SqlDataAccess.LoadData<Clothing_Item>(sql);
        }

        public static List<Clothing_Item> LoadShirts()
        {
            string sql = @"SELECT ClothingID, Clothing_Item.Name, Clothing_Item.Description, Price
            FROM Clothing_Item
            JOIN CLothing_Category on Clothing_Category.CategoryID = Clothing_Item.CategoryID
            WHERE Clothing_Item.CategoryID = 1
            ORDER BY price asc;";
            return SqlDataAccess.LoadData<Clothing_Item>(sql);
        }

        public static List<Clothing_Item> LoadSweatshirts()
        {
            string sql = @"SELECT ClothingID, Clothing_Item.Name, Clothing_Item.Description, Price
            FROM Clothing_Item
            JOIN CLothing_Category on Clothing_Category.CategoryID = Clothing_Item.CategoryID
            WHERE Clothing_Item.CategoryID = 2
            ORDER BY price asc;";
            return SqlDataAccess.LoadData<Clothing_Item>(sql);
        }

        public static List<Clothing_Item> LoadPants()
        {
            string sql = @"SELECT ClothingID, Clothing_Item.Name, Clothing_Item.Description, Price
            FROM Clothing_Item
            JOIN CLothing_Category on Clothing_Category.CategoryID = Clothing_Item.CategoryID
            WHERE Clothing_Item.CategoryID = 3
            ORDER BY price asc;";
            return SqlDataAccess.LoadData<Clothing_Item>(sql);
        }

        public static List<Cart_Item> LoadShoppingCart(string email)
        {
            //get studentID in order to get OrderID
            string studentID = GetStudentID(email);
            string getOrderID = $"SELECT OrderID FROM Orders WHERE StudentID={studentID} AND Submitted_Date IS NULL;";
            var orderID = SqlDataAccess.selectQuery(getOrderID);
            //if the user doesn't have an order in db yet, then error will occur...
            if (orderID == "no order created yet")
            {
                return null;
            }
            else { 
                string sql = $"SELECT Order_Details.ClothingID, Name, Description, Price, Quantity FROM Order_Details" +
                    $" INNER JOIN Orders ON Orders.OrderID = Order_Details.OrderID" +
                    $" INNER JOIN Clothing_Item ON Clothing_Item.ClothingID = Order_Details.ClothingID" +
                    $" WHERE Order_Details.OrderID={orderID}" +
                    $" ORDER BY Name;";
                return SqlDataAccess.LoadData<Cart_Item>(sql);
            }
        }

        public static int AddtoCart(string email, int clothingID, int qty)
        {
            string studentID = GetStudentID(email);
            string procName = "dbo.spAddtoCart";
            return SqlDataAccess.Insert(procName, studentID, clothingID, qty);
        }

        public static int DeleteFromCart(string email, int clothingID)
        {
            string studentID = GetStudentID(email);
            string getOrderID = $"SELECT OrderID FROM Orders WHERE studentID={studentID} and Submitted_Date IS NULL;";
            string orderID = SqlDataAccess.selectQuery(getOrderID);
            string sql = $"DELETE FROM Order_Details WHERE ClothingID={clothingID} AND OrderID={orderID}";
            return SqlDataAccess.Delete(sql);
        }

        public static int UpdateQuantity(string email, List<string> Quantities)
        {
            //get orderID
            string studentID = GetStudentID(email);
            string getOrderID = $"SELECT OrderID FROM Orders WHERE studentID={studentID} and Submitted_Date IS NULL;";
            string orderID = SqlDataAccess.selectQuery(getOrderID);

            //get All ClothingID from OrderID, store the ID attr of Cart_Item inside of a list<string>
            string getClothingIDs = $"SELECT Order_Details.ClothingID FROM Order_Details" +
                $" INNER JOIN Clothing_Item on Clothing_Item.ClothingID=Order_Details.ClothingID" +
                $" WHERE OrderID={orderID}" +
                $" ORDER BY Name;";
            List<Cart_Item> cart = SqlDataAccess.LoadData<Cart_Item>(getClothingIDs);
            List<string> clothingIDs = new List<string>();
            foreach(var item in cart){
                clothingIDs.Add(item.ClothingID.ToString());
            }

            //pass OrderID, clothingIDs, and Qtys
            string sql = $"UPDATE Order_Details SET Quantity=@Quantity WHERE ClothingID=@ClothingID AND OrderID=@OrderID;";
            return SqlDataAccess.UpdateCart(sql, orderID, clothingIDs, Quantities);
        }

        public static int Checkout(string email)
        {
            string studentID = GetStudentID(email);
            string sql = $"UPDATE Orders SET Submitted_Date=GETDATE() WHERE StudentID={studentID} AND Submitted_Date IS NULL;";
            return SqlDataAccess.UpdateNoValues(sql);
        }
    }
}
