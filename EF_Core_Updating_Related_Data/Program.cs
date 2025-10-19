
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Reflection;

Console.WriteLine("Hello, World!");

ApplicationDbContext context = new();

#region One to One Iliskisel Senaryolarda Veri Guncelleme
#region Saving
//Person person = new()
//{
//    Name = "Cem",
//    Address = new() { PersonAddress = "Denizli" }
//};

//Person person2 = new() { Name = "Murat" };

//await context.AddAsync(person);
//await context.AddAsync(person2);
//await context.SaveChangesAsync();
#endregion

#region Example 1
//Person? person = await context.Persons
//    .Include(p=>p.Address)
//    .FirstOrDefaultAsync(p => p.Id == 1);

//person.Address.PersonAddress = "İZMİR";
//await context.SaveChangesAsync();
#endregion
#region Example 2
//Person? person = await context.Persons.FindAsync(4);
//person.Address = new() { PersonAddress = "BURSA" };
//await context.SaveChangesAsync();
#endregion
#endregion
#region One to Many Iliskisel Senaryolarda Veri Guncelleme
#region Saving
//Blog blog = new() { Name = "exampleBlog.com", Posts = new List<Post> { new() { Title = "Post1" }, new() { Title = "Post2" }, new() { Title = "Post3" } } };

//await context.Blogs.AddAsync(blog);
//await context.SaveChangesAsync();
#endregion

#region Example 1
//Blog? blog1 = await context.Blogs
//    .Include(b => b.Posts)
//    .FirstOrDefaultAsync(b => b.Id == 1);

//blog1.Posts.Add(new() { Title = "Post4" });

//await context.SaveChangesAsync();
#endregion
#region Example 2
//Post? post2 = await context.Posts.FindAsync(2);
//post2.Title = "Post2Updated";
//await context.SaveChangesAsync();

#endregion
#region Example 3
//Post? post2 = await context.Posts.FindAsync(2);

//Blog? blog = new() { Name = "testBlog.com" };
//blog.Posts.Add(post2);

//await context.Blogs.AddAsync(blog);
//await context.SaveChangesAsync();
#endregion
#region Example 4
//Blog? blog2 = await context.Blogs
//    .Include(b => b.Posts)
//    .FirstOrDefaultAsync(b => b.Id == 2);

//context.Posts.RemoveRange(blog2.Posts);
//await context.SaveChangesAsync();
#endregion
#region Example 5
//Post? post = await context.Posts.FindAsync(3);

//context.Posts.Remove(post);
//await context.SaveChangesAsync();
#endregion
#endregion
#region Many to Many Iliskisel Senaryolarda Veri Guncelleme
#region Saving
//Book book1 = new() { BookName = "Book 1" };
//Book book2 = new() { BookName = "Book 2" };
//Book book3 = new() { BookName = "Book 3" };

//Author author1 = new() { AuthorName = "Author 1" };
//Author author2 = new() { AuthorName = "Author 2" };
//Author author3 = new() { AuthorName = "Author 3" };

//book1.Authors.Add(author1);
//book1.Authors.Add(author2);

//book2.Authors.Add(author1);
//book2.Authors.Add(author2);
//book2.Authors.Add(author3);

//book3.Authors.Add(author3);

//await context.AddRangeAsync(book1, book2, book3);
//await context.SaveChangesAsync();
#endregion

#region Example 1
//Book? book1 = await context.Books
//    .Include(b => b.Authors)
//    .FirstOrDefaultAsync(b => b.Id == 1);

//Author? author3 = await context.Authors.FindAsync(3);

//book1.Authors.Add(author3);

//await context.SaveChangesAsync();
#endregion
#region Example 2
//Book? book2 = await context.Books
//    .Include(b => b.Authors)
//    .FirstOrDefaultAsync(b => b.Id == 2);

//Author? author1 = await context.Authors.FindAsync(1);

//book2.Authors.Remove(author1);
//await context.SaveChangesAsync();
#endregion
#region Example 3
//Book? book3 = await context.Books
//    .Include(b => b.Authors)
//    .FirstOrDefaultAsync(b => b.Id == 3);

//book3.Authors.Clear();

//Author? author1 = await context.Authors.FindAsync(1);
//Author? author2 = await context.Authors.FindAsync(2);

//book3.Authors.Add(author1);
//book3.Authors.Add(author2);

//await context.SaveChangesAsync();
#endregion
#region Example 4
//Book? book1 = await context.Books
//    .Include(b => b.Authors)
//    .FirstOrDefaultAsync(b => b.Id == 1);

//book1.Authors.Clear();

//Author? author3 = await context.Authors.FindAsync(3);

//book1.Authors.Add(author3);

//await context.SaveChangesAsync();
#endregion
#endregion


class Person
{
    public int Id { get; set; }
    public string Name { get; set; }
    public Address Address { get; set; }

}
class Address
{
    public int Id { get; set; }
    public string PersonAddress { get; set; }
    public Person Person { get; set; }

}
class Blog
{
    public Blog()
    {
        Posts = new HashSet<Post>();
    }
    public int Id { get; set; }
    public string Name { get; set; }
    public ICollection<Post> Posts { get; set; }
}
class Post
{
    public int Id { get; set; }
    public int BlogId { get; set; }
    public string Title { get; set; }
    public Blog Blog { get; set; }
}
class Book
{
    public Book()
    {
        Authors = new HashSet<Author>();
    }
    public int Id { get; set; }
    public string BookName { get; set; }
    public ICollection<Author> Authors { get; set; }
}
class Author
{
    public Author()
    {
        Books = new HashSet<Book>();
    }
    public int Id { get; set; }
    public string AuthorName { get; set; }
    public ICollection<Book> Books { get; set; }
}
class ApplicationDbContext : DbContext
{
    public DbSet<Person> Persons { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<Blog> Blogs { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Book> Books { get; set; }
    public DbSet<Author> Authors { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=DESKTOP-V99G48T;Database=ApplicationDb;Trusted_Connection=True;TrustServerCertificate=True;");
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Address>()
             .HasOne(a => a.Person)
             .WithOne(p => p.Address)
             .HasForeignKey<Address>(a => a.Id);
    }
}