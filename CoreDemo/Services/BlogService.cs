using System;
using System.Collections.Generic;
using System.Data;
using CoreDemo.Models;
using CoreDemo.Repositories;

namespace CoreDemo.Services
{

  public class BlogsService
  {
    private readonly BlogsRepository _repo;
    public BlogsService(BlogsRepository repo)
    {
      _repo = repo;
    }

    internal IEnumerable<Blog> Get()
    {
      return _repo.Get();
    }
    internal Blog Get(int id)
    {
      return _repo.GetById(id);
    }

    internal Blog Create(Blog newBlog)
    {
      return _repo.Create(newBlog);
    }
  }
}