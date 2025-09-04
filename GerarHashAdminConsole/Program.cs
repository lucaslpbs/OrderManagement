using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNetCore.Identity;

var hasher = new PasswordHasher<IdentityUser>();
var user = new IdentityUser();
string hash = hasher.HashPassword(user, "Admin@123"); // senha desejada
Console.WriteLine(hash);
