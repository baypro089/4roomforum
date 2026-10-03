using System;
using MicroServices.UserServices.Data;
using UserServices.DTOs;
using BCrypt.Net;

namespace UserServices.Data;

public class UserRepo : IUserRepo

{
    private readonly AppDBContext _context;

    public UserRepo(AppDBContext context)
    {
        _context = context;
    }
    public void CreateUser(User user)
    {
        if (user == null){
            throw new ArgumentNullException(nameof(user));

        }

        user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
        _context.Users.Add(user);
        _context.SaveChanges();
    }

    public void DeleteUser(int id)
    {
        var user = _context.Users.FirstOrDefault(c => c.UserId == id);
        if (user == null){
            throw new ArgumentNullException(nameof(user));
        }
        _context.Users.Remove(user);
        _context.SaveChanges();
    }

    public IEnumerable<User> GetAllUsers()
    {
        return _context.Users.ToList();
    }

    public User GetUserById(int id)
    {
        return _context.Users.FirstOrDefault(c => c.UserId == id);
    }

    public User GetUserByEmail(string email)
    {
        return _context.Users.FirstOrDefault(u => u.Email == email);
    }

    public void UpdateUser(User user)
    {
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        if (!string.IsNullOrWhiteSpace(user.Password) &&
            !user.Password.StartsWith("$2"))
        {
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
        }

        _context.Users.Update(user);
        _context.SaveChanges();
    }
    public bool SaveChanges(){
        return(_context.SaveChanges() >= 0);
    }
    public User Login(UserLoginDTO userLogin)
    {
        try{
        var user = _context.Users.FirstOrDefault(c => 
        c.Email == userLogin.Email);
        bool isPasswordValid = BCrypt.Net.BCrypt.Verify(userLogin.Password, user.Password);
         if (!isPasswordValid)
        {
            throw new ArgumentException("Invalid email or password");
        }
        user.LastLogin=DateOnly.FromDateTime(DateTime.Now);
        UpdateUser(user);
        return user;
        }
        catch (ArgumentException e){
            throw new ArgumentException("Invalid email or password");
        }
        
    }
}
