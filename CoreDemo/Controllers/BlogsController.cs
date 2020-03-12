using System;
using System.Collections.Generic;
using System.Security.Claims;
using CoreDemo.Models;
using CoreDemo.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoreDemo.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class BlogsController : ControllerBase
  {
    private readonly BlogsService _bs;
    public BlogsController(BlogsService bs)
    {
      _bs = bs;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Blog>> Get()
    {
      try
      {
        return Ok(_bs.Get());
      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      };
    }

    [HttpGet("{id}")]
    public ActionResult<Blog> Get(int id)
    {
      try
      {
        return Ok(_bs.Get(id));
      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      };
    }

    [HttpPost]
    [Authorize]
    public ActionResult<Blog> Post([FromBody] Blog newBlog)
    {
      try
      {
        var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        newBlog.CreatorId = userId;
        return Ok(_bs.Create(newBlog));
      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }

  }
}