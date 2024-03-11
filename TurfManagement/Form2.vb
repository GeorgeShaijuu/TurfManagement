Imports System.Text
Imports MySql.Data.MySqlClient

Public Class Form2
    Private connectionString As String = "Server=localhost;Database=TurfManagement;User ID=root;Password=admin;"

    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Initialize controls visibility
        Guna2Button4.Visible = False
        Guna2GroupBox2.Visible = False
        Guna2Button5.Visible = False
        Guna2GroupBox3.Visible = False

        ' Add sample items to Guna2ComboBox1 (TurfType)
        Guna2ComboBox1.Items.Add("Football")
        Guna2ComboBox1.Items.Add("Soccer")
        ' Add more options as needed

        ' Add sample items to Guna2ComboBox2 (TurfCity)
        Guna2ComboBox2.Items.Add("Bangalore")
        Guna2ComboBox2.Items.Add("Kochi")
        ' Add more options as needed
    End Sub

    Private Sub Guna2Button1_Click(sender As Object, e As EventArgs) Handles Guna2Button1.Click
        Guna2Button4.Visible = True
    End Sub

    Private Sub Guna2Button4_Click(sender As Object, e As EventArgs) Handles Guna2Button4.Click
        Guna2GroupBox2.Visible = True
    End Sub

    Private Sub Guna2Button12_Click(sender As Object, e As EventArgs) Handles Guna2Button12.Click
        Dim turfname As String = Guna2TextBox1.Text.Trim()
        Dim turfType As String = Guna2ComboBox1.SelectedItem.ToString()
        Dim turfContactno As String = Guna2TextBox4.Text.Trim()
        Dim costPerHour As Decimal = Convert.ToDecimal(Guna2TextBox2.Text.Trim())
        Dim turfCity As String = Guna2ComboBox2.SelectedItem.ToString()

        Try
            Using connection As New MySqlConnection(connectionString)
                connection.Open()

                Dim query As String = "INSERT INTO Turfs (turfname, TurfType, TurfContactno, Costperhour, TurfCity) VALUES (@turfname, @turfType, @turfContactno, @costPerHour, @turfCity)"
                Using cmd As New MySqlCommand(query, connection)
                    cmd.Parameters.AddWithValue("@turfname", turfname)
                    cmd.Parameters.AddWithValue("@turfType", turfType)
                    cmd.Parameters.AddWithValue("@turfContactno", turfContactno)
                    cmd.Parameters.AddWithValue("@costPerHour", costPerHour)
                    cmd.Parameters.AddWithValue("@turfCity", turfCity)

                    cmd.ExecuteNonQuery()

                    MessageBox.Show("Turf added successfully!")
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        End Try
    End Sub

    Private Sub Guna2Button2_Click(sender As Object, e As EventArgs) Handles Guna2Button2.Click
        Guna2Button5.Visible = True
        Guna2GroupBox3.Visible = True
    End Sub

    Private Sub Guna2Button5_Click(sender As Object, e As EventArgs) Handles Guna2Button5.Click
        ' Load Turfs data to Guna2DataGridView1
        LoadTurfData()
    End Sub

    Private Sub LoadTurfData()
        Try
            Using connection As New MySqlConnection(connectionString)
                connection.Open()

                Dim query As String = "SELECT * FROM Turfs"
                Using adapter As New MySqlDataAdapter(query, connection)
                    Dim dataTable As New DataTable()
                    adapter.Fill(dataTable)

                    ' Assuming Guna2DataGridView1 is the name of your DataGridView
                    Guna2DataGridView1.DataSource = dataTable
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        End Try
    End Sub

    Private Sub Guna2Button9_Click(sender As Object, e As EventArgs) Handles Guna2Button9.Click
        ' Remove selected turf details
        RemoveTurfDetails()
    End Sub

    Private Sub RemoveTurfDetails()
        ' Assuming Guna2DataGridView1 is the name of your DataGridView
        If Guna2DataGridView1.SelectedRows.Count > 0 Then
            Dim selectedTurfId As Integer = Convert.ToInt32(Guna2DataGridView1.SelectedRows(0).Cells("turfId").Value)

            Try
                Using connection As New MySqlConnection(connectionString)
                    connection.Open()

                    Dim query As String = "DELETE FROM Turfs WHERE turfId = @turfId"
                    Using cmd As New MySqlCommand(query, connection)
                        cmd.Parameters.AddWithValue("@turfId", selectedTurfId)

                        cmd.ExecuteNonQuery()

                        MessageBox.Show("Turf details removed successfully!")
                    End Using
                End Using

                ' Reload Turfs data to update the DataGridView
                LoadTurfData()
            Catch ex As Exception
                MessageBox.Show("Error: " & ex.Message)
            End Try
        Else
            MessageBox.Show("Please select a turf to remove.")
        End If
    End Sub

    Private Sub Guna2Button13_Click(sender As Object, e As EventArgs) Handles Guna2Button13.Click
        Me.Close()
    End Sub

    Private Sub Guna2Button7_Click(sender As Object, e As EventArgs) Handles Guna2Button7.Click
        Form3.Show()
    End Sub

    Private Sub Guna2Button3_Click(sender As Object, e As EventArgs) Handles Guna2Button3.Click
        Try
            Using connection As New MySqlConnection(connectionString)
                connection.Open()

                Dim query As String = "SELECT * FROM Reports"
                Using cmd As New MySqlCommand(query, connection)
                    Using reader As MySqlDataReader = cmd.ExecuteReader()
                        Dim reportData As New StringBuilder()

                        ' Loop through the rows in the Reports table
                        While reader.Read()
                            ' Assuming you have TurfId, BookingId, and PaymentId columns in the Reports table
                            Dim turfId As Integer = Convert.ToInt32(reader("TurfId"))
                            Dim bookingId As Integer = Convert.ToInt32(reader("BookingId"))
                            Dim paymentId As Integer = Convert.ToInt32(reader("PaymentId"))

                            ' Append the data to the StringBuilder
                            reportData.AppendLine($"ReportId: {reader("ReportId")}, TurfId: {turfId}, BookingId: {bookingId}, PaymentId: {paymentId}")
                        End While

                        ' Display the data in a MessageBox
                        MessageBox.Show(reportData.ToString(), "Reports Data")
                    End Using
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        End Try
    End Sub
End Class
