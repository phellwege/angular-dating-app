using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace API.Controllers;
    [ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly DataContext context;
    public UsersController(DataContext context)
    {
        this.context = context;
    }
    [HttpGet]
    public ActionResult<IEnumerable<AppUser>> GetUsers()
    {
        return context.Users.ToList();
    }
    [HttpGet("{id}")]
    public ActionResult<AppUser> GetUser(int id)
    {
        return context.Users.Find(id);
    }
}
