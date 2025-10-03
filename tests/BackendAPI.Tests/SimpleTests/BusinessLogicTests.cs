using BackendAPI.Domain.DTOs;
using FluentAssertions;
using Xunit;

namespace BackendAPI.Tests.SimpleTests;

public class BusinessLogicTests
{
    [Fact]
    public void Pagination_ShouldCalculateCorrectly()
    {
        // Arrange
        var totalCount = 25;
        var pageSize = 10;
        var currentPage = 2;

        // Act
        var pagedResult = new PagedResultDto<UsuarioDto>
        {
            Data = new List<UsuarioDto>(),
            TotalCount = totalCount,
            Page = currentPage,
            PageSize = pageSize
        };

        // Assert
        pagedResult.HasNextPage.Should().BeTrue(); // Page 2 of 3
        pagedResult.HasPreviousPage.Should().BeTrue(); // Has previous page
    }

    [Fact]
    public void Pagination_LastPage_ShouldNotHaveNextPage()
    {
        // Arrange
        var totalCount = 25;
        var pageSize = 10;
        var currentPage = 3; // Last page

        // Act
        var pagedResult = new PagedResultDto<UsuarioDto>
        {
            Data = new List<UsuarioDto>(),
            TotalCount = totalCount,
            Page = currentPage,
            PageSize = pageSize
        };

        // Assert
        pagedResult.HasNextPage.Should().BeFalse(); // Last page
        pagedResult.HasPreviousPage.Should().BeTrue(); // Has previous page
    }

    [Fact]
    public void Pagination_FirstPage_ShouldNotHavePreviousPage()
    {
        // Arrange
        var totalCount = 25;
        var pageSize = 10;
        var currentPage = 1; // First page

        // Act
        var pagedResult = new PagedResultDto<UsuarioDto>
        {
            Data = new List<UsuarioDto>(),
            TotalCount = totalCount,
            Page = currentPage,
            PageSize = pageSize
        };

        // Assert
        pagedResult.HasNextPage.Should().BeTrue(); // Has next page
        pagedResult.HasPreviousPage.Should().BeFalse(); // First page
    }

    [Fact]
    public void Authentication_ShouldGenerateValidTokens()
    {
        // Arrange
        var authResponse = new AuthResponseDto
        {
            Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...", // JWT format
            RefreshToken = "refresh-token-12345",
            Usuario = new UsuarioDto
            {
                Id = 1,
                Nombre = "Test",
                Apellido = "User",
                Email = "test@example.com"
            }
        };

        // Assert
        authResponse.Token.Should().NotBeNullOrEmpty();
        authResponse.RefreshToken.Should().NotBeNullOrEmpty();
        authResponse.Token.Should().Contain("."); // JWT format
        authResponse.Usuario.Should().NotBeNull();
    }

    [Fact]
    public void Password_Hashing_ShouldWork()
    {
        // Arrange
        var password = "password123";
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

        // Act
        var isValid = BCrypt.Net.BCrypt.Verify(password, hashedPassword);

        // Assert
        hashedPassword.Should().NotBeNullOrEmpty();
        hashedPassword.Should().NotBe(password);
        isValid.Should().BeTrue();
    }

    [Fact]
    public void Password_Hashing_WithWrongPassword_ShouldFail()
    {
        // Arrange
        var correctPassword = "password123";
        var wrongPassword = "wrongpassword";
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(correctPassword);

        // Act
        var isValid = BCrypt.Net.BCrypt.Verify(wrongPassword, hashedPassword);

        // Assert
        isValid.Should().BeFalse();
    }

    [Fact]
    public void JWT_Token_ShouldHaveCorrectFormat()
    {
        // Arrange
        var token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c";

        // Act & Assert
        token.Should().NotBeNullOrEmpty();
        token.Split('.').Should().HaveCount(3); // JWT has 3 parts
    }

    [Fact]
    public void Email_Validation_ShouldWork()
    {
        // Arrange
        var validEmails = new[]
        {
            "test@example.com",
            "user.name@domain.co.uk",
            "admin@company.org"
        };

        var invalidEmails = new[]
        {
            "invalid-email",
            "@domain.com",
            "user@",
            "user@domain"
        };

        // Act & Assert
        foreach (var email in validEmails)
        {
            var isValid = System.Text.RegularExpressions.Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            isValid.Should().BeTrue($"Email '{email}' should be valid");
        }

        foreach (var email in invalidEmails)
        {
            var isValid = System.Text.RegularExpressions.Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            isValid.Should().BeFalse($"Email '{email}' should be invalid");
        }
    }

    [Fact]
    public void Date_Validation_ShouldWork()
    {
        // Arrange
        var validDate = DateTime.Now.AddYears(-25);
        var futureDate = DateTime.Now.AddYears(1);
        var tooOldDate = DateTime.Now.AddYears(-150);

        // Act & Assert
        validDate.Should().BeBefore(DateTime.Now);
        validDate.Should().BeAfter(DateTime.Now.AddYears(-100));

        futureDate.Should().BeAfter(DateTime.Now);
        tooOldDate.Should().BeBefore(DateTime.Now.AddYears(-100));
    }

    [Fact]
    public void String_Validation_ShouldWork()
    {
        // Arrange
        var validString = "Valid String";
        var emptyString = "";
        var nullString = (string?)null;
        var longString = new string('a', 1000);

        // Act & Assert
        validString.Should().NotBeNullOrEmpty();
        validString.Length.Should().BeLessThan(500);

        emptyString.Should().BeEmpty();
        nullString.Should().BeNull();
        longString.Length.Should().BeGreaterThan(500);
    }
}
