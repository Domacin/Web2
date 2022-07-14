using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

namespace Restaurant.Models
{

    public class DataBase
    {
       
        public DataBase()
        {
            //mysql = new SqlConnection(connection);
            //mysql.Open();

            //string query = "INSERT INTO CLIENT(UM,PASS,U_FNAME,U_LNAME,U_DB,ADRESS,U_TYPE,ACTIVE) VALUES('TEST USER NAME','TEST PASSWORD','TEST FIRST NAME','TEST LAST NAME',getdate(),'TEST ADRESS','TEST TYPE','NO')";
            //SqlCommand command = new SqlCommand(query, mysql);
            //command.ExecuteNonQuery();
            //mysql.Close();
        }

        //Connects item(dish) to an order 
        public static void AddItemToOrder(int itemId,int orderId)
        {
            string connection = @"Server=(localdb)\MSSQLLocalDB;Database=DOSTAVA;Trusted_Connection=True;";
            SqlConnection mysql = new SqlConnection(connection);
            mysql.Open();

            string query = "INSERT INTO OREDRINGITEM(ITEM_ID,ORDER_ID) VALUES(" +itemId+","+orderId+")";
            SqlCommand command = new SqlCommand(query, mysql);
            command.ExecuteNonQuery();
            mysql.Close();
        }
        //Gets info about what orders were taken by which deliverers
        public static List<OrderDeliverer> LoadTakenOrders()
        {
            List<OrderDeliverer> orders = new List<OrderDeliverer>();
            string connection = @"Server=(localdb)\MSSQLLocalDB;Database=DOSTAVA;Trusted_Connection=True;";
            SqlConnection mysql = new SqlConnection(connection);
            mysql.Open();
            string query = "SELECT * FROM ORDERINGDELIVERER";
            SqlCommand command = new SqlCommand(query, mysql);
            SqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                OrderDeliverer o = new OrderDeliverer();
                o.Deliverer_ID = dr.GetInt32(0);
                o.Order_ID = dr.GetInt32(1);
                orders.Add(o);
            }
            mysql.Close();

            return orders;

        }

        //Update completed field of order in database
        public static void OrderCompleted(int orderId)
        {
            string connection = @"Server=(localdb)\MSSQLLocalDB;Database=DOSTAVA;Trusted_Connection=True;";
            SqlConnection mysql = new SqlConnection(connection);
            mysql.Open();
            string query = "UPDATE ORDERING SET COMPLETED = 'YES' WHERE O_ID = " + orderId;
            SqlCommand command = new SqlCommand(query, mysql);
            command.ExecuteNonQuery();
            mysql.Close();
        }
        //Update accepted fieled of order in dataBase
        public static void AcceptOrder(int orderId)
        {
            string connection = @"Server=(localdb)\MSSQLLocalDB;Database=DOSTAVA;Trusted_Connection=True;";
            SqlConnection mysql = new SqlConnection(connection);
            mysql.Open();
            string query = "UPDATE ORDERING SET ACCEPTED = 'YES' WHERE O_ID = "+ orderId;
            SqlCommand command = new SqlCommand(query, mysql);
            command.ExecuteNonQuery();
            mysql.Close();
        }
        //Connects order with a deliverer
        public static void AddOrderToDeliverer(int delivererID,int orderID)
        {
            string connection = @"Server=(localdb)\MSSQLLocalDB;Database=DOSTAVA;Trusted_Connection=True;";
            SqlConnection mysql = new SqlConnection(connection);
            mysql.Open();

            string query = "INSERT INTO ORDERINGDELIVERER(DELIVERER,ORDER_ID) VALUES(" + delivererID + "," + orderID + ")";
            SqlCommand command = new SqlCommand(query, mysql);
            command.ExecuteNonQuery();
            mysql.Close();
        }

        //Gets all items with orders they are conencted to
        public static List<OrderingItem> LoadOrderItem()
        {
            List<OrderingItem> orderItem = new List<OrderingItem>();
            string connection = @"Server=(localdb)\MSSQLLocalDB;Database=DOSTAVA;Trusted_Connection=True;";
            SqlConnection mysql = new SqlConnection(connection);
            mysql.Open();
            string query = "SELECT * FROM OREDRINGITEM";
            SqlCommand command = new SqlCommand(query, mysql);
            SqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                OrderingItem o = new OrderingItem();
                o.ItemID = dr.GetInt32(0);
                o.OrderID = dr.GetInt32(1);

                orderItem.Add(o);
            }
            mysql.Close();

