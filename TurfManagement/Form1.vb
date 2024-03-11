Imports MySql.Data.MySqlClient

Public Class Form1
    Dim connString As String = "server=localhost;user id=root;password=admin;database=TurfManagement"
    Dim conn As New MySqlConnection(connString)

    Private Sub Guna2GradientButton1_Click(sender As Object, e As EventArgs) Handles Guna2GradientButton1.Click
        Dim username As String = Guna2TextBox1.Text.Trim()
        Dim password As String = Guna2TextBox2.Text.Trim()

        Try
            conn.Open()

            Dim query As String = "SELECT * FROM Admin WHERE username=@username AND password=@password"
            Dim cmd As New MySqlCommand(query, conn)
            cmd.Parameters.AddWithValue("@username", username)
            cmd.Parameters.AddWithValue("@password", password)

            Dim reader As MySqlDataReader = cmd.ExecuteReader()

            If reader.Read() Then
                MessageBox.Show("Login successful!")
                Form2.Show()
                Me.Hide()
            Else
                MessageBox.Show("Invalid username or password")
            End If

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        Finally
            conn.Close()
        End Try
    End Sub
End Class

