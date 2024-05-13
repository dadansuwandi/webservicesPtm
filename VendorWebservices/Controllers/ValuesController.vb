Imports System.Data.SqlClient
Imports System.IdentityModel.Tokens.Jwt
Imports System.Net
Imports System.Net.Http
Imports System.Security.Claims
Imports System.Security.Cryptography
Imports System.Web.Http
Imports Microsoft.IdentityModel.Tokens
Imports Newtonsoft.Json

Public Class ValuesController
    Inherits ApiController

    ' GET api/values
    <HttpGet>
    <Route("api/getvalue/{NoVendor}")>
    Public Function GetValues(ByVal NoVendor As String) As HttpResponseMessage
        Try
            Dim jsonString As String = ""
            Dim token As String = Request.Headers.Authorization.Parameter

            ' Check if the token is valid
            If IsValidToken(token) = False Then

                Return Request.CreateResponse(HttpStatusCode.Unauthorized, "Invalid token")

            End If
            Dim connectionString As String = ConfigurationManager.ConnectionStrings("DefaultConnection").ConnectionString



            Using connection As New SqlConnection(connectionString)

                Using command As New SqlCommand("UIDESK_TrmDropdown", connection)
                    command.CommandType = CommandType.StoredProcedure

                    ' Add parameter to the command
                    command.Parameters.AddWithValue("@TrxID", NoVendor)
                    command.Parameters.AddWithValue("@TrxUserName", "")
                    command.Parameters.AddWithValue("@TrxAction", "UIDESK159")

                    connection.Open()
                    Dim reader As SqlDataReader = command.ExecuteReader()
                    If reader IsNot Nothing AndAlso reader.HasRows Then
                        While reader.Read()

                            Dim jsonObject = New With {
                                    .status = 200,
                                    .message = "success",
                                    .data = New With {
                                        .Name = reader("Name").ToString(),
                                        .Alamat = reader("AlamatHTML").ToString(),
                                        .Email = reader("Email").ToString(),
                                        .NoHp = reader("HP").ToString()
                             }
                        }

                            ' Serialize the object to JSON
                            jsonString = JsonConvert.SerializeObject(jsonObject, Formatting.Indented)


                        End While
                    Else
                        Dim jsonObject = New With {
                                    .status = 201,
                                    .message = "data empty"
                                    }

                        jsonString = JsonConvert.SerializeObject(jsonObject, Formatting.Indented)

                    End If

                    connection.Close()
                    ' Load data into the DataTable

                End Using

            End Using
            ' Create an HttpResponseMessage with JSON content
            Dim response = New HttpResponseMessage(HttpStatusCode.OK)
            response.Content = New StringContent(jsonString, System.Text.Encoding.UTF8, "application/json")

            Return response
        Catch ex As Exception
            ' Log the exception
            Dim jsonObject = New With {
                                    .status = 401,
                                    .message = "error"
                                    }
            Dim response = New HttpResponseMessage(HttpStatusCode.InternalServerError)
            Dim jsonString = JsonConvert.SerializeObject(jsonObject, Formatting.Indented)
            response.Content = New StringContent(jsonString, System.Text.Encoding.UTF8, "application/json")
            Return response
        End Try

    End Function
    <HttpGet>
    <Route("api/getCustomerChannel/{ValueChannel}")>
    Public Function getCustomerChannel(ByVal ValueChannel As String) As HttpResponseMessage
        Try

            Dim token As String = Request.Headers.Authorization.Parameter

            ' Check if the token is valid
            If IsValidToken(token) = False Then

                Return Request.CreateResponse(HttpStatusCode.Unauthorized, "Invalid token")

            End If
            Dim connectionString As String = ConfigurationManager.ConnectionStrings("DefaultConnection").ConnectionString

            Dim jsonString As String = ""

            Using connection As New SqlConnection(connectionString)

                Using command As New SqlCommand("UIDESK_TrmDropdown", connection)
                    command.CommandType = CommandType.StoredProcedure

                    ' Add parameter to the command
                    command.Parameters.AddWithValue("@TrxID", ValueChannel)
                    command.Parameters.AddWithValue("@TrxUserName", "")
                    command.Parameters.AddWithValue("@TrxAction", "UIDESK165")

                    connection.Open()
                    Dim reader As SqlDataReader = command.ExecuteReader()
                    If reader IsNot Nothing AndAlso reader.HasRows Then
                        While reader.Read()

                            Dim jsonObject = New With {
                                    .status = 200,
                                    .message = "success",
                                    .data = New With {
                                        .Name = reader("Name").ToString(),
                                        .Alamat = reader("AlamatHTML").ToString(),
                                        .Email = reader("Email").ToString(),
                                        .NoHp = reader("HP").ToString()
                             }
                        }

                            ' Serialize the object to JSON
                            jsonString = JsonConvert.SerializeObject(jsonObject, Formatting.Indented)


                        End While
                    Else
                        Dim jsonObject = New With {
                                    .status = 201,
                                    .message = "data empty"
                                    }

                        jsonString = JsonConvert.SerializeObject(jsonObject, Formatting.Indented)

                    End If

                    connection.Close()
                    ' Load data into the DataTable

                End Using

            End Using
            ' Create an HttpResponseMessage with JSON content
            Dim response = New HttpResponseMessage(HttpStatusCode.OK)
            response.Content = New StringContent(jsonString, System.Text.Encoding.UTF8, "application/json")

            Return response
        Catch ex As Exception
            ' Log the exception
            Dim jsonObject = New With {
                                    .status = 401,
                                    .message = "error"
                                    }
            Dim response = New HttpResponseMessage(HttpStatusCode.InternalServerError)
            Dim jsonString = JsonConvert.SerializeObject(jsonObject, Formatting.Indented)
            response.Content = New StringContent(jsonString, System.Text.Encoding.UTF8, "application/json")
            Return response
        End Try

    End Function
    Public Function IsValidToken(ByVal Token As String) As Boolean
        Try
            Dim connectionString As String = ConfigurationManager.ConnectionStrings("DefaultConnection").ConnectionString

            Dim jsonString As String = ""

            Using connection As New SqlConnection(connectionString)

                Using command As New SqlCommand("UIDESK_TrmDropdown", connection)
                    command.CommandType = CommandType.StoredProcedure

                    ' Add parameter to the command
                    command.Parameters.AddWithValue("@TrxID", Token)
                    command.Parameters.AddWithValue("@TrxUserName", "")
                    command.Parameters.AddWithValue("@TrxAction", "UIDESK167")

                    connection.Open()
                    Dim reader As SqlDataReader = command.ExecuteReader()
                    If reader IsNot Nothing AndAlso reader.HasRows Then
                        Return True
                    Else
                        Return False

                    End If

                    connection.Close()
                    ' Load data into the DataTable

                End Using

            End Using



        Catch ex As Exception
            ' Log the exception

        End Try

    End Function
    Public Shared Function GenerateToken(username As String) As String
        ' Key rahasia untuk menandatangani token JWT (harus dijaga kerahasiaannya)

        Dim secretKey As Byte() = New Byte(15) {} ' 128 bits
        Dim rng As New RNGCryptoServiceProvider()
        rng.GetBytes(secretKey)

        Dim securityKey As New SymmetricSecurityKey(secretKey)



        Dim tokenDescriptor As New SecurityTokenDescriptor() With {
            .Subject = New ClaimsIdentity(New Claim() {
                New Claim(ClaimTypes.Name, username)
            }),
            .SigningCredentials = New SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature)
}

        ' Membuat token JWT dengan token descriptor
        Dim tokenHandler As New JwtSecurityTokenHandler()
        Dim token As SecurityToken = tokenHandler.CreateToken(tokenDescriptor)

        ' Menghasilkan token JWT sebagai string
        Return tokenHandler.WriteToken(token)
    End Function
    Public Class ValueItem
        Public Property Name As String
        Public Property Alamat As String
        Public Property Email As String
        Public Property NoHp As String

    End Class
    Public Class UserModel
        Public Property userName As String
        Public Property Password As String


    End Class

    ' GET api/values/5
    Public Function GetValue(ByVal id As Integer) As String
        Return "value"
    End Function

    ' POST api/values
    Public Sub PostValue(<FromBody()> ByVal value As String)

    End Sub

    ' PUT api/values/5
    Public Sub PutValue(ByVal id As Integer, <FromBody()> ByVal value As String)

    End Sub

    ' DELETE api/values/5
    Public Sub DeleteValue(ByVal id As Integer)

    End Sub
    <HttpPost>
    <Route("api/PostToken")>
    Public Function GetTokenPostValue(<FromBody> model As UserModel) As HttpResponseMessage


        Dim jsonString As String = ""

        ' You can return the token as part of a response, for example, as JSON

        Dim tkn = GenerateToken(model.userName)
        Dim connectionString As String = ConfigurationManager.ConnectionStrings("DefaultConnection").ConnectionString

        Using connection As New SqlConnection(connectionString)
            connection.Open()

            ' Create SqlCommand object for the stored procedure
            Dim command As New SqlCommand("InsertToken", connection)
            command.CommandType = CommandType.StoredProcedure

            ' Add parameter for the token
            command.Parameters.AddWithValue("@UserName", model.userName)
            command.Parameters.AddWithValue("@Token", tkn)

            ' Execute the stored procedure
            command.ExecuteNonQuery()

            Console.WriteLine("Token inserted successfully.")
        End Using



        Dim jsonObject = New With {
                                    .status = 200,
                                    .message = "success",
                                    .data = New With {
                                        .Jwt = tkn
                                     }
        }


        jsonString = JsonConvert.SerializeObject(jsonObject, Formatting.Indented)
        Dim response = New HttpResponseMessage(HttpStatusCode.OK)
        response.Content = New StringContent(jsonString, System.Text.Encoding.UTF8, "application/json")

        Return response
    End Function
End Class
