﻿using System.Linq;
using System.Threading.Tasks;
using Mollie.Api.Models.Customer;
using Mollie.Api.Models.List;
using Mollie.Api.Models.Mandate;
using Mollie.Tests.Integration.Framework;
using NUnit.Framework;

namespace Mollie.Tests.Integration.Api {
    [TestFixture]
    public class MandateTests : BaseMollieApiTestClass {
        [Test]
        public async Task CanRetrieveMandateList() {
            // We can only test this if there are customers
            ListResponse<CustomerResponse> customers = await this._customerClient.GetCustomerListAsync();

            if (customers.TotalCount > 0) {
                // When: Retrieve mandate list with default settings
                ListResponse<MandateResponse> response = await this._mandateClient.GetMandateListAsync(customers.Data.First().Id);

                // Then
                Assert.IsNotNull(response);
            }
        }

        [Test]
        public async Task ListMandatesNeverReturnsMoreCustomersThenTheNumberOfRequestedMandates() {
            // We can only test this if there are customers
            ListResponse<CustomerResponse> customers = await this._customerClient.GetCustomerListAsync();

            if (customers.TotalCount > 0) {
                // If: Number of customers requested is 5
                int numberOfMandates = 5;

                // When: Retrieve 5 mandates
                ListResponse<MandateResponse> response = await this._mandateClient.GetMandateListAsync(customers.Data.First().Id, 0, numberOfMandates);

                // Then
                Assert.IsTrue(response.Data.Count <= numberOfMandates);
            }
        }

        [Test]
        public async Task CanCreateMandate() {
            // We can only test this if there are customers
            ListResponse<CustomerResponse> customers = await this._customerClient.GetCustomerListAsync();
            if (customers.TotalCount > 0) {
                // If: We create a new mandate request
                MandateRequest mandateRequest = new MandateRequest() {
                    ConsumerAccount = "NL26ABNA0516682814",
                    ConsumerName = "John Doe"
                };

                // When: We send the mandate request
                MandateResponse mandateResponse = await this._mandateClient.CreateMandateAsync(customers.Data.First().Id, mandateRequest);

                // Then: Make sure we created a new mandate
                Assert.AreEqual(mandateRequest.ConsumerAccount, mandateResponse.Details.ConsumerAccount);
                Assert.AreEqual(mandateRequest.ConsumerName, mandateResponse.Details.ConsumerName);
            }
        }
    }
}
