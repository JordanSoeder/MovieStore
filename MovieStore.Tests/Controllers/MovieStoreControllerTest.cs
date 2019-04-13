
namespace MovieStore.Tests.Controllers
{
    using System;
    using System.Linq;
    using System.Net;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using MovieStore.Controllers;
    using MovieStore.Models;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Web.Mvc;

    [TestClass]
    public class MovieStoreControllerTest
    {
        [TestMethod]
        public void MovieStore_Index_TestView()
        {
            // Arrange
            MoviesController controller = new MoviesController();

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void MovieStore_ListOfMovies()
        {
            // Arrange
            MoviesController controller = new MoviesController();

            // Act
            List<Movie> result = controller.ListOfMovies();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expected:"Iron Man 1", actual: result[0].Title);
            Assert.AreEqual(expected: "Iron Man 2", actual: result[1].Title);
            Assert.AreEqual(expected: "Iron Man 3", actual: result[2].Title);
        }

        [TestMethod]
        public void MovieStore_IndexRedirect_Success()
        {
            // Arrange
            MoviesController controller = new MoviesController();

            // Act
            RedirectToRouteResult result = controller.IndexRedirect(id:1) as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expected: "Create", actual: result.RouteValues["action"]);
            Assert.AreEqual(expected: "HomeController", actual: result.RouteValues["controller"]);
        }

        [TestMethod]
        public void MovieStore_IndexRedirect_BadRequest()
        {
            // Arrange
            MoviesController controller = new MoviesController();

            // Act
            HttpStatusCodeResult result = controller.IndexRedirect(id: 0) as HttpStatusCodeResult;

            // Assert
            Assert.AreEqual(expected: HttpStatusCode.BadRequest, actual:(HttpStatusCode) result.StatusCode);
        }

        [TestMethod]
        public void MovieStore_ListFromDb()
        {
            //Goal: query from out own list instead of DB
            var list = new List<Movie>
            {
                new Movie() {MovieID = 1, Title = "Superman 1"},
                new Movie() {MovieID = 2, Title = "Superman 2"}
            }.AsQueryable();

            Mock<MovieStoreDBContext> mockContext = new Mock<MovieStoreDBContext>();
            Mock<DbSet<Movie>> mockSet = new Mock<DbSet<Movie>>();
            mockSet.As<IQueryable<Movie>>().Setup(expression: m => m.GetEnumerator()).Returns(list.GetEnumerator());
            mockSet.As<IQueryable<Movie>>().Setup(expression: m => m.Provider).Returns(list.Provider);
            mockSet.As<IQueryable<Movie>>().Setup(expression: m => m.ElementType).Returns(list.ElementType);
            mockContext.Setup(expression: db => db.Movies).Returns(mockSet.Object);

            // Arrange
            MoviesController controller = new MoviesController(mockContext.Object);

            // Act
            ViewResult result = controller.ListFromDb() as ViewResult;
            List<Movie> resultMovies = result.Model as List<Movie>;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void MovieStore_Details_Success()
        {
            //Goal: query from out own list instead of DB
            var list = new List<Movie>
            {
                new Movie() {MovieID = 1, Title = "Superman 1"},
                new Movie() {MovieID = 2, Title = "Superman 2"}
            }.AsQueryable();

            Mock<MovieStoreDBContext> mockContext = new Mock<MovieStoreDBContext>();
            Mock<DbSet<Movie>> mockSet = new Mock<DbSet<Movie>>();
            mockSet.As<IQueryable<Movie>>().Setup(expression: m => m.GetEnumerator()).Returns(list.GetEnumerator());
            mockSet.As<IQueryable<Movie>>().Setup(expression: m => m.Provider).Returns(list.Provider);
            mockSet.As<IQueryable<Movie>>().Setup(expression: m => m.ElementType).Returns(list.ElementType);
            mockSet.Setup(expression:m => m.Find(It.IsAny<Object>())).Returns(list.First());

            mockContext.Setup(expression: db => db.Movies).Returns(mockSet.Object);

            // Arrange
            MoviesController controller = new MoviesController(mockContext.Object);

            // Act
            ViewResult result = controller.Details(id:1) as ViewResult;
           

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void MovieStore_Details_IdIsNull()
        {
            //Goal: query from out own list instead of DB
            var list = new List<Movie>
            {
                new Movie() {MovieID = 1, Title = "Superman 1"},
                new Movie() {MovieID = 2, Title = "Superman 2"}
            }.AsQueryable();

            Mock<MovieStoreDBContext> mockContext = new Mock<MovieStoreDBContext>();
            Mock<DbSet<Movie>> mockSet = new Mock<DbSet<Movie>>();
            mockSet.As<IQueryable<Movie>>().Setup(expression: m => m.GetEnumerator()).Returns(list.GetEnumerator());
            mockSet.As<IQueryable<Movie>>().Setup(expression: m => m.Provider).Returns(list.Provider);
            mockSet.As<IQueryable<Movie>>().Setup(expression: m => m.ElementType).Returns(list.ElementType);
            mockSet.Setup(expression: m => m.Find(It.IsAny<Object>())).Returns(list.First());

            mockContext.Setup(expression: db => db.Movies).Returns(mockSet.Object);

            // Arrange
            MoviesController controller = new MoviesController(mockContext.Object);

            // Act
            HttpStatusCodeResult result = controller.Details(id: null) as HttpStatusCodeResult;


            // Assert
            Assert.AreEqual(expected: HttpStatusCode.BadRequest, actual: (HttpStatusCode)result.StatusCode);
        }

        [TestMethod]
        public void MovieStore_Details_MovieIsNull()
        {
            //Goal: query from out own list instead of DB
            var list = new List<Movie>
            {
                new Movie() {MovieID = 1, Title = "Superman 1"},
                new Movie() {MovieID = 2, Title = "Superman 2"}
            }.AsQueryable();

            Mock<MovieStoreDBContext> mockContext = new Mock<MovieStoreDBContext>();
            Mock<DbSet<Movie>> mockSet = new Mock<DbSet<Movie>>();
            mockSet.As<IQueryable<Movie>>().Setup(expression: m => m.GetEnumerator()).Returns(list.GetEnumerator());
            mockSet.As<IQueryable<Movie>>().Setup(expression: m => m.Provider).Returns(list.Provider);
            mockSet.As<IQueryable<Movie>>().Setup(expression: m => m.ElementType).Returns(list.ElementType);
            Movie movie = null;
            mockSet.Setup(expression: m => m.Find(It.IsAny<Object>())).Returns(movie);

            mockContext.Setup(expression: db => db.Movies).Returns(mockSet.Object);

            // Arrange
            MoviesController controller = new MoviesController(mockContext.Object);

            // Act
            HttpStatusCodeResult result = controller.Details(id: 0) as HttpStatusCodeResult;


            // Assert
            Assert.AreEqual(expected: HttpStatusCode.NotFound, actual: (HttpStatusCode)result.StatusCode);
        }
    }
}

