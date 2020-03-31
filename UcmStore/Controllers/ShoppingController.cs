using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataLibrary;
using UcmStore.Models;
using System.Web.SessionState;

namespace UcmStore.Controllers
{
    public class ShoppingController : Controller
    {
        // GET: Shopping
        public ActionResult Index()
        {
            var data = ShoppingItems.LoadClothingItems();
            List<ClothingItem> clothingList = new List<ClothingItem>();
            foreach(var item in data)
            {
                clothingList.Add(
                new ClothingItem {
                    id = item.ClothingID,
                    category = item.CategoryID,
                    name = item.Name,
                    description = item.Description,
                    price = item.Price
                }
                
                );
            }       
            return View(clothingList);
            //create the view with the @model as clothingList
        }

        public ActionResult Shirts()
        {
            var data = ShoppingItems.LoadShirts();
            List<ClothingItem> shirts = new List<ClothingItem>();
            foreach (var row in data) { 
            shirts.Add(
                new ClothingItem
                {
                    id = row.ClothingID,
                    name = row.Name,
                    price = row.Price,
                    description = row.Description
                }
                );
            }
            return View(shirts);
        }
        public ActionResult Sweatshirts()
        {
            var data = ShoppingItems.LoadSweatshirts();
            List<ClothingItem> sweats = new List<ClothingItem>();
            foreach (var row in data)
            {
                sweats.Add(
                    new ClothingItem
                    {
                        id = row.ClothingID,
                        name = row.Name,
                        price = row.Price,
                        description = row.Description
                    }
                    ); ;
            }
            return View(sweats);
        }
        public ActionResult Pants()
        {
            var data = ShoppingItems.LoadPants();
            List<ClothingItem> pants = new List<ClothingItem>();
            foreach (var row in data)
            {
                pants.Add(
                    new ClothingItem
                    {
                        id = row.ClothingID,
                        name = row.Name,
                        price = row.Price,
                        description = row.Description
                    }
                    );
            }
            return View(pants);
        }

        
        public ActionResult ProductDetails(int Id, string Name, double Price, string Desc, string picLocation)
        {
            /*List<ClothingItem> Product = new List<ClothingItem>();
            Product.Add(new ClothingItem
            {
                id = Id,
                name = Name,
                price = Price,
                description = Desc
            }); */
            ClothingItem Product = new ClothingItem
            {
                id = Id,
                name = Name,
                price = Price,
                description = Desc,
                picLocation = picLocation
            };
            return View(Product);

        }

        
        [HttpPost]
        public ActionResult AddToCart(int Id, int Qty)
        {
            HttpContext context = System.Web.HttpContext.Current;
            String email = Session["email_ID"].ToString();
            //String email_id = context.Session["email_ID"].ToString();

            int clothingID = Id;
            int qty = Qty;

            var rowsAffected = ShoppingItems.AddtoCart(email, clothingID, qty);
            if (rowsAffected != 0)
            {
                return RedirectToAction("ShoppingCart");
            }
            else {
                return RedirectToAction("Index");
            }
            
        }

        public ActionResult ShoppingCart()
        {
            String email = Session["email_ID"].ToString();
            var data =ShoppingItems.LoadShoppingCart(email);
            if (data == null)
            {
                //return empty cart b/c shopping cart is empty
                List<CartItem> Cart = new List<CartItem>();
                return View(Cart);
            }
            else
            {
                List<CartItem> Cart = new List<CartItem>();
                foreach (var item in data)
                {
                    Cart.Add(
                        new CartItem
                        {
                            id = item.ClothingID,
                            name = item.Name,
                            Description = item.Description,
                            Price = item.Price,
                            Quantity = item.Quantity
                        }
                    );
                }
                return View(Cart);
            }
        }

        public ActionResult RemoveFromCart(int clothingID)
        {
            string email = Session["email_ID"].ToString();
            int rowsAffected =ShoppingItems.DeleteFromCart(email, clothingID);
            if(rowsAffected != 0)
            {
                return RedirectToAction("ShoppingCart");
            }
            else
            {
                return RedirectToAction("Index");
            }
            
        }

        [HttpPost]
        public ActionResult UpdateQuantity(FormCollection fc)
        {
            if (fc.GetValues("Qty") == null) {
                return RedirectToAction("Index");
            }

            List<string> quantities = fc.GetValues("Qty").ToList();

            string email = Session["email_ID"].ToString();
            int rowsAffected = ShoppingItems.UpdateQuantity(email, quantities);
           
            
            if (rowsAffected != 0)
            {
                return RedirectToAction("ShoppingCart");
            }
            else
            {
                return RedirectToAction("Index");
            }
        }


        public ActionResult Checkout()
        {
            return View();
        }

        public ActionResult SubmitCheckout()
        {
            string email = Session["email_ID"].ToString();
            int rowsAffected = ShoppingItems.Checkout(email);
            if(rowsAffected != 0)
            {
                return RedirectToAction("ShoppingCart");
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
    }
}