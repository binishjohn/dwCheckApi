using System.Linq;
using System.Net;
using System.Threading.Tasks;
using dwCheckApi.DAL;
using dwCheckApi.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace dwCheckApi.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class DatabaseController : BaseController
    {
        private readonly IConfiguration _configuration;
        private readonly IDatabaseService _databaseService;
        
        public DatabaseController(IConfiguration configuration, IDatabaseService databaseService)
        {
            _configuration = configuration;
            _databaseService = databaseService;
        }

        /// <summary>
        /// Used to Seed the Database (using JSON files which are included with the application)
        /// </summary>
        /// <returns>
        /// A <see cref="BaseController.MessageResult"/> with the number of entities which
        /// were added to the database.
        /// </returns>
        [HttpGet("SeedData")]
        public IActionResult SeedData()
        {
            var entitiesAdded = _databaseService.SeedDatabase();

            return Ok($"Number of new entities added: {entitiesAdded}");

        }

        /// <summary>
        /// Used to drop all current data from the database and recreate any tables
        /// </summary>
        /// <param name="secret">
        /// A passphrase like secret to ensure that a Drop Data action should take place
        /// </param>
        /// <returns>
        /// A <see cref="BaseController.MessageResult"/>
        /// </returns>
        [HttpDelete("DropData")]
        public IActionResult DropData(string secret = null)
        {
            if (SecretChecker.CheckUserSuppliedSecretValue(secret,
                _configuration["dropDatabaseSecretValue"]))
            {
                return Unauthorized("Incorrect secret");
            }

            var success = _databaseService.ClearDatabase();

            if (!success)
            {
             return BadRequest("Unable to drop database");   
            }

            return Ok("Database tabled dropped and recreated");
        }

        /// <summary>
        /// Used to prepare and apply all Book cover art (as Base64 strings)
        /// </summary>
        /// <returns>
        /// A <see cref="BaseController.MessageResult"/> with the number of entities which were altered.
        /// </returns>
        [HttpGet("ApplyBookCoverArt")]
        public async Task<IActionResult> ApplyBookCoverArt()
        {
            var relevantBooks = _databaseService.BooksWithoutCoverBytes();

            using (var webClient = new WebClient())
            {
                foreach (var book in relevantBooks.ToList())
                {
                    var coverData = webClient.DownloadData(book.BookCoverImageUrl);
                    book.BookCoverImage = coverData;
                }
            }

            var updatedRecordCount = await _databaseService.SaveAnyChanges();

            return Ok($"{updatedRecordCount} entities updated");
        }
    }
}