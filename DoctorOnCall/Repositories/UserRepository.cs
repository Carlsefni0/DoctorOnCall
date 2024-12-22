// using DoctorOnCall.Models;
// using DoctorOnCall.RepositoryInterfaces;
// using Microsoft.EntityFrameworkCore;
//
// namespace DoctorOnCall.Repositories;
//
// public class UserRepository : IUserRepository
// {
//     private readonly DataContext _db;
//
//     public UserRepository(DataContext db) => _db = db;
//
//     public async Task<UserModel> AddUser(UserModel user)
//     {
//         var createdUser = await _db.Users.AddAsync(user);
//         await _db.SaveChangesAsync();
//         return createdUser.Entity;
//     }
//
//     public async Task<UserModel> UpdateUser(UserModel user)
//     {
//         var updatedUser = _db.Users.Update(user);
//
//         await _db.SaveChangesAsync();
//         return  updatedUser.Entity;
//     }
//
//     public async Task<bool> DeleteUser(int id)
//     {
//         var user = await _db.Users.FirstOrDefaultAsync(user => user.Id == id);
//
//         if (user == null) return false;
//         
//         _db.Users.Remove(user);
//         await _db.SaveChangesAsync();
//
//         return true;
//     }
//     
//     public async Task<UserModel?> GetUserByEmail(string email)
//     {
//         return await _db.Users.FirstOrDefaultAsync(u => u.Email == email);
//     }
//     
//
// }