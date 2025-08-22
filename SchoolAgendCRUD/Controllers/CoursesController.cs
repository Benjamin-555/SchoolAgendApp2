using Microsoft.AspNetCore.Mvc;
using SchoolAgend.Domain.Entities;
using SchoolAgendCRUD.DTOs;
using SchoolAgend.Infrastructure.Data.Repositories;
using System.Threading.Tasks;
using SchoolAgend.Application.Contracts;
using SchoolAgendCRUD.DTOs;
using SchoolAgend.Application.Services;

namespace SchoolAgendCRUD.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseRepository _courseRepository;

        public CoursesController(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetCourses()
        {
            var courses = await _courseRepository.GetAllAsync();
            return Ok(courses);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCourseById(int id)
        {
            var course = await _courseRepository.GetByIdAsync(id);
            if (course == null) return NotFound();
            return Ok(course);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCourse([FromBody] Course course)
        {
            if (course == null || string.IsNullOrWhiteSpace(course.Name))
                return BadRequest("Invalid course data.");

            await _courseRepository.AddAsync(course);
            return Created($"/api/courses/{course.Id}", course);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCourse(int id, [FromBody] Course course)
        {
            if (course == null || id <= 0)
                return BadRequest("Invalid course data.");

            if (id != course.Id)
                return BadRequest("Course ID mismatch.");

            var existingCourse = await _courseRepository.GetByIdAsync(id);
            if (existingCourse == null)
                return NotFound($"Course with ID {id} not found.");

            existingCourse.Name = course.Name;
            existingCourse.Teacher = course.Teacher;
            existingCourse.Color = course.Color;

            await _courseRepository.UpdateAsync(existingCourse);
            return Ok(existingCourse);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var course = await _courseRepository.GetByIdAsync(id);
            if (course == null) return NotFound($"Course with ID {id} not found.");

            await _courseRepository.DeleteAsync(course);
            return NoContent();
        }
    }
}
