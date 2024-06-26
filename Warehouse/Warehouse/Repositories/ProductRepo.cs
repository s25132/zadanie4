﻿using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Warehouse.Exceptions;
using Warehouse.Model;

namespace Warehouse.Repositories
{
    public class ProductRepo : IProductRepo
    {

        private IConfiguration _configuration;

        public ProductRepo(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<int> AddProductToWarehouseAsync(Request addRequest)
        {
            using var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
            using var cmd = new SqlCommand();
            cmd.Connection = con;
            await con.OpenAsync();

            DbTransaction tran = await con.BeginTransactionAsync();
            cmd.Transaction = (SqlTransaction)tran;

            try
            {
                cmd.CommandText = "select Price from Product where IdProduct = @idProduct";
                cmd.Parameters.AddWithValue("@idProduct", addRequest.IdProduct);

                Double price = Convert.ToDouble(await cmd.ExecuteScalarAsync());
                if (price > 0)
                {
                    cmd.Parameters.Clear();
                    cmd.CommandText = "select count(1) from Warehouse where IdWarehouse = @idWarehouse";
                    cmd.Parameters.AddWithValue("@idWarehouse", addRequest.IdWarehouse);

                    Int32 countWarehouse = Convert.ToInt32(await cmd.ExecuteScalarAsync());

                    if(countWarehouse > 0)
                    {  //Warehouse istnieje

                        cmd.Parameters.Clear();
                        Int32 orderId = 0;
                        cmd.CommandText = "select IdOrder from \"Order\" where IdProduct = @idProduct and Amount = @amount and CreatedAt < @createdAt";
                        cmd.Parameters.AddWithValue("@idProduct", addRequest.IdProduct);
                        cmd.Parameters.AddWithValue("@amount", addRequest.Amount);
                        cmd.Parameters.AddWithValue("@createdAt", addRequest.CreatedAt);

                        orderId = Convert.ToInt32(await cmd.ExecuteScalarAsync());
                        Console.WriteLine("orderId : " + orderId);
                        if (orderId > 0)
                        { //Order istnieje


                            cmd.Parameters.Clear();
                            cmd.CommandText = "select count(1) from Product_Warehouse where IdOrder = @idOrder";
                            cmd.Parameters.AddWithValue("@idOrder", orderId);

                            Int32 countOrder = Convert.ToInt32(await cmd.ExecuteScalarAsync());
                            Console.WriteLine("countOrder : " + countOrder);

                            if (countOrder == 0)
                            { // nie ma wiersza Product_Warehouse, kontynuujemy pracę 

                                //update
                                cmd.Parameters.Clear();
                                cmd.CommandText = "UPDATE \"Order\" SET FulfilledAt=@fulfilledAt WHERE IdOrder = @idOrder";
                                cmd.Parameters.AddWithValue("@fulfilledAt", DateTime.Now);
                                cmd.Parameters.AddWithValue("@idOrder", orderId);
                                await cmd.ExecuteNonQueryAsync();
                                //insert

                                cmd.Parameters.Clear();
                                cmd.CommandText = "INSERT INTO Product_Warehouse(IdWarehouse, IdProduct, IdOrder, Amount, Price, CreatedAt) " +
                                "OUTPUT INSERTED.IdProductWarehouse " +
                                "VALUES(@idWarehouse, @idProduct, @idOrder, @amount, @price, @createdAt)";
                                cmd.Parameters.AddWithValue("@idWarehouse", addRequest.IdWarehouse);
                                cmd.Parameters.AddWithValue("@idProduct", addRequest.IdProduct);
                                cmd.Parameters.AddWithValue("@idOrder", orderId);
                                cmd.Parameters.AddWithValue("@amount", addRequest.Amount);
                                cmd.Parameters.AddWithValue("@price", addRequest.Amount * price);
                                cmd.Parameters.AddWithValue("@createdAt", DateTime.Now);

                                var id = (int) await cmd.ExecuteScalarAsync();

                                await tran.CommitAsync();
                                return id;
                            } 
                            else
                            {
                                throw new ProductWarehouseRowExists("ProductWarehouseRowExists for order " + orderId);
                            }
                        } 
                        else
                        { //Order nie istnieje
                            throw new OrderNotFound("OrderNotFound");
                        }
                    }
                    else
                    {  //Warehouse nie istnieje
                        throw new WarehouseNotFound("Warehouse " + addRequest.IdWarehouse + " NotFound");
                    }
                }
                else
                {   //Produkt nie istnieje
                    throw new ProductNotFound("Product "+ addRequest.IdProduct + " NotFound");
                }
            }
            catch (Exception ex)
            {
                await tran.RollbackAsync();
                throw;
            }
        }

        public async Task<int> AddProductToWarehouseByProcedureAsync(Request addRequest)
        {
            using var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
            using var com = new SqlCommand("AddProductToWarehouse", con);
            await con.OpenAsync();


            try
            {
                com.CommandType = CommandType.StoredProcedure;

                com.Parameters.Add(new SqlParameter("@IdProduct", addRequest.IdProduct));
                com.Parameters.Add(new SqlParameter("@IdWarehouse", addRequest.IdWarehouse));
                com.Parameters.Add(new SqlParameter("@Amount", addRequest.Amount));
                com.Parameters.Add(new SqlParameter("@CreatedAt", addRequest.CreatedAt));

                Decimal result = (Decimal) await com.ExecuteScalarAsync();

                return Decimal.ToInt32(result);

            }
            catch (Exception ex)
            {
                throw;
            }

        }
    }
}
