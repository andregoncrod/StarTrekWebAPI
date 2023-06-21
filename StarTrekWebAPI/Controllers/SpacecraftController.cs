using Microsoft.AspNetCore.Mvc;
using StarTrekWebAPI.Models;
using System.Linq.Dynamic.Core;

namespace StarTrekWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpacecraftController : ControllerBase
    {
        private readonly StarTrekContext _dbContext;

        public SpacecraftController(StarTrekContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public IEnumerable<Spacecraft> GetAll()
        {
            return _dbContext.Spacecrafts.Where(s => !s.Deleted).ToList();
        }

        [HttpGet("{id}")]
        public ActionResult<Spacecraft> GetSingle(string id)
        {
            var foundSpacecraft = _dbContext.Spacecrafts?.FirstOrDefault(s => s.Uid == id && !s.Deleted);
            return foundSpacecraft == null ? NotFound() : foundSpacecraft;
        }

        [HttpGet("paged/{pageNumber}/{pageSize}/{orderColumn}/{orderDir}/{searchValue?}")]
        public SpacecraftsPagedDto GetPaged(int pageNumber, int pageSize, string orderColumn, string orderDir, string? searchValue = null)
        {
            IQueryable<Spacecraft> result = null;
            var total = _dbContext.Spacecrafts.Where(s => !s.Deleted).Count();
            if (!string.IsNullOrWhiteSpace(searchValue))
            {
                var search = searchValue.Trim().ToLower();
                result = _dbContext.Spacecrafts.Where(s => !s.Deleted).Where(
                    s => s.Name.ToLower().Contains(search) || s.Registry.ToLower().Contains(search) || s.Status.ToLower().Contains(search) || s.DateStatus.ToLower().Contains(search)).OrderBy($"{orderColumn} {orderDir}");
            }
            else
            {
                result = _dbContext.Spacecrafts.Where(s => !s.Deleted).OrderBy($"{orderColumn} {orderDir}");
            }

            return new SpacecraftsPagedDto()
            {
                Total = total,
                Filtered = !string.IsNullOrWhiteSpace(searchValue) ? result.Count() : total,
                Results = result.Skip((pageNumber - 1) * pageSize).Take(pageSize)
            };
        }

        [HttpPost]
        public ActionResult<bool> Post([FromBody] SpacecraftDto spacecraft)
        {
            try
            {
                var newSpacecraft = new Spacecraft()
                {
                    Uid = Guid.NewGuid().ToString(),
                    Name = spacecraft.Name,
                    Registry = spacecraft.Registry,
                    Status = spacecraft.Status,
                    DateStatus = spacecraft.DateStatus,
                    SystemDate = DateTime.Now,
                    Deleted = false
                };

                _dbContext.Spacecrafts.Add(newSpacecraft);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error creating spacecraft ({spacecraft.Name}) on db => {ex}");
            }

            return true;
        }

        [HttpPut("{id}")]
        public ActionResult<bool> Put(string id, SpacecraftDto spacecraft)
        {
            try
            {
                var spacecraftToUpdate = _dbContext.Spacecrafts.FirstOrDefault(s => s.Uid == id && !s.Deleted);
                if (spacecraftToUpdate == null)
                    return NotFound();
                spacecraftToUpdate.Name = spacecraft.Name;
                spacecraftToUpdate.Registry = spacecraft.Registry;
                spacecraftToUpdate.Status = spacecraft.Status;
                spacecraftToUpdate.DateStatus = spacecraft.DateStatus;
                spacecraftToUpdate.LastChange = DateTime.Now;
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error updating spacecraft ({spacecraft.Name}) on db => {ex}");
            }

            return true;
        }

        [HttpDelete("{id}")]
        public ActionResult<bool> Delete(string id)
        {
            var spacecraftToDelete = _dbContext.Spacecrafts.FirstOrDefault(s => s.Uid == id && !s.Deleted);
            if (spacecraftToDelete == null)
                return NotFound();

            try
            {
                spacecraftToDelete.Deleted = true;
                spacecraftToDelete.LastChange = DateTime.Now;
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error deleting spacecraft ({spacecraftToDelete.Name}) from db => {ex}");
            }

            return true;
        }
    }
}
