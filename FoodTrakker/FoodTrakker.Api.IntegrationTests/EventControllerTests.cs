﻿using AutoMapper;
using FoodTrakker.Api.Models;
using FoodTrakker.Core.Model;
using FoodTrakker.Services.DTOs;
using System.Text.Json;

namespace FoodTrakker.Api.IntegrationTests
{
    public class EventControllerTests
    {
        private HttpClient _client;
        private WebApplicationFactory<Program> _factory;
        private Mock<IEventRepository> _eventRepository = new Mock<IEventRepository>();


        public EventControllerTests()
        {
            _factory = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        var dbContextServices = services.SingleOrDefault(service => service.ServiceType == typeof(DbContextOptions<FoodTrakkerContext>));
                        services.Remove(dbContextServices);

                        services.AddSingleton<IEventRepository>(_eventRepository.Object);

                        services
                            .AddDbContext<FoodTrakkerContext>(options => options.UseInMemoryDatabase("FoodTrakkerDb"));
                    });
                });
            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task Get_ForProperRequest_ReturnOk()
        {
            //arrange
            _eventRepository
                .Setup(e => e.GetAsync())
                .Returns(Task.FromResult(FakeDbEvents.Events));

            //act
            var response = await _client.GetAsync("/api/Event");

            //assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task Get_ForEmptyEventList_ReturnOk()
        {
            //arrange

            _eventRepository
                .Setup(e => e.GetAsync())
                .Returns(Task.FromResult(FakeDbEvents.EmptyEventList));
            //act
            var response = await _client.GetAsync("/api/Event");

            //assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task GetEventById_ForValidRequest_ReturnOk(int Id)
        {
            //arrange
            _eventRepository
                .Setup(e => e.GetAsync(Id))
                .Returns(Task.FromResult(FakeDbEvents.Events.Single(e => e.Id == Id)));
            //act
            var response = await _client.GetAsync($"/api/Event/{Id}");
            //var result = await response.Content.ReadFromJsonAsync<Event>();
            //assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task GetEventById_ForValidRequest_ReturnCorrectObject(int Id)
        {
            //arrange
            var testObject = FakeDbEvents.Events[Id - 1];

            _eventRepository
                .Setup(e => e.GetAsync(Id))
                .Returns(Task.FromResult(FakeDbEvents.Events.Single(e => e.Id == Id)));
            //act
            var response = await _client.GetAsync($"/api/Event/{Id}");
            var result = await response.Content.ReadFromJsonAsync<Event>();
            //assert
            result.Should().BeEquivalentTo(testObject);
        }

        [Fact]
        public async Task GetEventById_ForInvalidRequest_ReturnNotFound()
        {
            //arrange

            //act
            var response = await _client.GetAsync($"/api/Event/50");

            //assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Post_ForProperInput_InvokeAddAsyncMethodOnce()
        {
            //arrange
            EventApiPost eventApiPost = new EventApiPost
            {
                Name = "Dummy1",
                Description = "Test",
                Location = "testLocation",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(1),
            };


            //act
            var response = await _client.PostAsJsonAsync("/api/Event", eventApiPost);

            //assert
            _eventRepository.Verify(e => e.AddAsyncWithReturn(It.Is<Event>(x => eventApiPost.Description == x.Description && x.Name == eventApiPost.Name)), Times.Once);
        }

        [Fact]
        public async Task Post_ForProperInput_ReturnCreatedObject()
        {
            //arrange
            EventApiPost eventApiPost = new EventApiPost
            {
                Name = "Dummy1",
                Description = "Test",
                Location = "testLocation",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(1),
            };

            var returnedEvent = new Event
            {
                Name = eventApiPost.Name,
                Description = eventApiPost.Description,
                Location = eventApiPost.Location,
                StartDate = eventApiPost.StartDate,
                EndDate = eventApiPost.EndDate
            };

            _eventRepository
                .Setup(e => e.AddAsyncWithReturn(It.IsAny<Event>()))
                .Returns(Task.FromResult(returnedEvent));

            //act
            var response = await _client.PostAsJsonAsync("/api/Event", eventApiPost);
            var result = await response.Content.ReadFromJsonAsync<Event>();
            //assert
            result.Should().BeEquivalentTo(returnedEvent);
        }

        [Theory]
        [InlineData(2)]
        public async Task Delete_ForProperInput_ReturnOk(int Id)
        {
            //arrange
            _eventRepository
                .Setup(e => e.DeleteAsync(Id))
                .Returns(Task.FromResult(Task.FromResult(FakeDbEvents.Events.Single(e => e.Id == Id))));
            //act
            var responce = await _client.DeleteAsync($"/api/Event/{Id}");

            //assert
            responce.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task Delete_ForProperInput_InvokeDeleteAsyncMethodOnce()
        {
            //arrange
            int Id = 1;
            //act
            var responce = await _client.DeleteAsync($"/api/Event/{Id}");
            //assert
            _eventRepository.Verify(e => e.DeleteAsync(It.Is<int>(x => x == Id)), Times.Once);
        }

        [Theory]
        [InlineData(1)]
        public async Task Delete_ForInvalidInput_ReturnBadRequest(int Id)
        {
            //arrange

            //act
            var responce = await _client.DeleteAsync($"/api/Event/{Id}");

            //assert
            responce.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }



    }
}