﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Server_cloudata.Models;

namespace Server_cloudata.Services
{
    public class CustomersService
    {
        public IMongoCollection<Customer> _customersCollection { private set; get; }

        public CustomersService(IOptions<CustomerDatabaseSettings> customerDatabaseSettings)
        {
            var mongoClient = new MongoClient(
                customerDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                customerDatabaseSettings.Value.DatabaseName);

            _customersCollection = mongoDatabase.GetCollection<Customer>(
                customerDatabaseSettings.Value.CustomersCollectionName);
        }

        public async Task<List<Customer>> GetAsync() =>
            await _customersCollection.Find(_ => true).ToListAsync();

        public async Task<Customer> GetAsync(string id) =>
            await _customersCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task<Customer> GetAsyncByEmail(string email) =>
            await _customersCollection.Find(x => x.Email == email).FirstOrDefaultAsync();

        public async Task CreateAsync(Customer newCustomer) =>
            await _customersCollection.InsertOneAsync(newCustomer);

        public async Task UpdateAsync(string id, Customer updatedCustomer) =>
            await _customersCollection.ReplaceOneAsync(x => x.Id == id, updatedCustomer);

        public async Task AddVMAsync(Customer updatedCustomer) =>
            await _customersCollection.ReplaceOneAsync(x => x.Email == updatedCustomer.Email, updatedCustomer);

        public async Task RemoveAsync(string id) =>
            await _customersCollection.DeleteOneAsync(x => x.Id == id);

        public async Task UpdateVMAsync(Customer updatedCustomer)
        {
            var filter = Builders<Customer>.Filter.Eq(x => x.Email, updatedCustomer.Email);
            await _customersCollection.ReplaceOneAsync(filter, updatedCustomer);
        }
    }
}
