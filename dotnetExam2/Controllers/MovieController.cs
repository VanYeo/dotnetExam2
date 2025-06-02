using AutoMapper;
using dotnetExam2.DTOs;
using dotnetExam2.Models;
using dotnetExam2.Persistence;
using dotnetExam2.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace dotnetExam2.Controllers
{   
    [Route("api/[controller]")]
    [ApiController]

    public class MovieController : ControllerBase
    {
        private readonly MovieDbContext dbContext;
        private readonly MovieService movieService;
        private readonly IMapper mapper;

        public MovieController(MovieDbContext dbContext, 
            IMapper mapper, MovieService movieService)
        {
            this.dbContext = dbContext;
            this.mapper=mapper;
            this.movieService = movieService;
        }

        // GET: /api/movie?filterOn=Title&sortBy=Rating&isAscending=true
        [HttpGet]
        [Authorize(Roles = "Writer,Reader")]
        public async Task<IActionResult> GetAllMovies(
            [FromQuery] string? filterOn, [FromQuery] string? filterQuery, 
            [FromQuery] string? sortBy, [FromQuery] bool? isAscending,
            [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 1000)
        {
            var moviesDomain = await movieService.GetAllMoviesAsync(filterOn, filterQuery, sortBy, isAscending, pageNumber, pageSize);
            return Ok(mapper.Map<List<MovieDto>>(moviesDomain));
        }

        // get movie by id
        // GET: /api/movie/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer,Reader")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var movieDomain = await movieService.GetByIdAsync(id);
            if (movieDomain == null)
                return NotFound();
            return Ok(mapper.Map<MovieDto>(movieDomain));
        }


        // post: create new movie
        // POST: /api/movie
        [HttpPost]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Create([FromBody] CreateMovieDto createMovieDto)
        {
            var movieDomainModel = mapper.Map<Movie>(createMovieDto);
            var createdMovie = await movieService.CreateAsync(
                movieDomainModel.Title,
                movieDomainModel.Genre,
                movieDomainModel.ReleaseDate,
                movieDomainModel.Rating);

            var movieDto = mapper.Map<MovieDto>(createdMovie);
            return CreatedAtAction(nameof(GetById), new { id = movieDto.Id }, movieDto);
        }


        // put: update movie
        // PUT: /api/movie/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateMovieDto updateMovieDto)
        {
            var movieDomainModel = mapper.Map<Movie>(updateMovieDto);

            movieDomainModel = await movieService.UpdateAsync(id, movieDomainModel);

            //convert domain model to dto
            return Ok(mapper.Map<MovieDto>(movieDomainModel));
        }


        // delete: delete movie
        // DELETE: /api/movie/{id}
        [HttpDelete]
        [Route("{id:guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var movieDomainModel = await movieService.DeleteAsync(id);

            if (movieDomainModel == null)
            {
                return NotFound();
            } 

            return Ok(); 
        }
    }
}