            return orderItem;
        }
        //Gets all orders
        public static List<Ordering> LoadAllOrdersFromDataBase()
        {
            List<Ordering> orders = new List<Ordering>();
            string connection = @"Server=(localdb)\MSSQLLocalDB;Database=DOSTAVA;Trusted_Connection=True;";
            SqlConnection mysql = new SqlConnection(connection);
            mysql.Open();
            string query = "SELECT * FROM ORDERING";
            SqlCommand command = new SqlCommand(query, mysql);
            SqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                Ordering o = new Ordering();
                o.OrderId = dr.GetInt32(0);
                o.Address = dr.GetString(1);
                o.Comment = dr.GetString(2);
                o.OrderPrice = dr.GetInt32(3);
                o.ClientId = dr.GetInt32(4);
                string compelted = dr.GetString(5);
                string accepted = dr.GetString(6);

                if (compelted == "YES")
                    o.Completed = true;
                else
                    o.Completed = false;

                if (accepted == "YES")
                    o.Accepted = true;
                else
                    o.Accepted = false;

                orders.Add(o);


            }
            mysql.Close();


            return orders;

        }
        //Adds order to database
        public static void AddOrderToDataBase(Ordering o)
        {
            string connection = @"Server=(localdb)\MSSQLLocalDB;Database=DOSTAVA;Trusted_Connection=True;";
            SqlConnection mysql = new SqlConnection(connection);
            mysql.Open();

            string query = "INSERT INTO ORDERING(O_ADRESS,COMMENT,O_PRICE,CUSTOMER,COMPLETED,ACCEPTED) VALUES('"+o.Address+"','"+o.Comment+"',"+o.OrderPrice+","+o.ClientId+",'NO','NO')";
            SqlCommand command = new SqlCommand(query, mysql);
            command.ExecuteNonQuery();
            mysql.Close();
        }
        //Verifying the deliverer so he can start taking orders
        public static void VerifyUser(int id)
        {
            string connection = @"Server=(localdb)\MSSQLLocalDB;Database=DOSTAVA;Trusted_Connection=True;";
            SqlConnection mysql = new SqlConnection(connection);
            mysql.Open();

            string query = "UPDATE CLIENT SET VER = 'YES' WHERE U_ID = " +id;
            SqlCommand command = new SqlCommand(query, mysql);
            command.ExecuteNonQuery();
            mysql.Close();
        }

        //activate user account
        public static void ActivateUser(int id)
        {
            string connection = @"Server=(localdb)\MSSQLLocalDB;Database=DOSTAVA;Trusted_Connection=True;";
            SqlConnection mysql = new SqlConnection(connection);
            mysql.Open();

            string query = "UPDATE CLIENT SET ACTIVE = 'YES' WHERE U_ID = " + id;
            SqlCommand command = new SqlCommand(query, mysql);
            command.ExecuteNonQuery();
            mysql.Close();
        }
        //crypting  password
        public static string sha256(string randomString)
        {
            var crypt = new System.Security.Cryptography.SHA256Managed();
            var hash = new System.Text.StringBuilder();
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(randomString));
            foreach (byte theByte in crypto)
            {
                hash.Append(theByte.ToString("x2"));
            }
            return hash.ToString();
        }

        //Setting a price for dish
        public static void UpdateItemPrice(int price,int itemID)
        {
            string connection = @"Server=(localdb)\MSSQLLocalDB;Database=DOSTAVA;Trusted_Connection=True;";
            SqlConnection mysql = new SqlConnection(connection);
            mysql.Open();
            string query = "UPDATE ITEM SET I_PRICE =" + price + "WHERE I_ID = " + itemID;
            SqlCommand command = new SqlCommand(query, mysql);
            command.ExecuteNonQuery();
            mysql.Close();
        }

        //Loads all data that says what dishes have what ingredientes
        public static List<IngredienteItem> LoadAllIngredienteItemsFromDataBase()
        {
            List<IngredienteItem> i = new List<IngredienteItem>();
            string connection = @"Server=(localdb)\MSSQLLocalDB;Database=DOSTAVA;Trusted_Connection=True;";
            SqlConnection mysql = new SqlConnection(connection);
            mysql.Open();
            string query = "SELECT * FROM INGREDIENTEITEM";
            SqlCommand command = new SqlCommand(query, mysql);
            SqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                IngredienteItem ii = new IngredienteItem();
                int itemId = dr.GetInt32(0);
                int ingredienteId = dr.GetInt32(1);

                ii.ItemId = itemId;
                ii.IngredienteId = ingredienteId;
                i.Add(ii);

            }
            mysql.Close();

            return i;
        }

        //Loads all dishes
        public static List<Item> LoadAllItems()
        {
            List<Item> items = new List<Item>();
            string connection = @"Server=(localdb)\MSSQLLocalDB;Database=DOSTAVA;Trusted_Connection=True;";
            SqlConnection mysql = new SqlConnection(connection);
            mysql.Open();
            string query = "SELECT * FROM ITEM";
            SqlCommand command = new SqlCommand(query, mysql);
            SqlDataReader dr = command.ExecuteReader();

            while (dr.Read())
            {
                Item i = new Item();
                int itemId = dr.GetInt32(0);
                string itemName = dr.GetString(1);
                int itemPrice = dr.GetInt32(2);

                i.ItemId = itemId;
                i.ItemName = itemName;
                i.ItemPrice = itemPrice;
                items.Add(i);
               
            }
            mysql.Close();
            return items;

        }

        //Connects ingredientes to dishes
        public static void AddItemIngredianteToDataBase(int itemId,int ingredienteId)
        {
            string connection = @"Server=(localdb)\MSSQLLocalDB;Database=DOSTAVA;Trusted_Connection=True;";
            SqlConnection mysql = new SqlConnection(connection);
            mysql.Open();
            string query = "INSERT INTO INGREDIENTEITEM(ID_ITEM,ID_INGREDIENTE) VALUES( " +itemId+ ","+ ingredienteId+")";
            SqlCommand command = new SqlCommand(query, mysql);
            command.ExecuteNonQuery();
            mysql.Close();
        }

        //Add dish to database
        public static void AddItemToDataBase(Item i)
        {
            string connection = @"Server=(localdb)\MSSQLLocalDB;Database=DOSTAVA;Trusted_Connection=True;";
            SqlConnection mysql = new SqlConnection(connection);
            mysql.Open();
            string query = "INSERT INTO ITEM(I_NAME,I_PRICE) VALUES(' " + i.ItemName+"'," + i.ItemPrice + ")";
            SqlCommand command = new SqlCommand(query, mysql);
            command.ExecuteNonQuery();
            mysql.Close();
        }
        //Load all ingredientes
        public static List<Ingrediente> LoadAllIngredientes()
        {
            List<Ingrediente> ingredientes = new List<Ingrediente>();

            string connection = @"Server=(localdb)\MSSQLLocalDB;Database=DOSTAVA;Trusted_Connection=True;";

            Dictionary<string, User> users = new Dictionary<string, User>();
            SqlConnection mysql = new SqlConnection(connection);
            mysql.Open();
            string query = "SELECT * FROM INGREDIENTE";
            SqlCommand command = new SqlCommand(query, mysql);
            SqlDataReader dr = command.ExecuteReader();

            while (dr.Read())
            {
                Ingrediente i = new Ingrediente();
                int ingredienteId = dr.GetInt32(0);
                string ingredienteName = dr.GetString(1);
                int ingredietntePrice = dr.GetInt32(2);

                i.IngredienteId = ingredienteId;
                i.IngredienteName = ingredienteName;
                i.IngredientePrice = ingredietntePrice;
                ingredientes.Add(i);
            }
            mysql.Close();



            return ingredientes;
        }
        //Add ingrediente to database
        public static void AddIngredienteToDataBase(Ingrediente i)
        {
            string connection = @"Server=(localdb)\MSSQLLocalDB;Database=DOSTAVA;Trusted_Connection=True;";
            SqlConnection mysql = new SqlConnection(connection);
            mysql.Open();
            string query = "INSERT INTO INGREDIENTE(IN_NAME,IN_PRICE) VALUES (' " + i.IngredienteName+ "',"+i.IngredientePrice+")";
            SqlCommand command = new SqlCommand(query, mysql);
            command.ExecuteNonQuery();
            mysql.Close();
        }
        //Add user(customer or deliverer)
        public static void AddUserToDataBase(User u)
        {
            string connection = @"Server=(localdb)\MSSQLLocalDB;Database=DOSTAVA;Trusted_Connection=True;";
            SqlConnection mysql = new SqlConnection(connection);
            string date = u.DateOfBirth.ToString();
            mysql.Open();
            string tip = "";
            string active = "";
            string pass = sha256(u.Password);
            string verified = "";
           

            if (u.Tip == User.TIP.CONSUMER)
                tip = "CONSUMER";
            else if (u.Tip == User.TIP.DELIVERER)
                tip = "DELIVERER";

            if (u.Active == true)
                active = "YES";
            else
                active = "NO";

            if (u.Verified == true)
                verified = "YES";
            else
                verified = "NO";

            string query = "INSERT INTO CLIENT(UM,PASS,U_FNAME,U_LNAME,U_DB,ADRESS,U_TYPE,ACTIVE,VER,IMG)" +
                " VALUES('" + u.UserName +/* "',HASHBYTES('SHA2_256','" + u.Password + "'),'"*/ 
                "','"+pass+"','" + u.FirstName
                + "','" + u.LastName + "',CAST('"+date+"' AS DATE),'"
                + u.Address + "','" + tip + "','" + active + "','" + verified + "','" +u.Image+"')";
            SqlCommand command = new SqlCommand(query, mysql);
            command.ExecuteNonQuery();
            mysql.Close();
        }
        //Updae user info
        public static void UpdateUserToDataBase(User u)
        {
            string connection = @"Server=(localdb)\MSSQLLocalDB;Database=DOSTAVA;Trusted_Connection=True;";

            
            SqlConnection mysql = new SqlConnection(connection);
           
            mysql.Open();
            string query = "UPDATE CLIENT SET PASS= '" + u.Password + "',U_FNAME = '" + u.FirstName +"',U_LNAME ='"
                +u.LastName + "',ADRESS = '"+u.Address + "',IMG = '"+u.Image + "' WHERE U_ID = "+u.UserId;

            SqlCommand command = new SqlCommand(query, mysql);
            command.ExecuteNonQuery();
            mysql.Close();
        }
        //Load all users
        public static Dictionary<string,User> LoadAllUsers()
        {
            string connection = @"Server=(localdb)\MSSQLLocalDB;Database=DOSTAVA;Trusted_Connection=True;";

            Dictionary<string, User> users = new Dictionary<string, User>();
            SqlConnection mysql = new SqlConnection(connection);
            mysql.Open();
            string query = "SELECT * FROM CLIENT";
            SqlCommand command = new SqlCommand(query, mysql);
            SqlDataReader dr = command.ExecuteReader();

            while (dr.Read())
            {
                User u = new User();
                int userId = dr.GetInt32(0);
                string userUserName = dr.GetString(1);
                string password = dr.GetString(2);
                string firstName = dr.GetString(3);
                string lastName = dr.GetString(4);
                DateTime dateOfBirth = dr.GetDateTime(5);
                string address = dr.GetString(6);
                string type = dr.GetString(7);
                string active = dr.GetString(8);
                string image = dr.GetString(9);
                string verified = dr.GetString(10);

                if (active == "YES")
                    u.Active = true;
                else
                    u.Active = false;

                if (type == "ADMIN")
                    u.Tip = User.TIP.ADMIN;
                else if (type == "DELIVERER")
                    u.Tip = User.TIP.DELIVERER;
                else if (type == "CONSUMER")
                    u.Tip = User.TIP.CONSUMER;

                if (image == null)
                    u.Image = "a.jpg";
                else
                    u.Image = image;

                if (verified == "YES")
                    u.Verified = true;
                else
                    u.Verified = false;
                
                u.Address = address;
                u.DateOfBirth = dateOfBirth;
                u.LastName = lastName;
                u.FirstName = firstName;
                u.Password = password;
                u.UserId = userId;
                u.UserName = userUserName;

                users.Add(u.UserName, u);
                
            }
            mysql.Close();

            return users;

        }
    }
}